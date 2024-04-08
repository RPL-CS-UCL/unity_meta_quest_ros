using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityMetaQuest; // Make sure this namespace matches your actual ROS message types

public class RosMetaControllerLeft : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "controller_left_state";

    // Input Action References
    public InputActionReference LeftTriggerActionReference;
    public InputActionReference LeftGripActionReference;
    public InputActionReference LeftPrimaryButtonActionReference;
    public InputActionReference LeftSecondaryButtonActionReference;
    public InputActionReference LeftThumbstickActionReference;

    // Publisher frequency
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // Start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ControllerStateMsg>(topicName);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            // Read button states using the `triggered` attribute for digital inputs
            bool triggerPressed = LeftTriggerActionReference.action.IsPressed();
            bool gripPressed = LeftGripActionReference.action.IsPressed();
            bool primaryButtonPressed = LeftPrimaryButtonActionReference.action.IsPressed();
            bool secondaryButtonPressed = LeftSecondaryButtonActionReference.action.IsPressed();

            // Read thumbstick position
            Vector2 thumbstickPosition = LeftThumbstickActionReference.action.ReadValue<Vector2>();

            // Create the ROS message
            ControllerStateMsg message = new ControllerStateMsg
            {
                thumbstick_x = thumbstickPosition.x,
                thumbstick_y = thumbstickPosition.y,
                trigger_pressed = triggerPressed,
                grip_pressed = gripPressed,
                primary_button_pressed = primaryButtonPressed,
                secondary_button_pressed = secondaryButtonPressed
            };

            // Finally, send the message to ROS
            ros.Publish(topicName, message);

            timeElapsed = 0;
        }
    }
}
