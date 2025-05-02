#!/usr/bin/env python

from __future__ import print_function

import numpy as np
import rospy
# import ros_numpy
import rospkg, os, glob


from std_msgs.msg import Bool
from geometry_msgs.msg import Point
from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint 
from sensor_msgs.msg import JointState
from diffusion_teleop_planner.srv import planner, plannerRequest, plannerResponse
from diffusion_teleop_planner.msg import tracker_traj, jointTrajArray

class diffusion_teleop_planner_client:
    def __init__(self):
        rospy.init_node('diffusion_teleop_planner_client_test_node', anonymous=True)


        rospy.wait_for_service('diffusion_teleop_planner')

        self.animate_diffusion = rospy.get_param('~animate_diffusion', False)
        self.animate_fps = rospy.get_param('~animate_fps', 100)
        self.animate_every_x_frames = rospy.get_param('~animate_every_x_frames', 10)

        self.planner_client = rospy.ServiceProxy('diffusion_teleop_planner', planner) 

        self.start_srv_sub = rospy.Subscriber('/diffusion_teleop_planner/start_service', Bool, self.start_service)
        self.diffusion_arm_traj_pub = rospy.Publisher("arm_joint_traj_diffusion", JointTrajectory, queue_size=1000)
        self.diffusion_tracker_traj_pub = rospy.Publisher("tracker_traj_diffusion", tracker_traj, queue_size=1000)

        #----------------------Replace with the actual data from live source ----------------------#
        # load some fake data from the training set for testing
        self.joint_trajectories, self.tracker_trajectories = self.load_nps()
        # get the first 50 points of the tracker trajectory as conditioning
        tracker_traj_obs_np = self.tracker_trajectories[1][:250,:]
        # get the start states of the arm joint as conditioning
        joint_initial_np = self.joint_trajectories[1][0,:]
        # how many steps in the future to predict
        predict_horizon = 262
        # ----------------------Replace with the actual data from live source ----------------------#

        # convert input to correct format for the service request
        tracker_traj_obs_msg = self.numpy_to_points(tracker_traj_obs_np)
        joint_initial_msg  = self.numpy_to_JointState(joint_initial_np)
        
        self.request = plannerRequest()
        self.request.tracker_obs = tracker_traj_obs_msg
        self.request.arm_init_joint_state = joint_initial_msg
        self.request.predict_horizon = predict_horizon

        self.start_flag = False
        
        rate = rospy.Rate(self.animate_fps)
        while not rospy.is_shutdown():
            if self.start_flag:
                print("Requesting path from the planner")
                self.planner_response = self.planner_client(self.request)
                self.predicted_traj_msg = self.planner_response.arm_pred_joint_traj
                self.diffusion_hist_arm_msg = self.planner_response.diffusion_hist_arm
                self.diffusion_hist_tracker_msg = self.planner_response.diffusion_hist_tracker

                # convert the predicted trajectory to numpy array
                self.predicted_traj_np = self.joint_trajectories_to_numpy(self.predicted_traj_msg)

                # visualise the results:
                if self.animate_diffusion:
                    for i in np.arange(0,len(self.diffusion_hist_tracker_msg),self.animate_every_x_frames):
                        self.diffusion_arm_traj_pub.publish(self.diffusion_hist_arm_msg.jointTrajArray[i])
                        self.diffusion_tracker_traj_pub.publish(self.diffusion_hist_tracker_msg[i])
                        rate.sleep()
                    # always publish the final predicted trajectory as the last frame
                    self.diffusion_arm_traj_pub.publish(self.diffusion_hist_arm_msg.jointTrajArray[-1])
                    self.diffusion_tracker_traj_pub.publish(self.diffusion_hist_tracker_msg[-1])
                else:
                    self.diffusion_arm_traj_pub.publish(self.predicted_traj_msg)
                    self.diffusion_tracker_traj_pub.publish(self.diffusion_hist_tracker_msg[-1])

                self.start_flag = False

    def start_service(self, start):
        if start.data:
            self.start_flag = True
            print("client start flag set to True")

    def numpy_to_points(self, np_array):
        """Convert numpy array to a list of geometry_msgs/Point.

        Args:
            np_array (numpy.ndarray): A numpy array.

        Returns:
            list: A list of geometry_msgs/Point.
        """
        points = []
        for i in range(np_array.shape[0]):
            point = Point()
            point.x = np_array[i, 0]
            point.y = np_array[i, 1]
            point.z = np_array[i, 2]
            points.append(point)

        return points
    
    def numpy_to_JointState(self, np_array):
        """Convert numpy array to a list of sensor_msgs/JointState.

        Args:
            np_array (numpy.ndarray): A numpy array.

        Returns:
            list: A list of sensor_msgs/JointState.
        """
        joint_state = JointState()
        joint_state.header.stamp = rospy.Time.now()
        joint_state.name = ["panda_joint1", "panda_joint2", "panda_joint3", "panda_joint4", "panda_joint5", "panda_joint6", "panda_joint7"]
        joint_state.position = np_array

        return joint_state
    def numpy_to_JointTrajectory(self, np_array):
        """Convert numpy array to a list of sensor_msgs/JointState.

        Args:
            np_array (numpy.ndarray): A numpy array.

        Returns:
            list: A list of sensor_msgs/JointState.
        """
        joint_trajectory = JointTrajectory()
        joint_trajectory.header.stamp = rospy.Time.now()
        joint_trajectory.joint_names = ["panda_joint1", "panda_joint2", "panda_joint3", "panda_joint4", "panda_joint5", "panda_joint6", "panda_joint7"]
        for i in range(np_array.shape[0]):
            point = JointTrajectoryPoint()
            point.positions = np_array[i]
            joint_trajectory.points.append(point)

        return joint_trajectory
    
    def joint_trajectories_to_numpy(self, joint_trajectory):
        """Convert a list of sensor_msgs/JointState to a numpy array.

        Args:
            joint_trajectory (list): A list of sensor_msgs/JointState.

        Returns:
            numpy.ndarray: A numpy array.
        """
        joint_trajectory_np = []
        for joint_state in joint_trajectory.points:
            joint_trajectory_np.append(joint_state.positions)

        return np.array(joint_trajectory_np)
    
    def load_nps(self):
        """Load numpy files containing joint trajectories.
            
        """
        # Defining ros package path
        rospack = rospkg.RosPack()
        pakage_path = rospack.get_path('diffusion_teleop_vis')
        dataset_path = pakage_path + '/data/dataset_Hz_1k_processed'
        dataset_path = os.path.join(dataset_path, "*.npy")
        datasets = sorted(glob.glob(dataset_path))

        joint_trajectories = []
        tracker_trajectories = []
        for i, dataset in enumerate(datasets):
            path = np.load(dataset)
            joint_trajectory = path[:, 0:7]
            tracker_trajectory = path[:, 7:10]
            joint_trajectories.append(joint_trajectory)
            tracker_trajectories.append(tracker_trajectory)

        return joint_trajectories, tracker_trajectories
    
