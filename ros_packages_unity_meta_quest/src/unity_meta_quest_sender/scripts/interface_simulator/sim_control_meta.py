import sys 
import os
sys.path.append(os.path.dirname(__file__))
from std_msgs.msg import Float64MultiArray
import matplotlib.pyplot as plt
import numpy as np
import rospy
import time
from scipy.spatial.transform import Rotation as R
from sensor_msgs.msg import JointState 
from unity_meta_quest_msgs.msg import ControllerState
from simulation_and_control import pb, MotorCommands, PinWrapper, feedback_lin_ctrl, SinusoidalReference, ImpedanceController
import pinocchio as pin
from collections import deque

initial_q= [0.00, -0.785, 0.000,-2.356, 0.000, 1.571, 0.785]
# Callback function for receiving the Cartesian position
cur_q = initial_q
#ori_des=[0.7071, 0.0, 0.7071, 0.0]
# 9.23955699e-01 -3.82499497e-01  8.14422855e-17  1.36283715e-16
# Initialize the ROS node
rospy.init_node('sim_interface', anonymous=True) 
joint_state = JointState()
joint_state.name = ['joint_0', 'joint_1', 'joint_2', 'joint_3', 'joint_4', 'joint_5', 'joint_6','joint_7']  # Your joint names
joint_state.position = [0.00, -0.785, 0.000,-2.356, 0.000, 1.571, 0.785, 0.00]

cur_qd= [0.00, 0.00, 0.000, 0.00, 0.000, 0.00, 0.000]


# flag to switch between modalities (we start with the grabbing modality by default)
grab_the_end_effector = True
# global parameter to increase the velocity of the controller in the 3 directions
joystick_velocity_cmd_factor = [4.0, 4.0, 4.0]


conf_file_name = "pandaconfig.json"  # Configuration file for the robot
root_dir = os.path.dirname(os.path.abspath(__file__))
# added this line to manage the fact that the file is in tests folder
name_current_directory = "interface_simulator"
# remove current directory name from cur_dir
root_dir = root_dir.replace(name_current_directory, "")
# Configuration for the simulation
sim = pb.SimInterface(conf_file_name, conf_file_path_ext = root_dir)  # Initialize simulation interface

# Get active joint names from the simulation
ext_names = sim.getNameActiveJoints()
ext_names = np.expand_dims(np.array(ext_names), axis=0)  # Adjust the shape for compatibility

source_names = ["pybullet"]  # Define the source for dynamic modeling
dyn_model = PinWrapper(conf_file_name, "pybullet", ext_names, source_names, False,0,root_dir)
num_joints = dyn_model.getNumberofActuatedJoints()
# global reference variables
cartesian_position_des,ori_des= dyn_model.ComputeFK(initial_q,"panda_link8")
cartesian_velocity_des = np.array([0.0,0.0,0.0])
# just for debugging
ori_des_vec = ori_des

ori_des=pin.Quaternion(ori_des)
p_des_buffer= deque(maxlen=50)
pd_des_buffer= deque(maxlen=50)
ori_des_buffer= deque(maxlen=50)
prev_pdes= deque(maxlen=50)


kp_pos= 100.0
kp_ori= 10.0
# create the PD gains matrices
kp_pos_vec = np.ones(3)*kp_pos
kp_ori_vec = np.ones(3)*kp_ori
P_pos = np.eye(3) * kp_pos
P_ori = np.eye(3)* kp_ori

# this is used if want to do both position and orientation control
P_tot = np.diag(np.concatenate((kp_pos_vec,kp_ori_vec)))


def applyJointVelSaturation(qd_des,joint_vel_saturation):
    """
    Apply joint velocity saturation to the desired joint velocity.
    
    Parameters:
        qd_des (np.array): The desired joint velocity.
        joint_vel_saturation (float): The joint velocity saturation value.
    
    Returns:
        np.array: The desired joint velocity with saturation applied.
    """
    # Apply joint velocity saturation
    qd_des_clipped = np.clip(qd_des, -joint_vel_saturation, joint_vel_saturation)
    # Check if clipping has occurred
    clip_happened = not np.array_equal(qd_des, qd_des_clipped)
    if clip_happened:
        print("Joint velocity saturation occurred! check the desired joint velocity")
    return qd_des_clipped

def apply_dead_zone(velocity,thresh):
  """
  Apply a dead zone to the velocity vector.
  
  Parameters:
      velocity (np.array): The velocity vector.
      threshold (float): The threshold below which velocities are set to zero.
      
  Returns:
      np.array: The velocity vector with the dead zone applied.
  """
  # Apply dead zone to each component of the velocity
  dead_zone_velocity = np.where(np.abs(velocity) < thresh, 0, velocity)
  return dead_zone_velocity
   
