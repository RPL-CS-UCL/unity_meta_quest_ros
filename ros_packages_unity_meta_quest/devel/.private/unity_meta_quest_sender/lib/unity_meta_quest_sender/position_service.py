#!/opt/homebrew/Caskroom/miniforge/base/envs/ros_env/bin/python3.9
# -*- coding: utf-8 -*-
# generated from catkin/cmake/template/script.py.in
# creates a relay to a python script source file, acting as that file.
# The purpose is that of a symlink
python_script = '/Users/takuyab/unity_meta_quest_ros/ros_packages_unity_meta_quest/src/unity_meta_quest_sender/scripts/position_service.py'
with open(python_script, 'r') as fh:
    context = {
        '__builtins__': __builtins__,
        '__doc__': None,
        '__file__': python_script,
        '__name__': __name__,
        '__package__': None,
    }
    exec(compile(fh.read(), python_script, 'exec'), context)