# Dynamic Time Warping distance metric
def dtw_distance(ts1, ts2):
    """
    Compute the dynamic time warping distance between two time series trajectories.
    
    Parameters:
    ts1 (list): First time series trajectory.
    ts2 (list): Second time series trajectory.
    
    Returns:
    float: Dynamic Time Warping distance between the two trajectories.
    """
    # Lengths of the time series
    n = len(ts1)
    m = len(ts2)
    
    # Initialize cost matrix
    cost_matrix = np.zeros((n, m))
    
    # Compute pairwise distances between points in the time series
    for i in range(n):
        for j in range(m):
            cost_matrix[i, j] = abs(ts1[i] - ts2[j])
    
    # Initialize accumulated cost matrix
    accumulated_cost = np.zeros((n, m))
    accumulated_cost[0, 0] = cost_matrix[0, 0]
    
    # Compute accumulated cost matrix
    for i in range(1, n):
        accumulated_cost[i, 0] = cost_matrix[i, 0] + accumulated_cost[i - 1, 0]
    for j in range(1, m):
        accumulated_cost[0, j] = cost_matrix[0, j] + accumulated_cost[0, j - 1]
    for i in range(1, n):
        for j in range(1, m):
            accumulated_cost[i, j] = cost_matrix[i, j] + min(accumulated_cost[i - 1, j],
                                                             accumulated_cost[i, j - 1],
                                                             accumulated_cost[i - 1, j - 1])
    
    # Return the DTW distance
    return accumulated_cost[-1, -1]

if __name__ == "__main__":

    planner_server = diffusion_teleop_planner_client()
