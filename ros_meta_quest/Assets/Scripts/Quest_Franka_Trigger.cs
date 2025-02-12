using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityMetaQuest;
using BoolMessage = RosMessageTypes.Std.BoolMsg;
using ArrayMessage = RosMessageTypes.Std.Float64MultiArrayMsg;
using System;
using static UnityEngine.Application;
using UnityEngine.UI;
using Unity.Robotics;
using Unity.Robotics.UrdfImporter.Control;

public class Quest_Franka_Trigger : MonoBehaviour
{
    ROSConnection ros;
   
    public string topicName = "franca_start";
// public string jointsTopicname = "new_pos";
    public string cartTopicName = "sphere_pos";
    
    public InputActionReference rightTriggerActionReference; // Reference to your right trigger InputAction
    public InputActionReference grabActionReference; // Reference to your grab InputAction

    private bool hasRun = false;

    public GameObject handSphere;
    public GameObject targetSphere;
    public bool attached = false;

    // Reference to the root GameObject of the robot arm (imported URDF).
    public GameObject robotArm;
    public GameObject endEffector;
    public Controller controller;

    public bool Calibrated;

    // Start is called before the first frame update
    void Start()
    {
        Calibrated = false;
        // Start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<BoolMessage>(topicName);
        ros.Subscribe<ArrayMessage>("new_pos", JointsCallback);
        //movement = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ArrayMessage>(cartTopicName);

        SetVisibility(true); //false
    }

    // Method to toggle the visibility
    public void SetVisibility(bool isVisible)
    {
        robotArm.SetActive(isVisible);
    }


    void OnEnable()
    {
        // Enable the input action
        if (rightTriggerActionReference != null && rightTriggerActionReference.action != null)
        {
            rightTriggerActionReference.action.performed += OnRightTriggerPressed;
            rightTriggerActionReference.action.Enable();
        }

        // Enable the input action for grabbing
        if (grabActionReference != null && grabActionReference.action != null)
        {
            grabActionReference.action.performed += OnGrabActionPerformed;
            grabActionReference.action.Enable();
        }
    }

    void OnDisable()
    {
        // Disable the input action
        if (rightTriggerActionReference != null && rightTriggerActionReference.action != null)
        {
            rightTriggerActionReference.action.performed -= OnRightTriggerPressed;
            rightTriggerActionReference.action.Disable();
        }

        // Disable the input action for grabbing
        if (grabActionReference != null && grabActionReference.action != null)
        {
            grabActionReference.action.performed -= OnGrabActionPerformed;
            grabActionReference.action.Disable();
        }
    }

    
    void JointsCallback(ArrayMessage msgIn)
    {

        //UnityEngine.Debug.Log(controller.m_jointAngles.Length);
        //return;
        //if (!robotArm.active) return;

        if (controller.m_jointAngles.Length == 0) return;
        UnityEngine.Debug.Log("active");
        UnityEngine.Debug.Log(controller.enabled);
        controller.m_jointAngles[0] = 0;
        controller.m_jointAngles[1] = (float)msgIn.data[0] * Mathf.Rad2Deg;
        controller.m_jointAngles[2] = (float)msgIn.data[1] * Mathf.Rad2Deg;
        controller.m_jointAngles[3] = (float)msgIn.data[2] * Mathf.Rad2Deg;
        controller.m_jointAngles[4] = (float)msgIn.data[3] * Mathf.Rad2Deg;
        controller.m_jointAngles[5] = (float)msgIn.data[4] * Mathf.Rad2Deg;
        controller.m_jointAngles[6] = (float)msgIn.data[5] * Mathf.Rad2Deg;
        controller.m_jointAngles[7] = (float)msgIn.data[6] * Mathf.Rad2Deg;
        controller.m_jointAngles[8] = (float)msgIn.data[7] * Mathf.Rad2Deg;
        controller.m_jointAngles[9] = 0;
        controller.m_jointAngles[10] = 0;
        controller.m_jointAngles[11] = 0;
        controller.m_jointAngles[12] = 0;
        //UnityEngine.Debug.Log(targetSphere.transform.position);
        //UnityEngine.Debug.Log(endEffector.transform.position);
        if (Calibrated == true) // && !attached  
        {
            UnityEngine.Debug.Log(targetSphere.transform.position);
            UnityEngine.Debug.Log(endEffector.transform.position);
            //UnityEngine.Debug.Log("calibrated");
            ArrayMessage arrayMessage = new ArrayMessage();
            arrayMessage.data = new double[3];
            // Populate the array with target sphere position (axes swapped for Franka frame of reference)
            arrayMessage.data[0] = targetSphere.transform.position.z;
            arrayMessage.data[1] = -targetSphere.transform.position.x;
            arrayMessage.data[2] = targetSphere.transform.position.y - 0.55;
            UnityEngine.Debug.Log(arrayMessage);
            ros.Publish(cartTopicName, arrayMessage);
        }
        else
        {
            targetSphere.transform.position = endEffector.transform.position;
        }
            /*
            if (!attached)
            {
                //UnityEngine.Debug.Log("not attached");
                targetSphere.transform.position = endEffector.transform.position;
            }
            */
        }

    private void Update()
    {
        //UnityEngine.Debug.Log(targetSphere.transform.position);
        
        
        
    }


    void GrabSphere()
    {

        //UnityEngine.Debug.Log("trying to grab ");
        if (!attached)
        {

            Collider[] hitColliders = Physics.OverlapBox(handSphere.transform.position, targetSphere.transform.localScale, Quaternion.identity);

            foreach (Collider collider in hitColliders)
            {

                if (collider.gameObject.Equals(targetSphere))
                {
                    //UnityEngine.Debug.Log("hit ");

                    collider.gameObject.transform.parent = handSphere.transform;
                    attached = true;
                }
            }
        }
        else {
            targetSphere.gameObject.transform.parent = null;
            attached = false; 

        }

    }

    private void OnGrabActionPerformed(InputAction.CallbackContext context)
    {
        //unityEngine.Debug.Log("grabbed");
        // Call GrabSphere when the new input action is performed
        GrabSphere();
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        //UnityEngine.Debug.Log("calibration");
        SetVisibility(true);
        //targetSphere.transform.position = endEffector.transform.position;
        Calibrated = true;
        if (!hasRun)
        {
            hasRun = true;
            UnityEngine.Debug.Log("trigger pressed");
            StartFranca();
        }
        
    }

    // Update is called once per frame
    void StartFranca()
    {
        // Create the ROS message
        BoolMessage message = new BoolMessage
        {
            data = true
        };

        // Finally, send the message to ROS
        ros.Publish(topicName, message);
    }
}
    