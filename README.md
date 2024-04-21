# unity_meta_quest_ros

this package contains the unity app and all the necessary dependencies to stream the meta quest information device over Ros Noetic. i Have only tested the app on board the meta quest visualizer but maybe with the app onboard the windows computer more can be done in term of perfomance

# installation guide

## setting up the oculus visor
first it is necessary to set up the meta quest visor. basically you need to create an account on the meta quest and you need to set it in developer mode.
you can follow this guide with all the instruction for setting up your meta quest: https://developer.oculus.com/documentation/native/android/mobile-device-setup/
after that you need to setup the meta quest link to connect the meta quest to the computer. to do so follow this guide: https://www.meta.com/en-gb/help/quest/articles/headsets-and-accessories/oculus-link/set-up-link/

IMPORTANT to compile and deploy app on the meta quest with unity NEVER use the air link (which is the meta quest link but with wifi) and ONLY use the meta quest link with a usb-c cable connecting the meta quest to the unity machine

## unity installation (unity machine - win)
once the meta quest as been prepared it is time to setup the unity environment. from this link https://docs.unity3d.com/hub/manual/InstallHub.html download the unity hub and follow the instruction for the windows installation.
form the unity hub you can install unity. At the time of this guide was written unity 2022.3 has been used. once unity is installed you need to add the xr interaction tool.
in order to do so go to the tab windows->package manager and then here click on Packages:In project and select unity registry. Then here you can just use the seach bar to find the xr interaction tool. for a comprehensive guide for installing the package go to https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@1.0/manual/index.html and do not forget to download the samples they are quite important.
Once the interaction tool is installed you need to configure the environment to do so follow this [video](https://www.youtube.com/watch?v=zbqHNwDpi6Y&list=PLX8u1QKl_yPD4IQhcPlkqxMt35X2COvm0&index=1) on youtube

## ros installation (ros machine)

on the ros machine (which is the one that receive the messages sent from the metaquest device) first you need to create a mamba environment.
so first install mamba/anaconda. after that you can install the environment using this command

```
mamba env create -f ros_env.yml
conda activate ros_env
```
in a folder of the ros machine create a ros workspace (an empty folder with another empty folder inside called src)
aftert that you need to download this repo https://github.com/Unity-Technologies/ROS-TCP-Endpoint or you can copy the one which is found under the ros_package_unity_meta_quest
and copy it inside the src folder of the ros workspace. after that you need to copy the unity_meta_quest_msgs int the ros workspace src folder

### win machine
in order to compile the ros packages in windows you need to do:
```
catkin build (or catkin_make if it does not work)
```
 and if everything is installed correctly you have to do

```
./devel/setup.bat
```
to update the ros workspace with the new installed packages


### ubuntu machine
in order to compile the ros packages in windows you need to do:
```
catkin build 
```
 and if everything is installed correctly you have to do

```
source ./devel/setup.bash
```
to update the ros workspace with the new installed packages

 



## setup the unity project with unity.robotics.ros-tcp-connector (unity machine)
this guide has been tested by using unity on a windows 11 operating system
once the ros enviroment is fully set go back to the Unity project contained in the current repository. Open the ros_meta_project in unity and on the top bar click on Robotics->Ros settings. here you have to set the ip of the ros machine and the tcp port (the same used in the launch file) has described in this ![image](http://url/to/1.png)
After that under Robotics->Compile Ros messages you need to set the path the unity_meta_quest_msgs contained inside the unity_meta_quest_ros in this repository and then cliking on the name of the folder ou need to build all the messages one by one. At the end you should see something like ![this](http://url/to/2.png). 
if in the top row the Robotics field is not present you need to install two packages using the same package manager described before. the only difference here is that you will have to install two packages that are contained in the dependencies/unity_packages folder of this repository. Installing local packages should be straightforward (I did encouter few issue and going by memory I resolved that by zipping each package before importing it)

## compile the project
Now that the project is fully setup is time to compile the project.

### on the meta quest (apk)
if perform the installation for android it means that the app will be installed directly on the meta quest device. 
IMMPORTANT!!! to deploy code on the meta quest DO NOT use airlink but proceed only with the meta link with the usb cable connected to the unity machine.
To build click on the top bar inside unity on File->build setting and ensure that an android build setting is selected like in ![figure](http://url/to/3.png). 
click on build and run if you want to directly execute the code inside the meta quest. Otherwise you can only compile the app and run it later

### on windows

if you compile on the unity machine (win) you need to go to  File->build setting click on Windows, Mac, Linux and click on the switch platform button as shown in the ![image](http://url/to/4.png). Once this is done you can simply build the project or build and run the project as before. The difference here is that the app will be located in the unity machine and NOT on the meta quest. 
I did not test this but im pretty sure that the meta quest connected with a usb cable will work. (airlink connection should be tested as well)

# running the code

## on the ros machine
to launch ROS-TCP-Endpoint first yoiu need to update the  endpoint.launch. 
for the <arg name="tcp_ip" default="IP_ADDR"> in place of IP_ADDR you need to put the current ip of the ros computer which is connected to the same subnet to which the meta_quest is connected. 
After that run in a terminal
```
roslaunch ros_tcp_endpoint endpoint.launch
```

## on the meta quest (windows and onboard)
- android app execution: in order to find the app in your meta quest you need to select the app and in the inner dropdown menu select uknown source. from this you can run the app
- windows app execution. with the win app you just have to launch the executable created after the compilation which is located in the folder that has been selected during the compilation process (usually I create a build folder inside the unity project for this purpose). Ensure that the meta quest is connected either with the cable or the airlink to the unity machine.


# testing if everything is working

to test if the meta quest and ros are communicating you can echo some topics by doing

```
rostopic echo /pos_rot_meta_quest
rostopic echo /controller_left_state
rostopic echo /controller_right_state

```


# TODO list
- [x] make installation guide
- [x] testing sending data over ros
- [x] add buttons and create the publisher 
- [ ] make flat video feed inside Ros
- [ ] replace the sphere with the shape of the controller (looks nicer)
- [ ] test sending 2d images from cameras to meta quest (I'm expecting a low frame rate here, -> maybe downsampling and upsampling images could help)
- [ ] test sending 3d images from camera to meta quest for fully immersive operation (I'm expecting a very low frame rate here)
- [ ] test with the Windows app rather than the android app
