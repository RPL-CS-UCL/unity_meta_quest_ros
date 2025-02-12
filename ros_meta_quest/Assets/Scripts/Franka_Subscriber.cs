using SplatVfx;
using System.Diagnostics;
using System.IO;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using RosImage = RosMessageTypes.Sensor.CompressedImageMsg;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine.VFX;
//using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Franka_Subscriber : MonoBehaviour
{
    public GameObject m_VFXTEMPLATE;

    private int imageCounter = 0;
    private const string inputPath = @"C:\Users\takuy\gaussian-splatting\test_data\test_vid\input\";

    private const string imagesPath = @"C:\Users\takuy\gaussian-splatting\test_data\test_vid\images\";

    // Paths of directories to be deleted
    private const string sparsePath = @"C:\Users\takuy\gaussian-splatting\test_data\test_vid\sparse";
    private const string distortedPath = @"C:\Users\takuy\gaussian-splatting\test_data\test_vid\distorted";
    private const string pointcloudPath = @"C:\Users\takuy\gaussian-splatting\output\test_data\point_cloud";

    public Vector3 splatPosition = new Vector3(0, 0, 0);//new Vector3(3, 40, 40);
    //public Vector3 splatPosition = new Vector3(-100, 0, 0);

    // Define the desired rotation
    Quaternion splatRotation = Quaternion.Euler(0, 0, 0);//Quaternion.Euler(-90, -90, -15);

    // Start is called before the first frame update
    void Start()
    {
        //RunGaussianSplatting();
        //AddSplatToScene(splatPosition, splatRotation);
        // Delete the specified directories
        //DeleteDirectories();

        // Delete any existing images in the folder
        //DeleteExistingImages();

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
        string fullPath = Path.Combine(inputPath, imageName);
        File.WriteAllBytes(fullPath, imageData);

        // Check if the imageCounter has reached 100
        if (imageCounter == 100) //100
        {
            // Call the RunGaussianSplatting function
            UnityEngine.Debug.Log("recieved 100 images");
            RunGaussianSplatting();
        }
    }

    private void DeleteExistingImages()
    {
        if (Directory.Exists(inputPath))
        {
            foreach (string file in Directory.GetFiles(inputPath, "*.jpg"))
            {
                File.Delete(file);
            }
        }
    }

    private void DeleteDirectories()
    {
        if (Directory.Exists(sparsePath))
        {
            Directory.Delete(imagesPath, true);
            UnityEngine.Debug.Log("Deleted directory: " + imagesPath);
        }

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
            @"/c C:\Users\takuy\miniforge\condabin\mamba.bat activate gaussian_splatting && python C:\Users\takuy\gaussian-splatting\convert.py -s C:\Users\takuy\gaussian-splatting\test_data\test_vid && python C:\Users\takuy\gaussian-splatting\train.py -s C:\Users\takuy\gaussian-splatting\test_data\test_vid -m C:\Users\takuy\gaussian-splatting\output\test_data --iterations 15000 && C:\Users\takuy\miniforge\condabin\mamba.bat activate transform && python C:\Users\takuy\gaussian-splatting\transform_ply.py && C:\Users\takuy\miniforge\condabin\mamba.bat activate gaussian_splatting && python C:\Users\takuy\gaussian-splatting\convert_ply_splat.py C:\Users\takuy\gaussian-splatting\transformed_point_cloud.ply -o C:\Users\takuy\unity_meta_quest_ros\ros_meta_quest\Assets\output.splat"
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

    /*
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
    */
    void AddSplatToScene(Vector3 position, Quaternion rotation)
    {
        UnityEngine.Debug.Log("add splat to scene");
        RuntimeImporter splatImporter = new RuntimeImporter();
        SplatData data = splatImporter.ImportAsSplatData("C:/Users/takuy/unity_meta_quest_ros/ros_meta_quest/Assets/output.splat");
        //SplatData data = splatImporter.ImportAsSplatData("C:/Users/takuy/Documents/test1_output.splat");
        GameObject t = splatImporter.init(m_VFXTEMPLATE, data);

        // Instantiate the VFX
        GameObject vfxInstance = Instantiate(t);

        // Create a new empty GameObject to act as the parent
        GameObject parentObject = new GameObject("VFX Parent");



        // Make the vfxInstance a child of the parentObject
        vfxInstance.transform.SetParent(parentObject.transform);

        // Center the VFX by setting its local position to zero
        vfxInstance.transform.localPosition = position;//new Vector3((float)-2.5, -1, (float)0);
        vfxInstance.transform.localRotation = rotation;//Quaternion.Euler(0, 0, 0);
        //vfxInstance.transform.localScale = new Vector3((float)0.1, (float)0.1, (float)0.1);
        parentObject.transform.localScale = new Vector3((float)0.02, (float)0.02, (float)0.02); //0.1
        vfxInstance.GetComponent<VisualEffect>().Reinit();

        // Set the parent GameObject's position and rotation
        parentObject.transform.position = new Vector3((float)-0.15, (float)0.9, (float)0.25);//(float)0.05, (float)0.0, (float)0.46
        parentObject.transform.rotation = Quaternion.Euler(45, 180, 0);//(-45, 0, 0);

        // Optionally, if the VFX needs to be scaled or further adjusted, you can do it here
    }

}
