using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;



public class UserInputManager : MonoBehaviour
{
    public bool Right_triggerPressed = false;
    public bool Right_gripPressed = false;
    public bool Right_primaryButtonPressed = false;
    public bool Right_secondaryButtonPressed = false;
    public Vector2 Right_thumbstickPosition = Vector2.zero;

    public bool Left_triggerPressed = false;
    public bool Left_gripPressed = false;
    public bool Left_primaryButtonPressed = false;
    public bool Left_secondaryButtonPressed = false;
    public Vector2 Left_thumbstickPosition = Vector2.zero;

    [SerializeField] private InputActionReference m_RightTriggerActionReference;
    [SerializeField] private InputActionReference m_RightGripActionReference;
    [SerializeField] private InputActionReference m_RightPrimaryButtonActionReference;
    [SerializeField] private InputActionReference m_RightSecondaryButtonActionReference;
    [SerializeField] private InputActionReference m_RightThumbstickActionReference;

    [SerializeField] private InputActionReference m_LeftTriggerActionReference;
    [SerializeField] private InputActionReference m_LeftGripActionReference;
    [SerializeField] private InputActionReference m_LeftPrimaryButtonActionReference;
    [SerializeField] private InputActionReference m_LeftSecondaryButtonActionReference;
    [SerializeField] private InputActionReference m_LeftThumbstickActionReference;


    void Start()
    {

    }


    private void UpdateRightController()
    {
        Right_triggerPressed = m_RightTriggerActionReference.action.IsPressed();
        Right_gripPressed = m_RightGripActionReference.action.IsPressed();
        Right_primaryButtonPressed = m_RightPrimaryButtonActionReference.action.IsPressed();
        Right_secondaryButtonPressed = m_RightSecondaryButtonActionReference.action.IsPressed();
        Right_thumbstickPosition = m_RightThumbstickActionReference.action.ReadValue<Vector2>();
    }
    private void UpdateLeftController()
    {
        Left_triggerPressed = m_LeftTriggerActionReference.action.IsPressed();
        Left_gripPressed = m_LeftGripActionReference.action.IsPressed();
        Left_primaryButtonPressed = m_LeftPrimaryButtonActionReference.action.IsPressed();
        Left_secondaryButtonPressed = m_LeftSecondaryButtonActionReference.action.IsPressed();
        Left_thumbstickPosition = m_LeftThumbstickActionReference.action.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRightController();
        UpdateLeftController();

    }
}
