using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityMetaQuest; // Make sure this namespace matches your actual ROS message types

public class RosMetaControllerRight : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "controller_right_state";

    // Input Action References
    public InputActionReference RightTriggerActionReference;
    public InputActionReference RightGripActionReference;
    public InputActionReference RightPrimaryButtonActionReference;
    public InputActionReference RightSecondaryButtonActionReference;
    public InputActionReference RightThumbstickActionReference;

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
            bool triggerPressed = RightTriggerActionReference.action.IsPressed();
            bool gripPressed = RightGripActionReference.action.IsPressed();
            bool primaryButtonPressed = RightPrimaryButtonActionReference.action.IsPressed();
            bool secondaryButtonPressed = RightSecondaryButtonActionReference.action.IsPressed();

            // Read thumbstick position
            Vector2 thumbstickPosition = RightThumbstickActionReference.action.ReadValue<Vector2>();

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
