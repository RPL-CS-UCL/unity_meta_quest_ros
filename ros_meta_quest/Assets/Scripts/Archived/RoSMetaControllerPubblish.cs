using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityMetaQuest;

/// <summary>
///
/// </summary>
public class RosPublisherExample : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "pos_rot_meta_quest";

    // The game object
    public GameObject sphere_right,sphere_left,head_cube;
    // Publisher frequency
    public float publishMessageFrequency = 0.01f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PosRotListMsg>(topicName);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            //sphere_right.transform.rotation = Random.rotation;

            PosRotMsg controller_right = new PosRotMsg(
                sphere_right.transform.position.x,
                sphere_right.transform.position.y,
                sphere_right.transform.position.z,
                sphere_right.transform.rotation.x,
                sphere_right.transform.rotation.y,
                sphere_right.transform.rotation.z,
                sphere_right.transform.rotation.w,
                "controller_right"
            );

            PosRotMsg controller_left = new PosRotMsg(
                sphere_left.transform.position.x,
                sphere_left.transform.position.y,
                sphere_left.transform.position.z,
                sphere_left.transform.rotation.x,
                sphere_left.transform.rotation.y,
                sphere_left.transform.rotation.z,
                sphere_left.transform.rotation.w,
                "controller_left"
            );

            PosRotMsg head_tracker = new PosRotMsg(
                0,
                0,
                0,
                head_cube.transform.rotation.x,
                head_cube.transform.rotation.y,
                head_cube.transform.rotation.z,
                head_cube.transform.rotation.w,
                "head_tracker"
                );

            // Create an array of PosRotMsg objects with two elements
            PosRotMsg[] devicesArray = new PosRotMsg[] {controller_right, controller_left, head_tracker};

            PosRotListMsg devices = new PosRotListMsg(devicesArray);



            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, devices);

            timeElapsed = 0;
        }
    }
}