def estimateCartesianPosVelocity(cur_pdes, delta_t):
    """
    Estimate the Cartesian velocity of the end effector.
    
    Parameters:
        cur_pdes (np.array): The current desired position.
        cur_time (float): The current time.
        
    Returns:
        np.array: The estimated Cartesian velocity.
    """
    global prev_pdes
    if(len(prev_pdes)==0):
        pd_des = np.zeros(3)
    else:
        pd_des = (cur_pdes-prev_pdes)/delta_t
    # Update prev_pdes and prev_time
    prev_pdes = cur_pdes.copy()
    
    return pd_des 

def updateMovingAverage( p_des, pd_des, ori_des):
    """
    Update the moving average buffers for the desired position and orientation.
    
    Parameters:
        p_des (np.array): The desired position.
        pd_des (np.array): The desired velocity.
        ori_des (np.array): The desired orientation.
        ori_des (np.array): The desired angular velocity.
    """
    global p_des_buffer
    global pd_des_buffer
    global ori_des_buffer
    p_des_buffer.append(p_des)
    pd_des_buffer.append(pd_des)
    ori_des_buffer.append(ori_des)
    #ori_vel_des_buffer.append(ori_vel_des)

def GetMovingAverage():
    """
    Get the moving average of the desired position and orientation.
    
    Returns:
        np.array: The moving average of the desired position.
        np.array: The moving average of the desired velocity.
        np.array: The moving average of the desired orientation.
        np.array: The moving average of the desired angular velocity.
    """
    global p_des_buffer
    global pd_des_buffer
    global ori_des_buffer

    #for now we keep it empty
    ori_des_moving_avg = []

    p_des_moving_avg = np.mean(p_des_buffer, axis=0)
    pd_des_moving_avg = np.mean(pd_des_buffer, axis=0)
    # for now we do not change the orientation so we do not need to filter it
    #ori_des_moving_avg = np.mean(ori_des_buffer, axis=0)
    #ori_vel_des_moving_avg = np.mean(ori_vel_des_buffer, axis=0)
    return p_des_moving_avg, pd_des_moving_avg , ori_des_moving_avg

def DataPreprocessing(cur_pdes,cur_pd_des,cur_ori_des, delta_time):
    global grab_the_end_effector
    if grab_the_end_effector:
         # grabbing modality
        pd_des = estimateCartesianPosVelocity(cur_pdes, delta_time)
    else:
        # joystick modality
        pd_des = cur_pd_des
        #cur_pdes = cur_pdes + delta_time*cur_pd_des
        #pd_des = cur_pd_des
    # compute the desired orientation velocity
    #ori_vel_des = self.estimateCartesianAngVelocity(cur_ori_des, delta_time)
    updateMovingAverage(cur_pdes, pd_des, cur_ori_des)
    p_des_moving_avg, pd_des_moving_avg,ori_des_moving_average = GetMovingAverage()
    # here i appply the dead zone to remove vibration from the phasespace
    pd_des_moving_avg = apply_dead_zone(pd_des_moving_avg,0.01)

    return p_des_moving_avg, pd_des_moving_avg, ori_des_moving_average

