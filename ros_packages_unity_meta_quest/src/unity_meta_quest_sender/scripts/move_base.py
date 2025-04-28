#!/usr/bin/env python

import rospy
from geometry_msgs.msg import Twist
from unity_meta_quest_msgs.msg import ControllerState

class ControllerProcessor:
    def __init__(self):
        # Initialize the ROS node
        rospy.init_node('move_base', anonymous=True)
        
        # Create a publisher to the /cmd_vel topic
        self.pub = rospy.Publisher('/robot/robotnik_base_control/cmd_vel', Twist, queue_size=10)

        rospy.Subscriber('/controller_right_state', ControllerState, self.callback)
        
        self.rate = rospy.Rate(100)

        while not rospy.is_shutdown():
            self.rate.sleep()

    def callback(self, data):
        thumb_x = data.thumbstick_x
        thumb_y = data.thumbstick_y
        print("received data - thumb_x: %f, thumb_y: %f", thumb_x, thumb_y)

        # Create a Twist message
        move_cmd = Twist()
        
        # Set the desired velocities
        if abs(thumb_y) > 0.9:
            if thumb_y > 0:
                move_cmd.linear.x = 0.1
            else:
                move_cmd.linear.x = -0.1
        else:
            move_cmd.linear.x = 0
        move_cmd.linear.y = 0.0
        move_cmd.linear.z = 0.0
        move_cmd.angular.x = 0.0
        move_cmd.angular.y = 0.0
        if abs(thumb_x) > 0.9:
            if thumb_x > 0:
                move_cmd.angular.z = -0.1
            else:
                move_cmd.angular.z = 0.1
        else:
            move_cmd.angular.z = 0

        # Publish the Twist message
        self.pub.publish(move_cmd)
        print("published")

if __name__ == '__main__':
    try:
        ControllerProcessor()		
    except rospy.ROSInterruptException:
        pass

