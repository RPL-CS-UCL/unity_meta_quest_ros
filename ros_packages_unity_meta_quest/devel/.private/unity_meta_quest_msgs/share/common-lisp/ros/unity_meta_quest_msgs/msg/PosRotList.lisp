; Auto-generated. Do not edit!


(cl:in-package unity_meta_quest_msgs-msg)


;//! \htmlinclude PosRotList.msg.html

(cl:defclass <PosRotList> (roslisp-msg-protocol:ros-message)
  ((devices
    :reader devices
    :initarg :devices
    :type (cl:vector unity_meta_quest_msgs-msg:PosRot)
   :initform (cl:make-array 0 :element-type 'unity_meta_quest_msgs-msg:PosRot :initial-element (cl:make-instance 'unity_meta_quest_msgs-msg:PosRot))))
)

(cl:defclass PosRotList (<PosRotList>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <PosRotList>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'PosRotList)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name unity_meta_quest_msgs-msg:<PosRotList> is deprecated: use unity_meta_quest_msgs-msg:PosRotList instead.")))

(cl:ensure-generic-function 'devices-val :lambda-list '(m))
(cl:defmethod devices-val ((m <PosRotList>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader unity_meta_quest_msgs-msg:devices-val is deprecated.  Use unity_meta_quest_msgs-msg:devices instead.")
  (devices m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <PosRotList>) ostream)
  "Serializes a message object of type '<PosRotList>"
  (cl:let ((__ros_arr_len (cl:length (cl:slot-value msg 'devices))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_arr_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_arr_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_arr_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_arr_len) ostream))
  (cl:map cl:nil #'(cl:lambda (ele) (roslisp-msg-protocol:serialize ele ostream))
   (cl:slot-value msg 'devices))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <PosRotList>) istream)
  "Deserializes a message object of type '<PosRotList>"
  (cl:let ((__ros_arr_len 0))
    (cl:setf (cl:ldb (cl:byte 8 0) __ros_arr_len) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 8) __ros_arr_len) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 16) __ros_arr_len) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 24) __ros_arr_len) (cl:read-byte istream))
  (cl:setf (cl:slot-value msg 'devices) (cl:make-array __ros_arr_len))
  (cl:let ((vals (cl:slot-value msg 'devices)))
    (cl:dotimes (i __ros_arr_len)
    (cl:setf (cl:aref vals i) (cl:make-instance 'unity_meta_quest_msgs-msg:PosRot))
  (roslisp-msg-protocol:deserialize (cl:aref vals i) istream))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<PosRotList>)))
  "Returns string type for a message object of type '<PosRotList>"
  "unity_meta_quest_msgs/PosRotList")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'PosRotList)))
  "Returns string type for a message object of type 'PosRotList"
  "unity_meta_quest_msgs/PosRotList")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<PosRotList>)))
  "Returns md5sum for a message object of type '<PosRotList>"
  "439e0451c819cb409f987681338dd04c")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'PosRotList)))
  "Returns md5sum for a message object of type 'PosRotList"
  "439e0451c819cb409f987681338dd04c")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<PosRotList>)))
  "Returns full string definition for message of type '<PosRotList>"
  (cl:format cl:nil "PosRot[] devices~%================================================================================~%MSG: unity_meta_quest_msgs/PosRot~%float32 pos_x~%float32 pos_y~%float32 pos_z~%float32 rot_x~%float32 rot_y~%float32 rot_z~%float32 rot_w~%string name~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'PosRotList)))
  "Returns full string definition for message of type 'PosRotList"
  (cl:format cl:nil "PosRot[] devices~%================================================================================~%MSG: unity_meta_quest_msgs/PosRot~%float32 pos_x~%float32 pos_y~%float32 pos_z~%float32 rot_x~%float32 rot_y~%float32 rot_z~%float32 rot_w~%string name~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <PosRotList>))
  (cl:+ 0
     4 (cl:reduce #'cl:+ (cl:slot-value msg 'devices) :key #'(cl:lambda (ele) (cl:declare (cl:ignorable ele)) (cl:+ (roslisp-msg-protocol:serialization-length ele))))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <PosRotList>))
  "Converts a ROS message object to a list"
  (cl:list 'PosRotList
    (cl:cons ':devices (devices msg))
))
