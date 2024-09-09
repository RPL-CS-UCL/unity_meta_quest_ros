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

public class Quest_Franca_Trigger : MonoBehaviour
{
    ROSConnection ros;
    //ROSConnection movement;
    public string topicName = "franca_start";
// public string jointsTopicname = "new_pos";
    public string cartTopicName = "sphere_pos";
    
    public InputActionReference rightTriggerActionReference; // Reference to your right trigger InputAction
    public InputActionReference grabActionReference; // Reference to your grab InputAction

    private bool hasRun = false;

    public GameObject handSphere;
    public GameObject targetSphere;
    public bool attached = false;

    public GameObject endEffector;
    public Controller controller;

    public bool Calibrated;
    
    //public float[] m_jointAngles;

    // Start is called before the first frame update
    void Start()
    {
        //m_jointAngles = new float[8];
        Calibrated = false;
        // Start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        /*
        if (ros == null)
        {
            UnityEngine.Debug.LogError("ROS connection is not initialized!");
            return;
        }
        else
        {
            UnityEngine.Debug.Log("ROS connection initialized successfully.");
        }
        */
        ros.RegisterPublisher<BoolMessage>(topicName);
        ros.Subscribe<ArrayMessage>("new_pos", JointsCallback);
        //movement = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ArrayMessage>(cartTopicName);
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

        //controller.SetJoints(controller.m_jointAngles);
        /*
        if (Calibrated == false)
        {
            UnityEngine.Debug.Log(targetSphere.transform.position);
            UnityEngine.Debug.Log(endEffector.transform.position);
            targetSphere.transform.position = endEffector.transform.position;
            Calibrated = true;
            UnityEngine.Debug.Log(targetSphere.transform.position);
            UnityEngine.Debug.Log(endEffector.transform.position);
        }
        */
        /*
        else
        {
            UnityEngine.Debug.Log("calibrated");
            UnityEngine.Debug.Log(targetSphere.transform.position);
            UnityEngine.Debug.Log(endEffector.transform.position);
        }
        */
    }

    private void Update()

    {
        if (Calibrated == true)
        {
            ArrayMessage arrayMessage = new ArrayMessage();
            arrayMessage.data = new double[3];
            // Populate the array with target sphere position (axes swapped for Franka frame of reference)
            arrayMessage.data[0] = targetSphere.transform.position.z;
            arrayMessage.data[1] = -targetSphere.transform.position.x;
            arrayMessage.data[2] = targetSphere.transform.position.y;
            ros.Publish(cartTopicName, arrayMessage);
        }
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
        targetSphere.transform.position = endEffector.transform.position;
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
    