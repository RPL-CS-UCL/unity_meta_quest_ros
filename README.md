# VR Teleoperation Framework for Locomanipulation Tasks
This is a user guide for all components in this project. Later versions will be simpler, more portable and possibly work on other platform/arm combinations. Right now this only uses images and uses COLMAP with gaussian splatting. This will be updated in the future when pointcloud mode is added.

There are 4 components to this system. Unity, arm routine, platform control and teleoperation. Unity is run on a PC with Windows and a GPU while the arm routine and teleoperation are run on the onboard Nuc. Platform control is handled across both.

## Repo Setup
 - Use `git clone --recurse-submodules` when cloning on both the Nuc and the Unity machine to make sure you download all the required submodules
 - Install Mamba (miniforge) from https://conda-forge.org/download/ on both machines

### Unity Machine
 - Open the Unity project (runs on 2023.1.19f1, builds for Android and Windows)
 - Open `Window -> Package Manager` 
 - Install the SplatVFX unity package from disk by selecting jp.keijiro.splat-vfx/package.json
 - Get all relevant gaussian splatting dependencies (COLMAP, ImageMagik, FFMPEG)
 - Modify Assets/gaussian_splatting_config.txt so first line is the path to the gaussian splatting folder and the second line is the path to the miniforge folder
 - Build environment.yml in the gaussian splatting submodule
 - Build transform.yml in unity_meta_quest_ros/gaussian_splatting_setup
 - Open `Robotics -> Generate ROS Messages...`
 - In ROS Message Browser in ROS message path make sure the path to unity_meta_quest_msgs/msg is correct
 - Build the messages

### Nuc
 - Build ros_env.yml

## Physical Setup
The Unity machine is entirely on it's own here. The Nuc is slightly more complicated. It needs a USB 3.0 to USB-C connection to the Intel RealSense camera on the end of the arm. It also needs 2 ethernet connections, one to the arm and one to the base. The Nuc only has one ethernet port so use an adapter here. To turn on the Franka, plug it into a wall socket and hit the power switch hidden in the corner.

For the Summit-XL, hit the power switch and the CPU button. The restart button will automatically be turned on. Turn off the emergency power button (yellow phone-sized object clipped onto the base) and then hit the reset button to turn it off. The router (big yellow thing with an antenna) should turn on and its light will be flashing. Be aware that the Summit-XL will take a few minutes to start up, you'll see and hear it calibrating/adjusting it's cameras.

## Network Setup
Once the Franka is on, go to `192.168.2.100` in your browser and log in (username and password are both `eastrobotics`). Then unlock the joints, activate the FCI and make sure the black safety button is off (the lights on the Franka should be blue).

For the Nuc, you need to first make sure that it has the correct ethernet connections to the base (LAN port) and arm. The IP (ipv4) for the arm connection can be any address starting with `192.168.2` that isn't already taken (since the arm itself has address `192.168.2.100`), so right now it uses one called `Profile 3` with address `192.168.2.76`. For the base, connect to the ethernet profile called `summit-xl`. This has address `192.168.0.170` and gateway `255.255.255.0`. In practice, this address can be whatever you want as long as it matches the ROS IP Address field in the ROS settings for the Unity project (Robotics -> ROS Settings -> ROS IP Address) and the TCP IP field of the TCP endpoint launch file ("tcp_ip" in `ros_ws/meta_summit_xl_ws/src/ROS-TCP-ENDPOINT/launch/endpoint.launch`).

