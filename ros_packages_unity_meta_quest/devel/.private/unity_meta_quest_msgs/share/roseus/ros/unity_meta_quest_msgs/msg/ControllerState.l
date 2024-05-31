;; Auto-generated. Do not edit!


(when (boundp 'unity_meta_quest_msgs::ControllerState)
  (if (not (find-package "UNITY_META_QUEST_MSGS"))
    (make-package "UNITY_META_QUEST_MSGS"))
  (shadow 'ControllerState (find-package "UNITY_META_QUEST_MSGS")))
(unless (find-package "UNITY_META_QUEST_MSGS::CONTROLLERSTATE")
  (make-package "UNITY_META_QUEST_MSGS::CONTROLLERSTATE"))

(in-package "ROS")
;;//! \htmlinclude ControllerState.msg.html


(defclass unity_meta_quest_msgs::ControllerState
  :super ros::object
  :slots (_thumbstick_x _thumbstick_y _trigger_pressed _grip_pressed _primary_button_pressed _secondary_button_pressed ))

(defmethod unity_meta_quest_msgs::ControllerState
  (:init
   (&key
    ((:thumbstick_x __thumbstick_x) 0.0)
    ((:thumbstick_y __thumbstick_y) 0.0)
    ((:trigger_pressed __trigger_pressed) nil)
    ((:grip_pressed __grip_pressed) nil)
    ((:primary_button_pressed __primary_button_pressed) nil)
    ((:secondary_button_pressed __secondary_button_pressed) nil)
    )
   (send-super :init)
   (setq _thumbstick_x (float __thumbstick_x))
   (setq _thumbstick_y (float __thumbstick_y))
   (setq _trigger_pressed __trigger_pressed)
   (setq _grip_pressed __grip_pressed)
   (setq _primary_button_pressed __primary_button_pressed)
   (setq _secondary_button_pressed __secondary_button_pressed)
   self)
  (:thumbstick_x
   (&optional __thumbstick_x)
   (if __thumbstick_x (setq _thumbstick_x __thumbstick_x)) _thumbstick_x)
  (:thumbstick_y
   (&optional __thumbstick_y)
   (if __thumbstick_y (setq _thumbstick_y __thumbstick_y)) _thumbstick_y)
  (:trigger_pressed
   (&optional (__trigger_pressed :null))
   (if (not (eq __trigger_pressed :null)) (setq _trigger_pressed __trigger_pressed)) _trigger_pressed)
  (:grip_pressed
   (&optional (__grip_pressed :null))
   (if (not (eq __grip_pressed :null)) (setq _grip_pressed __grip_pressed)) _grip_pressed)
  (:primary_button_pressed
   (&optional (__primary_button_pressed :null))
   (if (not (eq __primary_button_pressed :null)) (setq _primary_button_pressed __primary_button_pressed)) _primary_button_pressed)
  (:secondary_button_pressed
   (&optional (__secondary_button_pressed :null))
   (if (not (eq __secondary_button_pressed :null)) (setq _secondary_button_pressed __secondary_button_pressed)) _secondary_button_pressed)
  (:serialization-length
   ()
   (+
    ;; float32 _thumbstick_x
    4
    ;; float32 _thumbstick_y
    4
    ;; bool _trigger_pressed
    1
    ;; bool _grip_pressed
    1
    ;; bool _primary_button_pressed
    1
    ;; bool _secondary_button_pressed
    1
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; float32 _thumbstick_x
       (sys::poke _thumbstick_x (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; float32 _thumbstick_y
       (sys::poke _thumbstick_y (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; bool _trigger_pressed
       (if _trigger_pressed (write-byte -1 s) (write-byte 0 s))
     ;; bool _grip_pressed
       (if _grip_pressed (write-byte -1 s) (write-byte 0 s))
     ;; bool _primary_button_pressed
       (if _primary_button_pressed (write-byte -1 s) (write-byte 0 s))
     ;; bool _secondary_button_pressed
       (if _secondary_button_pressed (write-byte -1 s) (write-byte 0 s))
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; float32 _thumbstick_x
     (setq _thumbstick_x (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; float32 _thumbstick_y
     (setq _thumbstick_y (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; bool _trigger_pressed
     (setq _trigger_pressed (not (= 0 (sys::peek buf ptr- :char)))) (incf ptr- 1)
   ;; bool _grip_pressed
     (setq _grip_pressed (not (= 0 (sys::peek buf ptr- :char)))) (incf ptr- 1)
   ;; bool _primary_button_pressed
     (setq _primary_button_pressed (not (= 0 (sys::peek buf ptr- :char)))) (incf ptr- 1)
   ;; bool _secondary_button_pressed
     (setq _secondary_button_pressed (not (= 0 (sys::peek buf ptr- :char)))) (incf ptr- 1)
   ;;
   self)
  )

(setf (get unity_meta_quest_msgs::ControllerState :md5sum-) "13c77d47a3ccdc5ae9cfb2e89658444a")
(setf (get unity_meta_quest_msgs::ControllerState :datatype-) "unity_meta_quest_msgs/ControllerState")
(setf (get unity_meta_quest_msgs::ControllerState :definition-)
      "# ControllerState.msg
# This message holds the state of the buttons and thumbstick on the left XR controller.

# Thumbstick values are usually represented as a 2D vector with x and y components.
float32 thumbstick_x
float32 thumbstick_y

# Buttons can be represented as booleans, where true means pressed, and false means not pressed.
bool trigger_pressed
bool grip_pressed
bool primary_button_pressed
bool secondary_button_pressed
")



(provide :unity_meta_quest_msgs/ControllerState "13c77d47a3ccdc5ae9cfb2e89658444a")


