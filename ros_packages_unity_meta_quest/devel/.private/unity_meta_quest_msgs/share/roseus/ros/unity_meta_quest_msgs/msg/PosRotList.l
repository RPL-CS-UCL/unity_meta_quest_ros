;; Auto-generated. Do not edit!


(when (boundp 'unity_meta_quest_msgs::PosRotList)
  (if (not (find-package "UNITY_META_QUEST_MSGS"))
    (make-package "UNITY_META_QUEST_MSGS"))
  (shadow 'PosRotList (find-package "UNITY_META_QUEST_MSGS")))
(unless (find-package "UNITY_META_QUEST_MSGS::POSROTLIST")
  (make-package "UNITY_META_QUEST_MSGS::POSROTLIST"))

(in-package "ROS")
;;//! \htmlinclude PosRotList.msg.html


(defclass unity_meta_quest_msgs::PosRotList
  :super ros::object
  :slots (_devices ))

(defmethod unity_meta_quest_msgs::PosRotList
  (:init
   (&key
    ((:devices __devices) ())
    )
   (send-super :init)
   (setq _devices __devices)
   self)
  (:devices
   (&rest __devices)
   (if (keywordp (car __devices))
       (send* _devices __devices)
     (progn
       (if __devices (setq _devices (car __devices)))
       _devices)))
  (:serialization-length
   ()
   (+
    ;; unity_meta_quest_msgs/PosRot[] _devices
    (apply #'+ (send-all _devices :serialization-length)) 4
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; unity_meta_quest_msgs/PosRot[] _devices
     (write-long (length _devices) s)
     (dolist (elem _devices)
       (send elem :serialize s)
       )
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; unity_meta_quest_msgs/PosRot[] _devices
   (let (n)
     (setq n (sys::peek buf ptr- :integer)) (incf ptr- 4)
     (setq _devices (let (r) (dotimes (i n) (push (instance unity_meta_quest_msgs::PosRot :init) r)) r))
     (dolist (elem- _devices)
     (send elem- :deserialize buf ptr-) (incf ptr- (send elem- :serialization-length))
     ))
   ;;
   self)
  )

(setf (get unity_meta_quest_msgs::PosRotList :md5sum-) "439e0451c819cb409f987681338dd04c")
(setf (get unity_meta_quest_msgs::PosRotList :datatype-) "unity_meta_quest_msgs/PosRotList")
(setf (get unity_meta_quest_msgs::PosRotList :definition-)
      "PosRot[] devices
================================================================================
MSG: unity_meta_quest_msgs/PosRot
float32 pos_x
float32 pos_y
float32 pos_z
float32 rot_x
float32 rot_y
float32 rot_z
float32 rot_w
string name
")



(provide :unity_meta_quest_msgs/PosRotList "439e0451c819cb409f987681338dd04c")


