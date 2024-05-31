#!/bin/sh

if [ -n "$DESTDIR" ] ; then
    case $DESTDIR in
        /*) # ok
            ;;
        *)
            /bin/echo "DESTDIR argument must be absolute... "
            /bin/echo "otherwise python's distutils will bork things."
            exit 1
    esac
fi

echo_and_run() { echo "+ $@" ; "$@" ; }

echo_and_run cd "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/ROS-TCP-Endpoint"

# ensure that Python install destination exists
echo_and_run mkdir -p "$DESTDIR/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/install/lib/python3.9/site-packages"

# Note that PYTHONPATH is pulled from the environment to support installing
# into one location when some dependencies were installed in another
# location, #123.
echo_and_run /usr/bin/env \
    PYTHONPATH="/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/install/lib/python3.9/site-packages:/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/build/ros_tcp_endpoint/lib/python3.9/site-packages:$PYTHONPATH" \
    CATKIN_BINARY_DIR="/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/build/ros_tcp_endpoint" \
    "/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/bin/python3.9" \
    "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/ROS-TCP-Endpoint/setup.py" \
    egg_info --egg-base /Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/build/ros_tcp_endpoint \
    build --build-base "/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/build/ros_tcp_endpoint" \
    install \
    --root="${DESTDIR-/}" \
     --prefix="/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/install" --install-scripts="/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/install/bin"
