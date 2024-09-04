using SplatVfx;
using System.Diagnostics;
using System.IO;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using RosImage = RosMessageTypes.Sensor.CompressedImageMsg;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class Franca_Subscriber : MonoBehaviour
{
    public GameObject m_VFXTEMPLATE;

    private int imageCounter = 0;
    private const string imagePath = @"C:\Users\takuy\gaussian-splatting\test_data\test_vid\input\";

    // Paths of directories to be deleted
    private const string sparsePath = @"C:\Users\takuy\gaussian-splatting\test_data\test_vid\sparse";
    private const string distortedPath = @"C:\Users\takuy\gaussian-splatting\test_data\test_vid\distorted";
    private const string pointcloudPath = @"C:\Users\takuy\gaussian-splatting\output\test_data\point_cloud";

    public Vector3 splatPosition = new Vector3(3, 3, 3);

    // Define the desired rotation
    Quaternion splatRotation = Quaternion.Euler(-90, 80, 0);

    // Start is called before the first frame update
    void Start()
    {
        // Delete the specified directories
        DeleteDirectories();

        // Delete any existing images in the folder
        DeleteExistingImages();

        // Subscribe to the ROS topic
        ROSConnection.GetOrCreateInstance().Subscribe<RosImage>("/camera/image_compressed", ImgCallback); //replace with ros node from Franca
    }

    private void ImgCallback(RosImage msgIn)
    {
        //name image and place it in C:\Users\takuy\gaussian-splatting\test_data\test_vid\input
        
        // Increment the image counter
        imageCounter++;
        UnityEngine.Debug.Log("imageCounter: "+imageCounter);

        // Format the image name to be 4 digits with leading zeros
        string imageName = imageCounter.ToString("D4") + ".jpg";

        // Convert the compressed image data to a byte array
        byte[] imageData = msgIn.data;

        // Save the image to the specified path
        string fullPath = Path.Combine(imagePath, imageName);
        File.WriteAllBytes(fullPath, imageData);

        // Check if the imageCounter has reached 100
        if (imageCounter == 100)
        {
            // Call the RunGaussianSplatting function
            UnityEngine.Debug.Log("recieved 100 images");
            RunGaussianSplatting();
        }
    }

    private void DeleteExistingImages()
    {
        if (Directory.Exists(imagePath))
        {
            foreach (string file in Directory.GetFiles(imagePath, "*.jpg"))
            {
                File.Delete(file);
            }
        }
    }

    private void DeleteDirectories()
    {
        // Delete the sparse directory if it exists
        if (Directory.Exists(sparsePath))
        {
            Directory.Delete(sparsePath, true);
            UnityEngine.Debug.Log("Deleted directory: " + sparsePath);
        }

        // Delete the distorted directory if it exists
        if (Directory.Exists(distortedPath))
        {
            Directory.Delete(distortedPath, true);
            UnityEngine.Debug.Log("Deleted directory: " + distortedPath);
        }

        // Delete the pointcloud directory if it exists
        if (Directory.Exists(pointcloudPath))
        {
            Directory.Delete(pointcloudPath, true);
            UnityEngine.Debug.Log("Deleted directory: " + pointcloudPath);
        }
    }

    async void RunGaussianSplatting()
    {
        UnityEngine.Debug.Log("run gaussian splatting");
        await Task.Run(() => RunProcess(
            @"cmd.exe",
            @"/c conda activate gaussian_splatting && python C:\Users\takuy\gaussian-splatting\convert.py -s C:\Users\takuy\gaussian-splatting\test_data\test_vid && python C:\Users\takuy\gaussian-splatting\train.py -s C:\Users\takuy\gaussian-splatting\test_data\test_vid -m C:\Users\takuy\gaussian-splatting\output\test_data --iterations 15000 && python C:\Users\takuy\gaussian-splatting\convert_ply_splat.py C:\Users\takuy\gaussian-splatting\output\test_data\point_cloud\iteration_15000\point_cloud.ply -o C:\Users\takuy\unity_meta_quest_ros\ros_meta_quest\Assets\output.splat"
        ));

        AddSplatToScene(splatPosition, splatRotation);
    }

    void RunProcess(string fileName, string arguments)
    {
        print(arguments);
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(psi))
        {
            process.OutputDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log("Output: " + e.Data);
                }
            };
            process.ErrorDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.LogError("Error: " + e.Data);
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            UnityEngine.Debug.Log("exited: " + process.ExitCode);
        }
    }

    void AddSplatToScene(Vector3 position, Quaternion rotation)
    {
        UnityEngine.Debug.Log("add splat to scene");
        RuntimeImporter splatImporter = new RuntimeImporter();
        SplatData data = splatImporter.ImportAsSplatData("C:/Users/takuy/unity_meta_quest_ros/ros_meta_quest/Assets/output.splat");
        
        GameObject t = splatImporter.init(m_VFXTEMPLATE, data);

        GameObject vfxInstance = Instantiate(t);

        // Set the position and rotation of the instantiated VFX
        vfxInstance.transform.position = position;
        vfxInstance.transform.rotation = rotation;
    }
}
