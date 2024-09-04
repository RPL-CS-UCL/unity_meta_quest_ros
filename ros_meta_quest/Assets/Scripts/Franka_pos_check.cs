using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

// This script subscribes to the "/franka/position" topic and spawns a sphere at the received position
public class FrankaPositionSubscriber : MonoBehaviour
{
    // Reference to the sphere prefab to instantiate
    public GameObject spherePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the ROS topic "/franka/position"
        ROSConnection.GetOrCreateInstance().Subscribe<Float64MultiArrayMsg>("/franka/position", PositionCallback);
    }

    // Callback function that gets executed when a new message is received
    void PositionCallback(Float64MultiArrayMsg msg)
    {
        UnityEngine.Debug.Log("message recieved: "+ msg);

        // Extract position data from the received message
        //Vector3 position = new Vector3((float)msg.data[0], (float)msg.data[1], (float)msg.data[2]);
        // Z and Y swapped
        Vector3 position = new Vector3((float)msg.data[0]-(float)0.4, (float)msg.data[2]+(float)0.3, (float)msg.data[1]-(float)0.8);

        // Instantiate a sphere at the received position
        Instantiate(spherePrefab, position, Quaternion.identity);
    }
}
