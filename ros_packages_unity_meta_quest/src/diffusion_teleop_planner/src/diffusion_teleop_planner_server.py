#!/usr/bin/env python

from __future__ import print_function

import numpy as np
import rospy
import matplotlib.pyplot as plt
import torch, pickle, os
import rospkg
from nav_msgs.msg import Path
# import ros_numpy

from geometry_msgs.msg import Point
from nav_msgs.msg import Path
from diffusion_teleop_planner.srv import planner, plannerResponse
from diffusion_teleop_planner.msg import tracker_traj, jointTrajArray

from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint 
from sensor_msgs.msg import JointState

pkg_path = rospkg.RosPack().get_path('diffusion_teleop_planner')


DIFFUSER_PATH = os.path.join(pkg_path, 'src/diffusion_teleop')
MODEL_PATH = os.path.join(DIFFUSER_PATH, "model/diffusion_teleop_100_Hz_512/model-750.pt")

# Edit the path so the module can read from the Diffusion planner library
import sys
    # caution: path[0] is reserved for script path (or '' in REPL)
sys.path.insert(2, DIFFUSER_PATH)

from diffusion_teleop.model import diffusion_planner

device = "cuda" if torch.cuda.is_available() else "cpu"


class diffusion_teleop_planner_server:
    def __init__(self):
        rospy.init_node('diffusion_teleop_planner_server', anonymous=True)
        

        print("Loading diffuser weights")
        self.diffuser_planner = diffusion_planner(device = device)
        self.diffuser_planner.load_weight(MODEL_PATH)

        print("Ready to generate path.")
        s = rospy.Service('diffusion_teleop_planner', planner, 
            self.handle_diffusion_planning)

        rospy.spin()

    def handle_diffusion_planning(self, req):

        print("Generating path")
        # get the tracker trajectory from the request
        tracker_traj_obs_np = self.points_to_numpy(req.tracker_obs)
        # get the start states of the arm joint as conditioning
        joint_initial_np = self.jointstate_to_numpy(req.arm_init_joint_state)
        # how many steps in the future to predict
        predict_horizon = req.predict_horizon
        

        print("Network input constructed")
        print("device used is ", device)

        print("Starting diffusion planning")
        # get the predicted trajectory
        predicted_traj_np, diffusion_hist_np = self.diffuser_planner.plan(tracker_traj_obs_np, joint_initial_np, predict_horizon)

        print("Diffusion planning successful")
        
        predicted_tracker_traj_np = predicted_traj_np[:,:3]
        predicted_arm_traj_np = predicted_traj_np[:,3:]
        diffusion_hist_np_tracker = diffusion_hist_np[:,:,:3]
        diffusion_hist_np_arm = diffusion_hist_np[:,:,3:]


        # convert the predicted trajectory to JointTrajectory
        predicted_arm_traj_msg = self.numpy_to_JointTrajectory(predicted_arm_traj_np)

        resp = plannerResponse()
        resp.arm_pred_joint_traj = predicted_arm_traj_msg
        resp.diffusion_hist_arm = self.numpy_to_JointTrajectory_array(diffusion_hist_np_arm)
        resp.diffusion_hist_tracker = self.numpy_to_points_array(diffusion_hist_np_tracker)
        resp.success = True
        return resp

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
    
    def numpy_to_points_array(self, np_array):
        """Convert numpy array to a list of geometry_msgs/Point.

        Args:
            np_array (numpy.ndarray): A numpy array.

        Returns:
            list: A list of geometry_msgs/Point.
        """
        tracker_traj_array = []
        for i in range(np_array.shape[0]):
            tracker_traj_msg = tracker_traj()
            tracker_traj_msg.tracker_traj = self.numpy_to_points(np_array[i,:,:])
            tracker_traj_array.append(tracker_traj_msg)

        return tracker_traj_array
    
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
            point.positions = list(np_array[i,:])
            joint_trajectory.points.append(point)

        return joint_trajectory

    def numpy_to_JointTrajectory_array(self, np_array):
        """Convert numpy array to a list of sensor_msgs/JoinTrajectory.

        Args:
            np_array (numpy.ndarray): A numpy array.

        Returns:
            list: A list of sensor_msgs/JoinTrajectory.
        """
        joint_trajectory_array = jointTrajArray()

        for i in range(np_array.shape[0]):
            joint_trajectory = self.numpy_to_JointTrajectory(np_array[i,:,:])
            joint_trajectory_array.jointTrajArray.append(joint_trajectory)

        return joint_trajectory_array
    def points_to_numpy(self, points):
        """Convert a list of geometry_msgs/Point to a numpy array.

        Args:
            points (list): A list of geometry_msgs/Point.

        Returns:
            numpy.ndarray: A numpy array.
        """
        points_np = []
        for point in points:
            points_np.append([point.x, point.y, point.z])

        return np.array(points_np)
    
    def jointstate_to_numpy(self, joint_state):
        """Convert a sensor_msgs/JointState to a numpy array.

        Args:
            joint_state (sensor_msgs/JointState): A sensor_msgs/JointState.

        Returns:
            numpy.ndarray: A numpy array.
        """
        return np.array(joint_state.position)

    def joint_trajectories_to_numpy(self, joint_trajectory):
        """Convert a list of sensor_msgs/JointState to a numpy array.

        Args:
            joint_trajectory (list): A list of sensor_msgs/JointState.

        Returns:
            numpy.ndarray: A numpy array.
        """
        joint_trajectory_np = []
        for joint_state in joint_trajectory:
            joint_trajectory_np.append(joint_state.position)

        return np.array(joint_trajectory_np)


if __name__ == "__main__":

    planner_server = diffusion_teleop_planner_server()
