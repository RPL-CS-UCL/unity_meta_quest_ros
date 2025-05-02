cmake_minimum_required(VERSION 3.0.2)
project(diffusion_teleop_planner)

## Compile as C++11, supported in ROS Kinetic and newer
# add_compile_options(-std=c++11)

## Find catkin macros and libraries
## if COMPONENTS list like find_package(catkin REQUIRED COMPONENTS xyz)
## is used, also find other catkin packages
find_package(catkin REQUIRED COMPONENTS
  roscpp
  rospy  
  geometry_msgs
  trajectory_msgs
  sensor_msgs
  message_generation
  message_runtime
)

## Generate services in the 'srv' folder
add_service_files(
  FILES
  planner.srv
)

add_message_files(
  FILES
  tracker_traj.msg
  jointTrajArray.msg
)

## Generate added messages and services with any dependencies listed here
generate_messages(
  DEPENDENCIES
  geometry_msgs
  std_msgs
  trajectory_msgs
  sensor_msgs
)

catkin_package(
#  INCLUDE_DIRS include
#  LIBRARIES diffusion_teleop_planner
#  CATKIN_DEPENDS roscpp rospy
#  DEPENDS system_lib
)

###########
## Build ##
###########

## Specify additional locations of header files
## Your package locations should be listed before other locations
include_directories(
# include
  ${catkin_INCLUDE_DIRS}
)

