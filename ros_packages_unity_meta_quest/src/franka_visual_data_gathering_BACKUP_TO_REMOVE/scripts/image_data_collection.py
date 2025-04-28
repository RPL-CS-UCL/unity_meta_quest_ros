#!/usr/bin/env python3

import sys 
import os
script_dir = os.path.dirname(__file__)

import pyfranka_interface as franka
import numpy as np
import rospy
from sensor_msgs.msg import Image, CompressedImage
from std_msgs.msg import Bool
from cv_bridge import CvBridge
import cv2

class FrankaImagePublisher:
    def __init__(self, robot_ip, filename):
        # Initialize the Franka robot
        self.robot = franka.Robot_(robot_ip, False)
        self.filename = filename
        # Initialize CvBridge
        self.bridge = CvBridge()
        # ROS subscriber to get images from the camera
        self.image_sub = rospy.Subscriber("/camera/color/image_raw", Image, self.image_callback)
        # ROS publisher to publish compressed images
        self.compressed_image_pub = rospy.Publisher("/camera/image_compressed", CompressedImage, queue_size=1)
        # Placeholder for the latest image
        self.latest_image = None
        # ROS subscriber for the trigger message
        self.trigger_sub = rospy.Subscriber("franca_start", Bool, self.trigger_callback)
        # Flag to indicate when the robot should start
        self.start_flag = False

    def image_callback(self, msg):
        # Save the latest image received from the camera
        self.latest_image = msg

    def trigger_callback(self, msg):
        # Set the flag to true when the trigger message is received
        if msg.data:
            rospy.loginfo("Trigger received, starting Franka robot.")
            self.start_flag = True
            # Start processing the position arrays
            self.process_arrays_in_file(self.filename)

    def process_array(self, array):
        # Move the Franka robot to the specified position
        print("Processing array:")
        print(array)
        self.robot.move("absolute", array)
        # Capture and publish the image after the movement
        self.capture_and_publish_image()

    def on_reset(self):
        # Move the Franka robot to the reset position
        reset_pos = [-0.02926544, -0.19098003, -0.00311942, -1.93799861, -0.01113504, 1.71863803, 0.77337716]
        self.robot.move_joints(reset_pos, 0.1)
        # Capture and publish the image after resetting
        self.capture_and_publish_image()

    def capture_and_publish_image(self):
        # Ensure there is a valid image to process
        if self.latest_image is None:
            rospy.logwarn("No image received yet. Skipping image capture.")
            return

        rospy.loginfo("Publishing compressed image after movement.")

        try:
            # Convert ROS Image message to OpenCV image
            cv_image = self.bridge.imgmsg_to_cv2(self.latest_image, "rgb8")

            # Compress the image
            success, compressed_image = cv2.imencode('.jpg', cv_image)
            if not success:
                rospy.logerr("Failed to compress image")
                return

            # Create a CompressedImage message
            compressed_image_msg = CompressedImage()
            compressed_image_msg.header = self.latest_image.header
            compressed_image_msg.format = "jpeg"
            compressed_image_msg.data = compressed_image.tobytes()

            # Publish the compressed image
            self.compressed_image_pub.publish(compressed_image_msg)

        except rospy.ROSException as e:
            rospy.logerr("Failed to process image: %s", str(e))

    def process_arrays_in_file(self, filename):
        # Read the entire file into a single string
        with open(filename, 'r') as f:
            file_content = f.read()

        # Split the content into separate array strings or reset markers
        elements = file_content.strip().split('\n\n')

        # Loop through each element
        for element in elements:
            if element.strip().lower() == 'reset':
                # Call the custom reset function
                self.on_reset()
            else:
                # Convert the string to a NumPy array and process it
                array = np.fromstring(element, sep=' ').reshape((4, 4))
                self.process_array(array)
        print("ciao")

if __name__ == '__main__':
    rospy.init_node('franka_move_and_image_capture', anonymous=True)
    robot_ip = '192.168.2.100'
    filename = os.path.join(script_dir, 'position_data.txt')
    fip = FrankaImagePublisher(robot_ip, filename)
    
    try:
        rospy.spin()
    except KeyboardInterrupt:
        rospy.loginfo("Shutting down Franka movement and image capture node.")