Finally, turn on the wifi hotspot. This will make it an extender of the Summit-XL router. Make sure that the Unity machine is connected to this hotspot (otherwise it won't reach the ROS master).

Bear in mind that the ROS master is on the base while the TCP endpoint is on the Nuc. **These are not the same thing**. The PC connects to the Nuc through the TCP endpoint while the Nuc connects to the base/ROS master via ethernet.

### ROS ###
#### TCP Endpoint ####
The address in the ROS settings of Unity must match the address of the endpoint launch file on the Nuc. This is because they are both the address of the Nuc in whatever wifi network you are running the PC and Nuc on. There are 3 options:
- Nuc wifi extender: Fastest option, weird IP address `10.42.0.1`
- Summit-xl router: Address starting with `192.168.0` (in this case `192.168.0.170`)
- Netgear router: Address starting with `192.168.1` (in this case `192.168.1.37`)
#### ROS Master ####
The ROS master is on the Summit-XL and communicates over ethernet with the Nuc. Every time you run a terminal on the Nuc you need to run `export ROS_MASTER_URI=http://192.168.0.200:11311/` so the terminal knows that the master is at this IP (the Summit). You can use localhost instead for testing to set the Nuc as the ROS master but this means that you can't control the Summit. You also need to run `export ROS_IP=192.168.0.170` in every Nuc terminal since this makes the terminal visible on the ROS network (hence it is the IP of the Nuc on the Summit network).

## Running the scripts
The scripts are in various sub-packages in `ros_ws/meta_summit_xl_ws`. The order to run the scripts from first to last is: 

ROS TCP endpoint -> Camera launch -> Franka routine/Base camera subscriber/Base movement/Teleop -> Unity

You need to run the scripts by ssh from the Unity PC after connecting to the Nuc wifi hotspot. Use `ssh takuya@10.42.0.1`.

Note that for all terminal windows you need to activate Mamba. You do this by running the command in `mamba_startup.txt` (file in desktop). 

`eval "$(/home/takuya/miniforge3/bin/conda shell.bash hook)"`

From here you can enter the necessary mamba environment with `conda activate <environment>`.

### Unity ###
Unity runs all it's scripts (`unity_meta_quest/ros_meta_quest`) whenever you run it in play mode or build and run. However, right now all the scripts on the Nuc need to be run manually (this will be automated later).

Note that `Franka_Subscriber` uses intermediary files and scripts (`convert_ply_splat.py` and `transform_ply.py`) in `gaussian_splatting_setup` so there is no need to modify `gaussian-splatting` for this project to work.

#### Relevant Scripts ####
- `Assets/Scripts/Quest_Franka_Trigger` handles triggering the image collection and teleoperation
- `Assets/Scripts/Franka_Subscriber` handles receiving the images, running gaussian splatting and translating the splat to Unity

Panda controller (attached to base panda object) controls all the joints (articulation bodies in an articulation chain). Quest_Franka_Trigger instantiates the controller to set the recieved joint angles. Panda Gripper Articulation (script attached to panda_hand) takes in input and instantiates the controller to open/close the gripper. It also handles the physics of holding the objects in the gripper.

### ROS TCP Endpoint
For the ROS TCP endpoint start the `summit-xl` mamba environment in a new terminal window. Then go to `ros_ws/meta_summit_xl_ws` and run:

```
catkin clean
catkin build
source ./devel/setup.bash
export ROS_MASTER_URI=http://192.168.0.200:11311/
export ROS_IP=192.168.0.170
roslaunch ros_tcp_endpoint endpoint.launch
```

### Camera ###
To start the camera make a new terminal windows in the `Summit-XL` Mamba environment, go to `ros_ws/meta_summit_xl_ws` and run:

```
source ./devel/setup.bash
export ROS_MASTER_URI=http://192.168.0.200:11311/
export ROS_IP=192.168.0.170
roslaunch realsense2_camera rs_camera.launch
```

### Arm ###
To run the Franka script make a new terminal window in the `Summit-XL` Mamba environment, go to `ros_ws/meta_summit_xl_ws` and run:

```
source ./devel/setup.bash
export ROS_MASTER_URI=http://192.168.0.200:11311/
export ROS_IP=192.168.0.170
cd src/unity_meta_quest_sender/scripts
python image_data_collection.py
```

### Base Camera ###
To run the base camera subscriber script make a new terminal window in the `Summit-XL` Mamba environment, go to `ros_ws/meta_summit_xl_ws` and run:

```
source ./devel/setup.bash
export ROS_MASTER_URI=http://192.168.0.200:11311/
export ROS_IP=192.168.0.170
cd src/unity_meta_quest_sender/scripts
python image_publisher.py
```

### Base Movement ###
To run the base movement script make a new terminal window in the `Summit-XL` Mamba environment, go to `ros_ws/meta_summit_xl_ws` and run:

```
source ./devel/setup.bash
export ROS_MASTER_URI=http://192.168.0.200:11311/
export ROS_IP=192.168.0.170
src/unity_meta_quest_sender/scripts
python move_base.py
```

### Teleoperation
There are two scripts that need to be run for this to work: the controller and the intermediary control node.

To run the intermediary control node make a new terminal window in the `Summit-XL` Mamba environment, go to `ros_ws/meta_summit_xl_ws` and run:

```
source ./devel/setup.bash
export ROS_MASTER_URI=http://192.168.0.200:11311/
export ROS_IP=192.168.0.170
rosrun unity_meta_quest_sender franka_teleop.py
```

To run the controller make a new terminal window in the `Summit-XL` Mamba environment, go to `ros_ws/meta_summit_xl_ws` and run:

```
source ./devel/setup.bash
export ROS_MASTER_URI=http://192.168.0.200:11311/
export ROS_IP=192.168.0.170
rosrun wb_quad_arm franka_teleop_inverse_kinematic_metaquest.py
```

(may be called wb_z1_spot)

Note that for the scripts instead of running `python <script>` you should go to `ros_ws/meta_summit_xl_ws` and run `rosrun <package> <script>` instead, where the package is `unity_meta_quest_sender` for the base, arm and teleoperation scripts.

## Usage
Run the scripts in the correct order. If you intend to run everything, make sure you build and run the Unity project. **Do not run Unity in play mode** - the gaussian splatting screws up the rendering and it doesn't work. Only use play mode for testing/debugging purposes.

Connect the Meta Quest 2 to the Unity PC via Quest Link. Use a physical cable - **do not use airlink, it's shit**. You should see two spheres where your hands/controllers are and a canvas in front of you with the front facing camera feed from the robot. Use the right joystick to move the Summit-XL remotely. 

When you're ready, pull the right trigger to start the gaussian splatter generation process. You won't see anything new in the Unity environment immediately, but the arm will start it's routine taking and sending pictures to the PC. Once all the pictures are sent, COLMAP will run structure-from-motion on said images, and then the gaussian splatter will start training. Again, this is all in the background so you won't see anything until the final output. Once this is complete, the output `.ply` file will be converted to a `.splat` file and the result will be imported as a visual effect into the VR scene. This whole process currently takes under 10 minutes.

To move the arm in VR, hold your right controller at the end-effector and press A (don't hold it). Move your controller slowly (the arm will follow it) and then press A again to release it.

To open/close the gripper on the end-effector press B (single press, not hold).

To switch to joystick control press the Y button on the left controller. To switch to end-effector dragging control press the X button on the left controller. Left joystick controls X and Y, right joystick controls up and down. Note that at the start it is automatically in dragging mode. When testing the baseline mode in user study you can't use the dragging mode so remember to press Y at the start.

For the user study, press esc at the start to switch to 'baseline' mode.

## Troubleshooting
When troubleshooting, check the hardware setup first (easy fix), then the scripts (regular debugging) and finally ROS (harder).
  ### Not reading from ROS node ### 
  Run `rostopic list` to check that it's getting the node. If the robot nodes don't show up you probably forgot to run `export ROS_MASTER_URI=http://192.168.0.200:11311/` and `export ROS_IP=192.168.0.170`. If you run this after you start the camera it should also show a bunch of RealSense nodes.
  ### Messages don't build ###
  You forgot to run `source ./devel/setup.bash`.
  ### Package/file not found when executing Rosrun
  Try going back through the directories a few times (`cd ..`), you're probably inside the package you're looking for.
  ### Splat not rendered
  Make sure the splat vfx graph (in assets and in the splat vfx package) is EXACTLY as it is here:

  ![splat VFX graph](https://github.com/RPL-CS-UCL/unity_meta_quest_ros/blob/main/images/splatvfx_graph.png?raw=true)
  ### Teleoperation calibration not working
  In Unity editor click on XR  Origin (XR Rig) and look at the Quest_Franka_Trigger component. Make sure it looks as follows:

  ![QFT settings](https://github.com/RPL-CS-UCL/unity_meta_quest_ros/blob/main/images/QFT_settings.png?raw=true)
  ### Misc ###
  If in doubt just try switching out the ethernet connections - they're super dodgy.
  
  Esc key to switch to 'baseline' control mode for user study.

  To generate splat of VR objects for user study:

   - Disable camera component of main (XR Origin) camera 
   - Enable Splat Camera
   - Disable XR Interactor Line Visual component of both controllers
   - Disable Mesh Renderer component of both controller spheres 

  When stopping Unity executable press Alt + F4.

  gaussian-splatting installation/environment issues: https://github.com/graphdeco-inria/gaussian-splatting/issues/332

  With public variables in Unity scripts, adjust them through the editor, this takes priority over the value written in the script.