def computeInverseDiffKinematics(cur_q, p_des, pd_des,ori_des, delta_t, ori_pos_both):
    
    global P_tot
    global P_ori
    global P_pos
    dead_zone_thresh_joints = 0.01
    joint_vel_saturation = np.array([1000,1000,1000,1000,1000,1000,1000])
    cur_cartesian_pos,Cur_R = dyn_model.ComputeFK(cur_q, "panda_link8")


    dyn_model.ComputeJacobian(cur_q,"panda_link8","local_global")
 

    # Since we are only interested in linear velocity, we select the first three rows of the Jacobian
    J_linear = dyn_model.res.J[:3, :]
    J_angular = dyn_model.res.J[3:, :]

    # compute ori des in quaternion
    #Cur_R =pin.rpy.rpyToMatrix(cur_cartesian_ori)
    cur_quat = pin.Quaternion(Cur_R)
    cur_quat = cur_quat.normalize()
    ori_des_quat = ori_des
    ori_des_quat = ori_des_quat.normalize()

    # Ensure quaternion is in the same hemisphere as the desired orientation
    cur_quat_coeff = cur_quat.coeffs()
    ori_des_quat_coeff = ori_des_quat.coeffs()
    if np.dot(cur_quat_coeff, ori_des_quat_coeff) < 0.0:
        cur_quat_coeff = cur_quat_coeff * -1.0
        cur_quat = pin.Quaternion(cur_quat_coeff)

    # Compute the "difference" quaternion (assuming orientation_d is also a pin.Quaternion object)
    angle_error_quat = cur_quat.inverse() * ori_des_quat
    # extract coefficient x y z from the quaternion
    angle_error = angle_error_quat.coeffs()
    angle_error = angle_error[:3]
    
    # rotate the angle error in the base frame
    angle_error_base_frame = Cur_R@angle_error


    # computing position error
    pos_error = p_des - cur_cartesian_pos

    print("pos error= ", pos_error,"---------------------")

    if(ori_pos_both=="pos"):
      cur_J = J_linear
      cur_P = P_pos  
      cur_error = pos_error 
      cur_d_des = pd_des   
    if (ori_pos_both=="ori"):
      cur_J = J_angular
      cur_P = P_ori
      cur_error = angle_error_base_frame
      cur_d_des = np.zeros(3)
    if (ori_pos_both=="both"):
      cur_J = dyn_model.res.J
      cur_P = P_tot
      cur_error = np.concatenate((pos_error,angle_error_base_frame),axis=0)
      cur_d_des = np.concatenate((pd_des,np.zeros(3)),axis=0)      

    # DEBUG
    print("cur_des_cartesian_velocity")
    print(cur_d_des)

    # here i compute the desired joint velocity
    qd_des= np.linalg.pinv(cur_J) @ (cur_P@(cur_error) + cur_d_des)

    # DEBUG
    print("qd_des_after_jacobian")
    print(qd_des)

    qd_des_dead_zone = apply_dead_zone(qd_des,dead_zone_thresh_joints)
    # DEBUG
    print("qd_des_dead_zone")
    print(qd_des_dead_zone)

    qd_des_clip = applyJointVelSaturation(qd_des_dead_zone,joint_vel_saturation)

    # DEBUG
    print("final_qd_des_after_clipping")
    print(qd_des_clip)

    # DEBUG (zeoring any reference to make it not moving)
    #qd_des_clip = np.zeros(7)

    # here i compute the desired joint position
    q_des = dyn_model.KinematicIntegration(cur_q, qd_des_clip, delta_t)

    # DEBUG (print)
    #print("-------------------------------------------------------------------------")
    # Calculate the determinant
    det_J = np.linalg.det(cur_J@cur_J.T)

    # Check if the matrix is singular
    #if np.isclose(det_J, 0):
    #    print("The Jacobian is singular.")
    #else:
    #   print("The Jacobian is not singular.")
    #print("cur_q",cur_q)
    #print("cur_cartesian_pos",cur_cartesian_pos)
    #print("des_cartesian_pos",p_des)
    #print("des_ori",ori_des)
    #print("cur_cartesian_ori",Cur_R)
    #print("error=",cur_error)
    #print("cur_d_des=",cur_d_des)
    #print("current desired joint velocity=",qd_des)
    #print("current desired joint velocity with deadzone=",qd_des_dead_zone)
    
    
    return q_des, qd_des_clip
  
def feedback_lin_ctrl(dyn_model, q_, qd_, q_d, qd_d, kp, kd):
    """
    Perform feedback linearization control on a robotic system.

    Parameters:
    - dyn_model (pin_wrapper): The dynamics model of the robot encapsulated within a 'pin_wrapper' object,
                               which provides methods for computing robot dynamics such as mass matrices,
                               Coriolis forces, etc.
    - q_ (numpy.ndarray): Measured positions of the robot's joints, indicating the current actual positions
                          as measured by sensors or estimated by observers.
    - qd_ (numpy.ndarray): Measured velocities of the robot's joints, reflecting the current actual velocities.
    - q_d (numpy.ndarray): Desired positions for the robot's joints set by a trajectory generator or a higher-level
                           controller, dictating target positions.
    - qd_d (numpy.ndarray): Desired velocities for the robot's joints, specifying the rate at which the joints
                            should move towards their target positions.
    - kp (float or numpy.ndarray): Proportional gain(s) for the control system, which can be a uniform value across
                                   all joints or unique for each joint, adjusting the response to position error.
    - kd (float or numpy.ndarray): Derivative gain(s), similar to kp, affecting the response to velocity error and
                                   aiding in system stabilization by damping oscillations.

    Returns:
    None

    This function computes the control inputs necessary to achieve desired joint positions and velocities by
    applying feedback linearization, using the robot's dynamic model to appropriately compensate for its
    inherent dynamics. The control law implemented typically combines proportional-derivative (PD) control
    with dynamic compensation to achieve precise and stable motion.
    """
    # here i want to add a control that if the kp gains are a numpy vector ill check if the size is good
    #  and then i will create the P matrix and D matrix using the vector a diagonal of the matrices
    # if the size is not good i will raise an error
    # if the kp is a scalar i will create a diagonal matrix with the scalar value
    # same for kd
    if isinstance(kp, np.ndarray):
        if kp.size != dyn_model.getNumberofActuatedJoints():
            raise ValueError("The size of the kp vector is not correct") 
    else:
        kp = np.array([kp] * dyn_model.getNumberofActuatedJoints())
    
    P = np.diag(kp)
    
    if isinstance(kd, np.ndarray):
        if kd.size != dyn_model.getNumberofActuatedJoints():
            raise ValueError("The size of the kd vector is not correct")
    else:
        kd = np.array([kd] * dyn_model.getNumberofActuatedJoints())
    D = np.diag(kd)
 
    # here i compute the feeback linearization tau // the reordering is already done inside compute all teamrs
    dyn_model.ComputeAllTerms(q_, qd_)
    # reoder measurements in pinocchio order
    q_mes = dyn_model.ReoderJoints2PinVec(q_,"pos")
    qd_mes = dyn_model.ReoderJoints2PinVec(qd_,"vel")
    # control 
    u = P @ (q_d - q_mes) + D @ (qd_d - qd_mes)
    n = dyn_model.res.c + dyn_model.res.g
    
    tau_FL = dyn_model.res.M @ u + n

    tau_FL = dyn_model._FromPinToExtVec(tau_FL)
  
    return tau_FL

