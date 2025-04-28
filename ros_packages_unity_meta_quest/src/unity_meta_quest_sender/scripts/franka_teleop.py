#!/usr/bin/env python3

import os
import rospy
from sensor_msgs.msg import Image, CompressedImage
from std_msgs.msg import Bool, Float64MultiArray
from cv_bridge import CvBridge
import cv2
import pyfranka_interface as franka
import numpy as np
from sensor_msgs.msg import JointState

class FrankaTeleop:
    def __init__(self):
        # Initialize Franka robot and ROS node
        
        rospy.init_node('teleop_exchange', anonymous=True)

        # Flags
        #self.start_flag = False

        # Publishers and Subscribers
        self.trigger_sub = rospy.Subscriber("/franca_start", Bool, self.trigger_callback)
        self.joint_sub = rospy.Subscriber("joint_states", JointState, self.joint_callback)
        self.joint_pub = rospy.Publisher("new_pos", Float64MultiArray, queue_size=1)
        self.cart_sub = rospy.Subscriber("sphere_pos", Float64MultiArray, self.cart_callback)

    # Callback for start trigger
    def trigger_callback(self, msg):
        #f msg.data:
        rospy.loginfo("Trigger received, starting calibration.")
            #self.start_flag = True

    def joint_callback(self, msgIn):
        #if self.start_flag == True:
        #rospy.loginfo("Publishing joints")
        
        joint_positions = msgIn.position
        msgOut = Float64MultiArray()
        msgOut.data = joint_positions
        #print(len(msgOut.data))
        #print(msgOut.data)
        self.joint_pub.publish(msgOut)

    def cart_callback(self, msg):
        print(msg.data)

    # Start the ROS loop
    def start(self):
        rospy.spin()

if __name__ == "__main__":
    teleop = FrankaTeleop()
    teleop.start()
