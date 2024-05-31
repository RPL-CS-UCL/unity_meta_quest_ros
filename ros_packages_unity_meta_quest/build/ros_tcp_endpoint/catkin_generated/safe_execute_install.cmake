execute_process(COMMAND "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/build/ros_tcp_endpoint/catkin_generated/python_distutils_install.sh" RESULT_VARIABLE res)

if(NOT res EQUAL 0)
  message(FATAL_ERROR "execute_process(/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/build/ros_tcp_endpoint/catkin_generated/python_distutils_install.sh) returned error code ")
endif()