def reading_from_sensor(msg):
    global cartesian_position_des
    global grab_the_end_effector
    #print("reading new cart posiion")
    #print(cartesian_position_des)
    
    # if im controlling with the joystick the ball should stay attached to the end effector hence we dont use it for ball position
    # so we need to always read it 
    cartesian_position_des = np.array(msg.data[:3])  # Extract x, y, z position from msg
    

def reading_from_left(msg):
    global cartesian_velocity_des
    global grab_the_end_effector
    global p_des_buffer
    global pd_des_buffer
    global ori_des_buffer
    global prev_pdes
    global joystick_velocity_cmd_factor
    # mechanics for switching
    if msg.primary_button_pressed == True and not grab_the_end_effector:
        grab_the_end_effector = True
        print("grab the end effector active")
        # empty the buffer
        #p_des_buffer.clear()
        #pd_des_buffer.clear()
        #ori_des_buffer.clear()
        #prev_pdes.clear()
    
    if msg.secondary_button_pressed == True and grab_the_end_effector:
        grab_the_end_effector = False
        print("joystick active")
        # empty the buffer
        #p_des_buffer.clear()
        #pd_des_buffer.clear()
        #ori_des_buffer.clear()
        #prev_pdes.clear()
        # reinitialize the velocity vector to zero 
        cartesian_velocity_des = np.array([0.0,0.0,0.0])


    # command mechanics
    # inertial frame x direction
    cartesian_velocity_des[0]=joystick_velocity_cmd_factor[0] * msg.thumbstick_y
    # inertial frame y direction
    cartesian_velocity_des[1]=joystick_velocity_cmd_factor[1] * -msg.thumbstick_x

def reading_from_right(msg):
    global cartesian_velocity_des
    global grab_the_end_effector
    global joystick_velocity_cmd_factor

    # command mechanics
    # inertial frame z direction
    cartesian_velocity_des[2]=joystick_velocity_cmd_factor[2] * msg.thumbstick_y   

    

    
rospy.Subscriber("/sphere_pos", Float64MultiArray, reading_from_sensor)
rospy.Subscriber("/controller_left_state",ControllerState,reading_from_left)
rospy.Subscriber("/controller_right_state",ControllerState,reading_from_right)

def main():

    global cur_q
    global ori_des
    # for testing purpose DEBUG (to remove for the real thing)
    global cartesian_position_des
    global cartesian_velocity_des
    kp = 1000
    kd = 100

    # DEBUG (to remove for the real thing) for testing purpose
    #cartesian_position_des = np.array([0.5059863328933716, -0.1572803109884262, 0.2218722820281982])
    frequency=400
    rate = rospy.Rate(frequency)  # 30 Hz, adjust this based on your control loop needs
    # Publisher to send joint states
    joint_pub = rospy.Publisher('/joint_states', JointState, queue_size=1)
    while not rospy.is_shutdown():
        #print(joint_state)
        #print("main cartesian position")
        #print(cartesian_position_des)
        processed_data_p_des_moving_avg, processed_data_pd_des_moving_avg, processed_data_ori_des_moving_average = DataPreprocessing(cartesian_position_des,cartesian_velocity_des,ori_des,0.01)

        # DEBUG
        print("printing desired velocity")
        print(processed_data_pd_des_moving_avg)

        q_des, qd_des_clip = computeInverseDiffKinematics(cur_q, processed_data_p_des_moving_avg, processed_data_pd_des_moving_avg,ori_des, 0.01, "both")

        joint_state.position = np.append(q_des,0.00)

        cur_q=q_des

        sim.SetjointPosition(cur_q)
        #print(joint_state.position)
        joint_pub.publish(joint_state)
        rate.sleep()



if __name__ == '__main__':
    main()