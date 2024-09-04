using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using System.Collections.Generic;

public class PointCloudSubscriber : MonoBehaviour
{
    public Material pointCloudMaterial;
    public GameObject meshPrefab;

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Color> colors = new List<Color>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<PointCloud2Msg>("/combined_pointcloud", OnPointCloudReceived);
        mesh = new Mesh();
    }

    void OnPointCloudReceived(PointCloud2Msg msg)
    {
        UnityEngine.Debug.Log("pointcloud recieved");
        GenerateMeshFromPointCloud(msg);
    }

    void GenerateMeshFromPointCloud(PointCloud2Msg msg)
    {
        UnityEngine.Debug.Log(msg.height);
        UnityEngine.Debug.Log(msg.width);

        vertices.Clear();
        colors.Clear();
        triangles.Clear();

        int pointStep = (int)msg.point_step;
        int rowStep = (int)msg.row_step;
        int numPoints = (int)(msg.width * msg.height);

        byte[] data = msg.data;

        for (int i = 0; i < numPoints; i++)
        {
            int baseIndex = i * pointStep;

            float x = System.BitConverter.ToSingle(data, baseIndex + (int)msg.fields[0].offset);
            float y = System.BitConverter.ToSingle(data, baseIndex + (int)msg.fields[1].offset);
            float z = System.BitConverter.ToSingle(data, baseIndex + (int)msg.fields[2].offset);
            vertices.Add(new Vector3(x, y, -z));

            byte r = data[baseIndex + (int)msg.fields[3].offset];
            byte g = data[baseIndex + (int)msg.fields[3].offset + 1];
            byte b = data[baseIndex + (int)msg.fields[3].offset + 2];
            colors.Add(new Color32(r, g, b, 255));
        }

        UnityEngine.Debug.Log("generating mesh triangles");
        
        // Generate triangles for the mesh
        for (int i = 0; i < (int)msg.height - 1; i++)
        {
            for (int j = 0; j < (int)msg.width - 1; j++)
            {
                UnityEngine.Debug.Log(msg.height);
                UnityEngine.Debug.Log(msg.width);

                int index = i * (int)msg.width + j;
                triangles.Add(index);
                triangles.Add(index + (int)msg.width);
                triangles.Add(index + 1);

                triangles.Add(index + 1);
                triangles.Add(index + (int)msg.width);
                triangles.Add(index + (int)msg.width + 1);

                UnityEngine.Debug.Log(triangles.Count);
            }
        }

        Debug.Log($"Vertices Count: {vertices.Count}");
        Debug.Log($"Triangles Count: {triangles.Count}");


        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetColors(colors);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        // Instantiate mesh prefab
        GameObject meshObject = Instantiate(meshPrefab);

        // Ensure MeshFilter is attached to the GameObject
        MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = meshObject.AddComponent<MeshFilter>();
        }

        // Assign the generated mesh
        meshFilter.mesh = mesh;

        // Ensure MeshRenderer is attached to the GameObject and assign the material
        MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = pointCloudMaterial;
    }
}
