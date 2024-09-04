using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityMetaQuest;
using BoolMessage = RosMessageTypes.Std.BoolMsg;

public class Quest_Franca_Trigger : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "franca_start";
    
    public InputActionReference rightTriggerActionReference; // Reference to your right trigger InputAction
    private bool hasRun = false;

    // Start is called before the first frame update
    void Start()
    {
        // Start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<BoolMessage>(topicName);
    }
    void OnEnable()
    {
        // Enable the input action
        if (rightTriggerActionReference != null && rightTriggerActionReference.action != null)
        {
            rightTriggerActionReference.action.performed += OnRightTriggerPressed;
            rightTriggerActionReference.action.Enable();
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
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
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
    