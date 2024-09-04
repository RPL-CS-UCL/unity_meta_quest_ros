using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityMetaQuest;
using System.Runtime.InteropServices.WindowsRuntime; // Make sure this namespace matches your actual ROS message types

public class RosMetaManager : MonoBehaviour
{
    [SerializeField] private string m_rightControllerTopicName = "controller_right";
    [SerializeField] private string m_leftControllerTopicName = "controller_left";
    [SerializeField] private string m_posRotTopicName = "pos_rot_meta_quest";
    private GameObject m_XRRig;
    private GameObject m_headGO;
    private GameObject m_leftControllerGO;
    private GameObject m_rightControllerGO;


    [SerializeField] private UserInputManager m_userInputManager;

    [SerializeField] private float publishMessageFrequency = 0.01f;
    private float m_timeElapsed;
    private ROSConnection m_ros;

    void Start()
    {
        // Start the ROS connection
        m_ros = ROSConnection.GetOrCreateInstance();
        m_ros.RegisterPublisher<ControllerStateMsg>(m_leftControllerTopicName);
        m_ros.RegisterPublisher<ControllerStateMsg>(m_rightControllerTopicName);
        m_ros.RegisterPublisher<PosRotListMsg>(m_posRotTopicName);



        //find the user input component
        m_userInputManager = FindObjectOfType<UserInputManager>();

        m_XRRig = GameObject.FindWithTag("Player");
        m_headGO = GameObject.Find("Main Camera");
        m_leftControllerGO = GameObject.Find("Left Controller");
        m_rightControllerGO = GameObject.Find("Right Controller");
    }

    private void PublishPosRot()
    {


        PosRotMsg controllerRight = new PosRotMsg(
            m_rightControllerGO.transform.position.x,
            m_rightControllerGO.transform.position.y,
            m_rightControllerGO.transform.position.z,
            m_rightControllerGO.transform.rotation.x,
            m_rightControllerGO.transform.rotation.y,
            m_rightControllerGO.transform.rotation.z,
            m_rightControllerGO.transform.rotation.w,
            "controller_right"
        );

        PosRotMsg controllerLeft = new PosRotMsg(
            m_leftControllerGO.transform.position.x,
            m_leftControllerGO.transform.position.y,
            m_leftControllerGO.transform.position.z,
            m_leftControllerGO.transform.rotation.x,
            m_leftControllerGO.transform.rotation.y,
            m_leftControllerGO.transform.rotation.z,
            m_leftControllerGO.transform.rotation.w,
            "controller_left"
        );

        PosRotMsg headTracker = new PosRotMsg(
            0,
            0,
            0,
            m_headGO.transform.rotation.x,
            m_headGO.transform.rotation.y,
            m_headGO.transform.rotation.z,
            m_headGO.transform.rotation.w,
            "head_tracker"
            );

        // Create an array of PosRotMsg objects with two elements
        PosRotMsg[] devicesArray = new PosRotMsg[] { controllerRight, controllerLeft, headTracker };

        PosRotListMsg devices = new PosRotListMsg(devicesArray);
        m_ros.Publish(m_posRotTopicName, devices);

    }

    private ControllerStateMsg CreateControllerStateMessage(Vector2 thumbstick, bool triggerPressed, bool gripPressed, bool primaryButtonPressed, bool secondaryButtonPressed)
    {

        return new ControllerStateMsg
        {
            thumbstick_x = thumbstick.x,
            thumbstick_y = thumbstick.y,
            trigger_pressed = triggerPressed,
            grip_pressed = gripPressed,
            primary_button_pressed = primaryButtonPressed,
            secondary_button_pressed = secondaryButtonPressed
        };
    }

    private void PublishControllers()
    {

        ControllerStateMsg leftState = CreateControllerStateMessage(
            m_userInputManager.Left_thumbstickPosition,
            m_userInputManager.Left_triggerPressed,
            m_userInputManager.Left_gripPressed,
            m_userInputManager.Left_primaryButtonPressed,
            m_userInputManager.Left_secondaryButtonPressed
            );


        ControllerStateMsg rightState = CreateControllerStateMessage(
            m_userInputManager.Right_thumbstickPosition,
            m_userInputManager.Right_triggerPressed,
            m_userInputManager.Right_gripPressed,
            m_userInputManager.Right_primaryButtonPressed,
            m_userInputManager.Right_secondaryButtonPressed
            );

        m_ros.Publish(m_leftControllerTopicName, leftState);
        m_ros.Publish(m_rightControllerTopicName, rightState);
    }


    void Update()
    {
        //UnityEngine.Debug.Log(Time.deltaTime);
        m_timeElapsed += Time.deltaTime;

        //MUCH faster with this removed
        //if (m_timeElapsed <= publishMessageFrequency)
            //return;

        PublishControllers();
        PublishPosRot();
        m_timeElapsed = 0;


    }
}
