;; Auto-generated. Do not edit!


(when (boundp 'unity_meta_quest_msgs::PosRot)
  (if (not (find-package "UNITY_META_QUEST_MSGS"))
    (make-package "UNITY_META_QUEST_MSGS"))
  (shadow 'PosRot (find-package "UNITY_META_QUEST_MSGS")))
(unless (find-package "UNITY_META_QUEST_MSGS::POSROT")
  (make-package "UNITY_META_QUEST_MSGS::POSROT"))

(in-package "ROS")
;;//! \htmlinclude PosRot.msg.html


(defclass unity_meta_quest_msgs::PosRot
  :super ros::object
  :slots (_pos_x _pos_y _pos_z _rot_x _rot_y _rot_z _rot_w _name ))

(defmethod unity_meta_quest_msgs::PosRot
  (:init
   (&key
    ((:pos_x __pos_x) 0.0)
    ((:pos_y __pos_y) 0.0)
    ((:pos_z __pos_z) 0.0)
    ((:rot_x __rot_x) 0.0)
    ((:rot_y __rot_y) 0.0)
    ((:rot_z __rot_z) 0.0)
    ((:rot_w __rot_w) 0.0)
    ((:name __name) "")
    )
   (send-super :init)
   (setq _pos_x (float __pos_x))
   (setq _pos_y (float __pos_y))
   (setq _pos_z (float __pos_z))
   (setq _rot_x (float __rot_x))
   (setq _rot_y (float __rot_y))
   (setq _rot_z (float __rot_z))
   (setq _rot_w (float __rot_w))
   (setq _name (string __name))
   self)
  (:pos_x
   (&optional __pos_x)
   (if __pos_x (setq _pos_x __pos_x)) _pos_x)
  (:pos_y
   (&optional __pos_y)
   (if __pos_y (setq _pos_y __pos_y)) _pos_y)
  (:pos_z
   (&optional __pos_z)
   (if __pos_z (setq _pos_z __pos_z)) _pos_z)
  (:rot_x
   (&optional __rot_x)
   (if __rot_x (setq _rot_x __rot_x)) _rot_x)
  (:rot_y
   (&optional __rot_y)
   (if __rot_y (setq _rot_y __rot_y)) _rot_y)
  (:rot_z
   (&optional __rot_z)
   (if __rot_z (setq _rot_z __rot_z)) _rot_z)
  (:rot_w
   (&optional __rot_w)
   (if __rot_w (setq _rot_w __rot_w)) _rot_w)
  (:name
   (&optional __name)
   (if __name (setq _name __name)) _name)
  (:serialization-length
   ()
   (+
    ;; float32 _pos_x
    4
    ;; float32 _pos_y
    4
    ;; float32 _pos_z
    4
    ;; float32 _rot_x
    4
    ;; float32 _rot_y
    4
    ;; float32 _rot_z
    4
    ;; float32 _rot_w
    4
    ;; string _name
    4 (length _name)
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; float32 _pos_x
       (sys::poke _pos_x (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; float32 _pos_y
       (sys::poke _pos_y (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; float32 _pos_z
       (sys::poke _pos_z (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; float32 _rot_x
       (sys::poke _rot_x (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; float32 _rot_y
       (sys::poke _rot_y (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; float32 _rot_z
       (sys::poke _rot_z (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; float32 _rot_w
       (sys::poke _rot_w (send s :buffer) (send s :count) :float) (incf (stream-count s) 4)
     ;; string _name
       (write-long (length _name) s) (princ _name s)
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; float32 _pos_x
     (setq _pos_x (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; float32 _pos_y
     (setq _pos_y (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; float32 _pos_z
     (setq _pos_z (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; float32 _rot_x
     (setq _rot_x (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; float32 _rot_y
     (setq _rot_y (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; float32 _rot_z
     (setq _rot_z (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; float32 _rot_w
     (setq _rot_w (sys::peek buf ptr- :float)) (incf ptr- 4)
   ;; string _name
     (let (n) (setq n (sys::peek buf ptr- :integer)) (incf ptr- 4) (setq _name (subseq buf ptr- (+ ptr- n))) (incf ptr- n))
   ;;
   self)
  )

(setf (get unity_meta_quest_msgs::PosRot :md5sum-) "c0e396f41a876de177e09f3c627fd741")
(setf (get unity_meta_quest_msgs::PosRot :datatype-) "unity_meta_quest_msgs/PosRot")
(setf (get unity_meta_quest_msgs::PosRot :definition-)
      "float32 pos_x
float32 pos_y
float32 pos_z
float32 rot_x
float32 rot_y
float32 rot_z
float32 rot_w
string name
")



(provide :unity_meta_quest_msgs/PosRot "c0e396f41a876de177e09f3c627fd741")


