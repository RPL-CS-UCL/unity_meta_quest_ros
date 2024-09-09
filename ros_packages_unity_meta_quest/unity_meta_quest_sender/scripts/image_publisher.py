#!/usr/bin/env python

import rospy
from sensor_msgs.msg import Image, CompressedImage
from std_msgs.msg import Bool
from cv_bridge import CvBridge
import cv2

class ImageCompressor:
    def __init__(self):
        self.bridge = CvBridge()           #/robot/front_ptz_camera/image_color/compressed /robot/front_ptz_camera/image_raw
        #self.image_sub = rospy.Subscriber("/robot/front_ptz_camera/image_color/compressed", CompressedImage, self.image_callback) #/camera/color/image_raw
        #self.image_sub = rospy.Subscriber("/robot/front_ptz_camera/image_color", Image, self.image_callback) #/camera/color/image_raw
        #self.test_sub = rospy.Subscriber("/test1", Bool, self.test_callback)
        self.image_sub = rospy.Subscriber("/robot/front_ptz_camera/image_color", Image, self.image_callback) #summit camera
        #self.image_sub = rospy.Subscriber("/camera/color/image_raw", Image, self.image_callback) #arm camera
        self.compressed_image_pub = rospy.Publisher("/camera/image_compressed2", CompressedImage, queue_size=1)


    def test_callback(self, msg):
        rospy.loginfo("received test message")

    def image_callback(self, msg):
        rospy.loginfo("Received image")
        rospy.loginfo("Image width: %d, height: %d, encoding: %s", msg.width, msg.height, msg.encoding)
        try:
            # Convert ROS Image message to OpenCV image
            cv_image = self.bridge.imgmsg_to_cv2(msg, "rgb8")
            
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
            print("publishing")
            self.compressed_image_pub.publish(compressed_image_msg)
            print("published")

        except rospy.ROSException as e:
            rospy.logerr("Failed to process image: %s", str(e))

if __name__ == '__main__':
    print("starting image_compressor node")
    rospy.init_node('image_compressor', anonymous=True)
    ic = ImageCompressor()
    try:
        print("starting spinning")
        rospy.spin()
    except KeyboardInterrupt:
        rospy.loginfo("Shutting down image compressor node")
