using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class XRTriggerListener : MonoBehaviour
{
    public InputActionReference LeftTriggerActionReference;
    public InputActionReference LeftGripActionReference;
    public InputActionReference LeftPrimaryButtonActionReference;
    public InputActionReference LeftSecondaryButtonActionReference;
    public InputActionReference LeftThumbstickActionReference;
    public TextMeshPro textMesh; // Ensure this references a TextMeshPro object in the scene

    private void OnEnable()
    {
        SubscribeAction(LeftTriggerActionReference, OnTriggerPerformed);
        SubscribeAction(LeftGripActionReference, OnGripPerformed);
        SubscribeAction(LeftPrimaryButtonActionReference, OnPrimaryButtonPerformed);
        SubscribeAction(LeftSecondaryButtonActionReference, OnSecondaryButtonPerformed);
        if (LeftThumbstickActionReference != null)
        {
            LeftThumbstickActionReference.action.Enable();
        }
    }

    private void OnDisable()
    {
        UnsubscribeAction(LeftTriggerActionReference, OnTriggerPerformed);
        UnsubscribeAction(LeftGripActionReference, OnGripPerformed);
        UnsubscribeAction(LeftPrimaryButtonActionReference, OnPrimaryButtonPerformed);
        UnsubscribeAction(LeftSecondaryButtonActionReference, OnSecondaryButtonPerformed);
        if (LeftThumbstickActionReference != null)
        {
            LeftThumbstickActionReference.action.Disable();
        }
    }

    private void Update()
    {
        // Handle continuous thumbstick input
        if (LeftThumbstickActionReference != null)
        {
            Vector2 thumbstickValue = LeftThumbstickActionReference.action.ReadValue<Vector2>();
            if (thumbstickValue != Vector2.zero)
            {
                // Display thumbstick values
                textMesh.text = $"Thumbstick: {thumbstickValue}";
            }
        }
    }

    private void SubscribeAction(InputActionReference actionReference, System.Action<InputAction.CallbackContext> callback)
    {
        if (actionReference != null)
        {
            actionReference.action.performed += callback;
            actionReference.action.Enable();
        }
    }

    private void UnsubscribeAction(InputActionReference actionReference, System.Action<InputAction.CallbackContext> callback)
    {
        if (actionReference != null)
        {
            actionReference.action.performed -= callback;
            actionReference.action.Disable();
        }
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context) => UpdateMessage("Trigger pressed");
    private void OnGripPerformed(InputAction.CallbackContext context) => UpdateMessage("Grip pressed");
    private void OnPrimaryButtonPerformed(InputAction.CallbackContext context) => UpdateMessage("Primary button pressed");
    private void OnSecondaryButtonPerformed(InputAction.CallbackContext context) => UpdateMessage("Secondary button pressed");

    private void UpdateMessage(string message)
    {
        textMesh.text = message;
    }
}
