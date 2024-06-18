#!/usr/bin/env python

import rospy
from sensor_msgs.msg import Image, CompressedImage
from cv_bridge import CvBridge
import cv2

class ImageCompressor:
def init(self):
self.bridge = CvBridge()
self.image_sub = rospy.Subscriber("/camera/color/image_raw", Image, self.image_callback)
self.compressed_image_pub = rospy.Publisher("/camera/image_compressed2", CompressedImage, queue_size=1)

def image_callback(self, msg):
    rospy.loginfo("Received image")
    try:
        # Convert ROS Image message to OpenCV image
        cv_image = self.bridge.imgmsg_to_cv2(msg, "rgb8")
        
        # Compress the image
        success, compressed_image = cv2.imedef image_callback(self, msg):
    rospy.loginfo("Received image")
    try:
        # Convert ROS Image message to OpenCV image
        cv_image = self.bridge.imgmsg_to_cv2(msg, "bgr8")
        
        # Compress the image
        success, compressed_image = cv2.imencode('.jpg', cv_image)
        if not success:
            rospy.logerr("Failed to compress image")
            return

        # Create a CompressedImage message
        compressed_image_msg = CompressedImage()
        compressed_image_msg.header = msg.header
        compressed_image_msg.format = "jpeg"
        compressed_image_msg.data = compressed_image.tobytes()

        # Publish the compressed image
        self.compressed_image_pub.publish(compressed_image_msg)

    except rospy.ROSException as e:
        rospy.logerr("Failed to process image: %s", str(e))ncode('.jpg', cv_image)
        if not success:
            rospy.logerr("Failed to compress image")
            return

        # Create a CompressedImage message
        compressed_image_msg = CompressedImage()
        compressed_image_msg.header = msg.header
        compressed_image_msg.format = "jpeg"
        compressed_image_msg.data = compressed_image.tobytes()

        # Publish the compressed image
        self.compressed_image_pub.publish(compressed_image_msg)

    except rospy.ROSException as e:
        rospy.logerr("Failed to process image: %s", str(e))
        
if name == 'main':
rospy.init_node('image_compressor', anonymous=True)
ic = ImageCompressor()
try:
rospy.spin()
except KeyboardInterrupt:
rospy.loginfo("Shutting down image compressor node")

