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
using NUnit.Framework.Interfaces;
//using static UnityEditor.Rendering.CameraUI;
using System;
using Unity.Robotics.UrdfImporter;
using Unity.VisualScripting;

public class Franka_Subscriber : MonoBehaviour
{
    public GameObject m_VFXTEMPLATE;
    public GameObject RuntimeImporterHolder;

    private string configPath;
    private string splatPath;

    private string mambaSubPath;
    private string mambaPath;
    private string mambaActivatePath;

    private int imageCounter = 0;
  
    private string inputPath;

    private string conversionPath;
    private string transformPath;

    string splatinputPath;
    string splatoutputPath;

    private string finalOutputPath;

    //choose to do this globally and remove AddSplatToScene arguments or remove global variables but don't do both
    public Vector3 splatPosition = new Vector3((float)-0.03, (float)1.315, (float)0.6);

    // Define the desired rotation
    public Quaternion splatRotation = Quaternion.Euler(295, 293, 185);

    public Vector3 splatScale = new Vector3((float)0.15, (float)0.15, (float)0.15);

    // Start is called before the first frame update
    void Start()
    {
        InitializePaths();    

        //RunGaussianSplatting();
        //AddSplatToScene(splatPosition, splatRotation, splatScale);

        // Delete the specified directories
        //DeleteDirectories(splatinputPath, "input");
        //DeleteDirectories(splatoutputPath, "");

        // Delete any existing images in the folder
        //DeleteExistingImages();

        // Subscribe to the ROS topic
        ROSConnection.GetOrCreateInstance().Subscribe<RosImage>("/camera/image_compressed", ImgCallback); //replace with ros node from Franca
    }

    private void InitializePaths()
    {
        configPath = Path.Combine(Application.dataPath, "gaussian_splatting_config.txt");

        string[] lines = File.ReadAllLines(configPath);
        if (lines.Length < 2) 
        {
            UnityEngine.Debug.LogError("Config file does not contain enough paths.");
            return;
        }

        splatPath = lines[0].Trim();
        mambaPath = lines[1].Trim();

        mambaSubPath = "condabin\\mamba.bat";
        mambaActivatePath = Path.Combine(mambaPath, mambaSubPath);

        string unityProjectPath = Directory.GetParent(Application.dataPath).FullName;
        string mainProjectPath = Directory.GetParent(unityProjectPath).FullName;
        string setupPath = Path.Combine(mainProjectPath, "gaussian_splatting_setup");
        conversionPath = Path.Combine(setupPath, "convert_ply_splat.py");
        transformPath = Path.Combine(setupPath, "transform_ply.py");

        splatinputPath = Path.Combine(setupPath, "splat_input");
        splatoutputPath = Path.Combine(setupPath, "splat_output");

        finalOutputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "output.splat"));
    }

    private void ImgCallback(RosImage msgIn)
    {
        //name image and place it in [C:\Users\RoboH\unity_meta_quest_ros\]gaussian_splatting_setup\splat_input NOT C:\Users\takuy\gaussian-splatting\test_data\test_vid\input

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

    private void DeleteDirectories(string directoryPath, string exceptionFolder)
    {
        if (Directory.Exists(directoryPath))
        {
            foreach (string file in Directory.GetFiles(directoryPath) )
            {
                File.Delete(file);
            }

            foreach (string dir  in Directory.GetDirectories(directoryPath)) 
            { 
                if (Path.GetFileName(dir) != exceptionFolder)
                {
                    Directory.Delete(dir, true);
                }
            }
        }
        else
        {
            UnityEngine.Debug.LogError(directoryPath + " doesn't exist");
        }
    }

    async void RunGaussianSplatting()
    {
        UnityEngine.Debug.Log("run gaussian splatting");

        string arguments = $"/c {mambaActivatePath} activate gaussian_splatting && " +
                           $"python {Path.Combine(splatPath, "convert.py")} -s {splatinputPath} && " +
                           $"python {Path.Combine(splatPath, "train.py")} -s {splatinputPath} -m {splatoutputPath} --iterations 15000 && " +
                           $"{mambaActivatePath} activate transform && " +
                           $"python {transformPath} && " +
                           $"{mambaActivatePath} activate gaussian_splatting && " +
                           $"python {conversionPath} {Path.Combine(splatoutputPath, "transformed_point_cloud.ply")} -o {finalOutputPath}";
        // use input and output folders in gaussian_splatting_setup NOT folders in gaussian-splatting
        // find script which recieves images from NUC so you use the same folders there

        await Task.Run(() => RunProcess(@"cmd.exe", arguments));
        AddSplatToScene(splatPosition, splatRotation, splatScale);
    }

    void RunProcess(string fileName, string arguments)
    {
        //print(arguments);
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

    void AddSplatToScene(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        UnityEngine.Debug.Log("add splat to scene");

        RuntimeImporter splatImporter = RuntimeImporterHolder.GetComponent<RuntimeImporter>();

        SplatData data = splatImporter.ImportAsSplatData(finalOutputPath);
        GameObject t = splatImporter.init(m_VFXTEMPLATE, data);

        // Instantiate the VFX
        GameObject vfxInstance = Instantiate(t);

        // Set to layer
        vfxInstance.layer = LayerMask.NameToLayer("SplatLayer");

        // Create a new empty GameObject to act as the parent
        GameObject parentObject = new GameObject("VFX Parent");



        // Make the vfxInstance a child of the parentObject
        vfxInstance.transform.SetParent(parentObject.transform);

        // Center the VFX by setting its local position to zero (do all these through UI not script)
        vfxInstance.transform.position = position;
        vfxInstance.transform.rotation = rotation;
        vfxInstance.transform.localScale = scale; // world version of this?
        //vfxInstance.transform.localRotation = rotation;
        //vfxInstance.transform.localScale = scale;
        // parentObject.transform.localScale = new Vector3((float)1.0, (float)1.0, (float)1.0); //0.02, 0.02, 0.02
        vfxInstance.GetComponent<VisualEffect>().Reinit();

        // Set the parent GameObject's position and rotation
        parentObject.transform.position = new Vector3(0, 0, 0);//new Vector3((float)-2.5, (float)-2.0, (float)0.0); //-0.15, 0.9, 0.25
        parentObject.transform.rotation = Quaternion.Euler(0, 0, 0); //45, 180, 0

        // Optionally, if the VFX needs to be scaled or further adjusted, you can do it here
    }

}
