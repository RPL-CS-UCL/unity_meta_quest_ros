;; Auto-generated. Do not edit!


(when (boundp 'unity_meta_quest_msgs::PositionService)
  (if (not (find-package "UNITY_META_QUEST_MSGS"))
    (make-package "UNITY_META_QUEST_MSGS"))
  (shadow 'PositionService (find-package "UNITY_META_QUEST_MSGS")))
(unless (find-package "UNITY_META_QUEST_MSGS::POSITIONSERVICE")
  (make-package "UNITY_META_QUEST_MSGS::POSITIONSERVICE"))
(unless (find-package "UNITY_META_QUEST_MSGS::POSITIONSERVICEREQUEST")
  (make-package "UNITY_META_QUEST_MSGS::POSITIONSERVICEREQUEST"))
(unless (find-package "UNITY_META_QUEST_MSGS::POSITIONSERVICERESPONSE")
  (make-package "UNITY_META_QUEST_MSGS::POSITIONSERVICERESPONSE"))

(in-package "ROS")





(defclass unity_meta_quest_msgs::PositionServiceRequest
  :super ros::object
  :slots (_input ))

(defmethod unity_meta_quest_msgs::PositionServiceRequest
  (:init
   (&key
    ((:input __input) (instance unity_meta_quest_msgs::PosRot :init))
    )
   (send-super :init)
   (setq _input __input)
   self)
  (:input
   (&rest __input)
   (if (keywordp (car __input))
       (send* _input __input)
     (progn
       (if __input (setq _input (car __input)))
       _input)))
  (:serialization-length
   ()
   (+
    ;; unity_meta_quest_msgs/PosRot _input
    (send _input :serialization-length)
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; unity_meta_quest_msgs/PosRot _input
       (send _input :serialize s)
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; unity_meta_quest_msgs/PosRot _input
     (send _input :deserialize buf ptr-) (incf ptr- (send _input :serialization-length))
   ;;
   self)
  )

(defclass unity_meta_quest_msgs::PositionServiceResponse
  :super ros::object
  :slots (_output ))

(defmethod unity_meta_quest_msgs::PositionServiceResponse
  (:init
   (&key
    ((:output __output) (instance unity_meta_quest_msgs::PosRot :init))
    )
   (send-super :init)
   (setq _output __output)
   self)
  (:output
   (&rest __output)
   (if (keywordp (car __output))
       (send* _output __output)
     (progn
       (if __output (setq _output (car __output)))
       _output)))
  (:serialization-length
   ()
   (+
    ;; unity_meta_quest_msgs/PosRot _output
    (send _output :serialization-length)
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; unity_meta_quest_msgs/PosRot _output
       (send _output :serialize s)
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; unity_meta_quest_msgs/PosRot _output
     (send _output :deserialize buf ptr-) (incf ptr- (send _output :serialization-length))
   ;;
   self)
  )

(defclass unity_meta_quest_msgs::PositionService
  :super ros::object
  :slots ())

(setf (get unity_meta_quest_msgs::PositionService :md5sum-) "74d162dfa312ad5f1087485ee56d06ee")
(setf (get unity_meta_quest_msgs::PositionService :datatype-) "unity_meta_quest_msgs/PositionService")
(setf (get unity_meta_quest_msgs::PositionService :request) unity_meta_quest_msgs::PositionServiceRequest)
(setf (get unity_meta_quest_msgs::PositionService :response) unity_meta_quest_msgs::PositionServiceResponse)

(defmethod unity_meta_quest_msgs::PositionServiceRequest
  (:response () (instance unity_meta_quest_msgs::PositionServiceResponse :init)))

(setf (get unity_meta_quest_msgs::PositionServiceRequest :md5sum-) "74d162dfa312ad5f1087485ee56d06ee")
(setf (get unity_meta_quest_msgs::PositionServiceRequest :datatype-) "unity_meta_quest_msgs/PositionServiceRequest")
(setf (get unity_meta_quest_msgs::PositionServiceRequest :definition-)
      "PosRot input

================================================================================
MSG: unity_meta_quest_msgs/PosRot
float32 pos_x
float32 pos_y
float32 pos_z
float32 rot_x
float32 rot_y
float32 rot_z
float32 rot_w
---
PosRot output

================================================================================
MSG: unity_meta_quest_msgs/PosRot
float32 pos_x
float32 pos_y
float32 pos_z
float32 rot_x
float32 rot_y
float32 rot_z
float32 rot_w
")

(setf (get unity_meta_quest_msgs::PositionServiceResponse :md5sum-) "74d162dfa312ad5f1087485ee56d06ee")
(setf (get unity_meta_quest_msgs::PositionServiceResponse :datatype-) "unity_meta_quest_msgs/PositionServiceResponse")
(setf (get unity_meta_quest_msgs::PositionServiceResponse :definition-)
      "PosRot input

================================================================================
MSG: unity_meta_quest_msgs/PosRot
float32 pos_x
float32 pos_y
float32 pos_z
float32 rot_x
float32 rot_y
float32 rot_z
float32 rot_w
---
PosRot output

================================================================================
MSG: unity_meta_quest_msgs/PosRot
float32 pos_x
float32 pos_y
float32 pos_z
float32 rot_x
float32 rot_y
float32 rot_z
float32 rot_w
")



(provide :unity_meta_quest_msgs/PositionService "74d162dfa312ad5f1087485ee56d06ee")


