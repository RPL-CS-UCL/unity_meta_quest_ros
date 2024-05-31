; Auto-generated. Do not edit!


(cl:in-package unity_meta_quest_msgs-msg)


;//! \htmlinclude ControllerState.msg.html

(cl:defclass <ControllerState> (roslisp-msg-protocol:ros-message)
  ((thumbstick_x
    :reader thumbstick_x
    :initarg :thumbstick_x
    :type cl:float
    :initform 0.0)
   (thumbstick_y
    :reader thumbstick_y
    :initarg :thumbstick_y
    :type cl:float
    :initform 0.0)
   (trigger_pressed
    :reader trigger_pressed
    :initarg :trigger_pressed
    :type cl:boolean
    :initform cl:nil)
   (grip_pressed
    :reader grip_pressed
    :initarg :grip_pressed
    :type cl:boolean
    :initform cl:nil)
   (primary_button_pressed
    :reader primary_button_pressed
    :initarg :primary_button_pressed
    :type cl:boolean
    :initform cl:nil)
   (secondary_button_pressed
    :reader secondary_button_pressed
    :initarg :secondary_button_pressed
    :type cl:boolean
    :initform cl:nil))
)

(cl:defclass ControllerState (<ControllerState>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <ControllerState>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'ControllerState)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name unity_meta_quest_msgs-msg:<ControllerState> is deprecated: use unity_meta_quest_msgs-msg:ControllerState instead.")))

(cl:ensure-generic-function 'thumbstick_x-val :lambda-list '(m))
(cl:defmethod thumbstick_x-val ((m <ControllerState>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader unity_meta_quest_msgs-msg:thumbstick_x-val is deprecated.  Use unity_meta_quest_msgs-msg:thumbstick_x instead.")
  (thumbstick_x m))

(cl:ensure-generic-function 'thumbstick_y-val :lambda-list '(m))
(cl:defmethod thumbstick_y-val ((m <ControllerState>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader unity_meta_quest_msgs-msg:thumbstick_y-val is deprecated.  Use unity_meta_quest_msgs-msg:thumbstick_y instead.")
  (thumbstick_y m))

(cl:ensure-generic-function 'trigger_pressed-val :lambda-list '(m))
(cl:defmethod trigger_pressed-val ((m <ControllerState>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader unity_meta_quest_msgs-msg:trigger_pressed-val is deprecated.  Use unity_meta_quest_msgs-msg:trigger_pressed instead.")
  (trigger_pressed m))

(cl:ensure-generic-function 'grip_pressed-val :lambda-list '(m))
(cl:defmethod grip_pressed-val ((m <ControllerState>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader unity_meta_quest_msgs-msg:grip_pressed-val is deprecated.  Use unity_meta_quest_msgs-msg:grip_pressed instead.")
  (grip_pressed m))

(cl:ensure-generic-function 'primary_button_pressed-val :lambda-list '(m))
(cl:defmethod primary_button_pressed-val ((m <ControllerState>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader unity_meta_quest_msgs-msg:primary_button_pressed-val is deprecated.  Use unity_meta_quest_msgs-msg:primary_button_pressed instead.")
  (primary_button_pressed m))

(cl:ensure-generic-function 'secondary_button_pressed-val :lambda-list '(m))
(cl:defmethod secondary_button_pressed-val ((m <ControllerState>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader unity_meta_quest_msgs-msg:secondary_button_pressed-val is deprecated.  Use unity_meta_quest_msgs-msg:secondary_button_pressed instead.")
  (secondary_button_pressed m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <ControllerState>) ostream)
  "Serializes a message object of type '<ControllerState>"
  (cl:let ((bits (roslisp-utils:encode-single-float-bits (cl:slot-value msg 'thumbstick_x))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) bits) ostream))
  (cl:let ((bits (roslisp-utils:encode-single-float-bits (cl:slot-value msg 'thumbstick_y))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) bits) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) bits) ostream))
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:if (cl:slot-value msg 'trigger_pressed) 1 0)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:if (cl:slot-value msg 'grip_pressed) 1 0)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:if (cl:slot-value msg 'primary_button_pressed) 1 0)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:if (cl:slot-value msg 'secondary_button_pressed) 1 0)) ostream)
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <ControllerState>) istream)
  "Deserializes a message object of type '<ControllerState>"
    (cl:let ((bits 0))
      (cl:setf (cl:ldb (cl:byte 8 0) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) bits) (cl:read-byte istream))
    (cl:setf (cl:slot-value msg 'thumbstick_x) (roslisp-utils:decode-single-float-bits bits)))
    (cl:let ((bits 0))
      (cl:setf (cl:ldb (cl:byte 8 0) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) bits) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) bits) (cl:read-byte istream))
    (cl:setf (cl:slot-value msg 'thumbstick_y) (roslisp-utils:decode-single-float-bits bits)))
    (cl:setf (cl:slot-value msg 'trigger_pressed) (cl:not (cl:zerop (cl:read-byte istream))))
    (cl:setf (cl:slot-value msg 'grip_pressed) (cl:not (cl:zerop (cl:read-byte istream))))
    (cl:setf (cl:slot-value msg 'primary_button_pressed) (cl:not (cl:zerop (cl:read-byte istream))))
    (cl:setf (cl:slot-value msg 'secondary_button_pressed) (cl:not (cl:zerop (cl:read-byte istream))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<ControllerState>)))
  "Returns string type for a message object of type '<ControllerState>"
  "unity_meta_quest_msgs/ControllerState")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'ControllerState)))
  "Returns string type for a message object of type 'ControllerState"
  "unity_meta_quest_msgs/ControllerState")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<ControllerState>)))
  "Returns md5sum for a message object of type '<ControllerState>"
  "13c77d47a3ccdc5ae9cfb2e89658444a")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'ControllerState)))
  "Returns md5sum for a message object of type 'ControllerState"
  "13c77d47a3ccdc5ae9cfb2e89658444a")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<ControllerState>)))
  "Returns full string definition for message of type '<ControllerState>"
  (cl:format cl:nil "# ControllerState.msg~%# This message holds the state of the buttons and thumbstick on the left XR controller.~%~%# Thumbstick values are usually represented as a 2D vector with x and y components.~%float32 thumbstick_x~%float32 thumbstick_y~%~%# Buttons can be represented as booleans, where true means pressed, and false means not pressed.~%bool trigger_pressed~%bool grip_pressed~%bool primary_button_pressed~%bool secondary_button_pressed~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'ControllerState)))
  "Returns full string definition for message of type 'ControllerState"
  (cl:format cl:nil "# ControllerState.msg~%# This message holds the state of the buttons and thumbstick on the left XR controller.~%~%# Thumbstick values are usually represented as a 2D vector with x and y components.~%float32 thumbstick_x~%float32 thumbstick_y~%~%# Buttons can be represented as booleans, where true means pressed, and false means not pressed.~%bool trigger_pressed~%bool grip_pressed~%bool primary_button_pressed~%bool secondary_button_pressed~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <ControllerState>))
  (cl:+ 0
     4
     4
     1
     1
     1
     1
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <ControllerState>))
  "Converts a ROS message object to a list"
  (cl:list 'ControllerState
    (cl:cons ':thumbstick_x (thumbstick_x msg))
    (cl:cons ':thumbstick_y (thumbstick_y msg))
    (cl:cons ':trigger_pressed (trigger_pressed msg))
    (cl:cons ':grip_pressed (grip_pressed msg))
    (cl:cons ':primary_button_pressed (primary_button_pressed msg))
    (cl:cons ':secondary_button_pressed (secondary_button_pressed msg))
))
