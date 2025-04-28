using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotCapture : MonoBehaviour
{
    public int frameRate = 30;
    public int totalFrames = 150;
    private int frameCount = 0;
    private string savePath;

    // Start is called before the first frame update
    void Start()
    {
        //savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "user_study");
        string unityProjectPath = Directory.GetParent(Application.dataPath).FullName;
        string mainProjectPath = Directory.GetParent(unityProjectPath).FullName;
        savePath = Path.Combine(mainProjectPath, "gaussian_splatting_setup", "splat_input", "input");
        //UnityEngine.Debug.Log(savePath);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath); 
        }
        
        Time.captureFramerate = frameRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount < totalFrames)
        {
            string filename = Path.Combine(savePath, $"frame_{frameCount:D04}.jpg");
            ScreenCapture.CaptureScreenshot(filename);
            frameCount++;
        }
    }
}
