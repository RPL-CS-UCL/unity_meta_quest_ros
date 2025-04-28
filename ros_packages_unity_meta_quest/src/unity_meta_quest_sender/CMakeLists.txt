cmake_minimum_required(VERSION 2.8.3)
project(unity_meta_quest_sender)

find_package(catkin REQUIRED COMPONENTS
  rospy
  std_msgs
  geometry_msgs
  ros_tcp_endpoint
  message_generation
)

catkin_package(CATKIN_DEPENDS
    ros_tcp_endpoint
    message_runtime)

catkin_install_python(PROGRAMS
  scripts/image_publisher.py
  scripts/move_base.py
  scripts/image_data_collection.py
  scripts/move_base.py
  scripts/franka_teleop.py
  scripts/interface_simulator/sim_control_meta.py
  DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION}
)


#############
## Testing ##
#############

## Add gtest based cpp test target and link libraries
# catkin_add_gtest(${PROJECT_NAME}-test test/test_robotics_demo.cpp)
# if(TARGET ${PROJECT_NAME}-test)
#   target_link_libraries(${PROJECT_NAME}-test ${PROJECT_NAME})
# endif()

## Add folders to be run by python nosetests
# catkin_add_nosetests(test)
