# generated from genmsg/cmake/pkg-genmsg.cmake.em

message(STATUS "unity_meta_quest_msgs: 3 messages, 2 services")

set(MSG_I_FLAGS "-Iunity_meta_quest_msgs:/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg;-Igeometry_msgs:/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg;-Istd_msgs:/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/std_msgs/cmake/../msg")

# Find all generators
find_package(gencpp REQUIRED)
find_package(geneus REQUIRED)
find_package(genlisp REQUIRED)
find_package(gennodejs REQUIRED)
find_package(genpy REQUIRED)

add_custom_target(unity_meta_quest_msgs_generate_messages ALL)

# verify that message/service dependencies have not changed since configure



get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg" NAME_WE)
add_custom_target(_unity_meta_quest_msgs_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "unity_meta_quest_msgs" "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg" ""
)

get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg" NAME_WE)
add_custom_target(_unity_meta_quest_msgs_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "unity_meta_quest_msgs" "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg" ""
)

get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg" NAME_WE)
add_custom_target(_unity_meta_quest_msgs_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "unity_meta_quest_msgs" "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg" "unity_meta_quest_msgs/PosRot"
)

get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv" NAME_WE)
add_custom_target(_unity_meta_quest_msgs_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "unity_meta_quest_msgs" "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv" "geometry_msgs/Pose:geometry_msgs/Quaternion:geometry_msgs/Point"
)

get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv" NAME_WE)
add_custom_target(_unity_meta_quest_msgs_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "unity_meta_quest_msgs" "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv" "unity_meta_quest_msgs/PosRot"
)

#
#  langs = gencpp;geneus;genlisp;gennodejs;genpy
#

### Section generating for lang: gencpp
### Generating Messages
_generate_msg_cpp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_cpp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_cpp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Services
_generate_srv_cpp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv"
  "${MSG_I_FLAGS}"
  "/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Pose.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Quaternion.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Point.msg"
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_srv_cpp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Module File
_generate_module_cpp(unity_meta_quest_msgs
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs
  "${ALL_GEN_OUTPUT_FILES_cpp}"
)

