
(cl:in-package :asdf)

(defsystem "unity_meta_quest_msgs-msg"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "ControllerState" :depends-on ("_package_ControllerState"))
    (:file "_package_ControllerState" :depends-on ("_package"))
    (:file "PosRot" :depends-on ("_package_PosRot"))
    (:file "_package_PosRot" :depends-on ("_package"))
    (:file "PosRotList" :depends-on ("_package_PosRotList"))
    (:file "_package_PosRotList" :depends-on ("_package"))
  ))