add_custom_target(unity_meta_quest_msgs_generate_messages_cpp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_cpp}
)
add_dependencies(unity_meta_quest_msgs_generate_messages unity_meta_quest_msgs_generate_messages_cpp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_cpp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_cpp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_cpp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_cpp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_cpp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(unity_meta_quest_msgs_gencpp)
add_dependencies(unity_meta_quest_msgs_gencpp unity_meta_quest_msgs_generate_messages_cpp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS unity_meta_quest_msgs_generate_messages_cpp)

### Section generating for lang: geneus
### Generating Messages
_generate_msg_eus(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_eus(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_eus(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Services
_generate_srv_eus(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv"
  "${MSG_I_FLAGS}"
  "/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Pose.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Quaternion.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Point.msg"
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_srv_eus(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Module File
_generate_module_eus(unity_meta_quest_msgs
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs
  "${ALL_GEN_OUTPUT_FILES_eus}"
)

add_custom_target(unity_meta_quest_msgs_generate_messages_eus
  DEPENDS ${ALL_GEN_OUTPUT_FILES_eus}
)
add_dependencies(unity_meta_quest_msgs_generate_messages unity_meta_quest_msgs_generate_messages_eus)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_eus _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_eus _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_eus _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_eus _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_eus _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(unity_meta_quest_msgs_geneus)
add_dependencies(unity_meta_quest_msgs_geneus unity_meta_quest_msgs_generate_messages_eus)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS unity_meta_quest_msgs_generate_messages_eus)

### Section generating for lang: genlisp
### Generating Messages
_generate_msg_lisp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_lisp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_lisp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Services
_generate_srv_lisp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv"
  "${MSG_I_FLAGS}"
  "/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Pose.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Quaternion.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Point.msg"
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_srv_lisp(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Module File
_generate_module_lisp(unity_meta_quest_msgs
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs
  "${ALL_GEN_OUTPUT_FILES_lisp}"
)

add_custom_target(unity_meta_quest_msgs_generate_messages_lisp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_lisp}
)
add_dependencies(unity_meta_quest_msgs_generate_messages unity_meta_quest_msgs_generate_messages_lisp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_lisp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_lisp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_lisp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_lisp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_lisp _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(unity_meta_quest_msgs_genlisp)
add_dependencies(unity_meta_quest_msgs_genlisp unity_meta_quest_msgs_generate_messages_lisp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS unity_meta_quest_msgs_generate_messages_lisp)

### Section generating for lang: gennodejs
### Generating Messages
_generate_msg_nodejs(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_nodejs(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_nodejs(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Services
_generate_srv_nodejs(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv"
  "${MSG_I_FLAGS}"
  "/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Pose.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Quaternion.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Point.msg"
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_srv_nodejs(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Module File
_generate_module_nodejs(unity_meta_quest_msgs
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs
  "${ALL_GEN_OUTPUT_FILES_nodejs}"
)

add_custom_target(unity_meta_quest_msgs_generate_messages_nodejs
  DEPENDS ${ALL_GEN_OUTPUT_FILES_nodejs}
)
add_dependencies(unity_meta_quest_msgs_generate_messages unity_meta_quest_msgs_generate_messages_nodejs)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_nodejs _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_nodejs _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_nodejs _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_nodejs _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_nodejs _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(unity_meta_quest_msgs_gennodejs)
add_dependencies(unity_meta_quest_msgs_gennodejs unity_meta_quest_msgs_generate_messages_nodejs)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS unity_meta_quest_msgs_generate_messages_nodejs)

### Section generating for lang: genpy
### Generating Messages
_generate_msg_py(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_py(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_msg_py(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Services
_generate_srv_py(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv"
  "${MSG_I_FLAGS}"
  "/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Pose.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Quaternion.msg;/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/share/geometry_msgs/cmake/../msg/Point.msg"
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs
)
_generate_srv_py(unity_meta_quest_msgs
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv"
  "${MSG_I_FLAGS}"
  "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg"
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs
)

### Generating Module File
_generate_module_py(unity_meta_quest_msgs
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs
  "${ALL_GEN_OUTPUT_FILES_py}"
)

add_custom_target(unity_meta_quest_msgs_generate_messages_py
  DEPENDS ${ALL_GEN_OUTPUT_FILES_py}
)
add_dependencies(unity_meta_quest_msgs_generate_messages unity_meta_quest_msgs_generate_messages_py)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/ControllerState.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_py _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRot.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_py _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/msg/PosRotList.msg" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_py _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/ObjectPoseService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_py _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_msgs/srv/PositionService.srv" NAME_WE)
add_dependencies(unity_meta_quest_msgs_generate_messages_py _unity_meta_quest_msgs_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(unity_meta_quest_msgs_genpy)
add_dependencies(unity_meta_quest_msgs_genpy unity_meta_quest_msgs_generate_messages_py)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS unity_meta_quest_msgs_generate_messages_py)



if(gencpp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/unity_meta_quest_msgs
    DESTINATION ${gencpp_INSTALL_DIR}
  )
endif()
if(TARGET geometry_msgs_generate_messages_cpp)
  add_dependencies(unity_meta_quest_msgs_generate_messages_cpp geometry_msgs_generate_messages_cpp)
endif()
if(TARGET std_msgs_generate_messages_cpp)
  add_dependencies(unity_meta_quest_msgs_generate_messages_cpp std_msgs_generate_messages_cpp)
endif()

if(geneus_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/unity_meta_quest_msgs
    DESTINATION ${geneus_INSTALL_DIR}
  )
endif()
if(TARGET geometry_msgs_generate_messages_eus)
  add_dependencies(unity_meta_quest_msgs_generate_messages_eus geometry_msgs_generate_messages_eus)
endif()
if(TARGET std_msgs_generate_messages_eus)
  add_dependencies(unity_meta_quest_msgs_generate_messages_eus std_msgs_generate_messages_eus)
endif()

if(genlisp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/unity_meta_quest_msgs
    DESTINATION ${genlisp_INSTALL_DIR}
  )
endif()
if(TARGET geometry_msgs_generate_messages_lisp)
  add_dependencies(unity_meta_quest_msgs_generate_messages_lisp geometry_msgs_generate_messages_lisp)
endif()
if(TARGET std_msgs_generate_messages_lisp)
  add_dependencies(unity_meta_quest_msgs_generate_messages_lisp std_msgs_generate_messages_lisp)
endif()

if(gennodejs_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/unity_meta_quest_msgs
    DESTINATION ${gennodejs_INSTALL_DIR}
  )
endif()
if(TARGET geometry_msgs_generate_messages_nodejs)
  add_dependencies(unity_meta_quest_msgs_generate_messages_nodejs geometry_msgs_generate_messages_nodejs)
endif()
if(TARGET std_msgs_generate_messages_nodejs)
  add_dependencies(unity_meta_quest_msgs_generate_messages_nodejs std_msgs_generate_messages_nodejs)
endif()

if(genpy_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs)
  install(CODE "execute_process(COMMAND \"/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/bin/python3.9\" -m compileall \"${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs\")")
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/unity_meta_quest_msgs
    DESTINATION ${genpy_INSTALL_DIR}
  )
endif()
if(TARGET geometry_msgs_generate_messages_py)
  add_dependencies(unity_meta_quest_msgs_generate_messages_py geometry_msgs_generate_messages_py)
endif()
if(TARGET std_msgs_generate_messages_py)
  add_dependencies(unity_meta_quest_msgs_generate_messages_py std_msgs_generate_messages_py)
endif()
