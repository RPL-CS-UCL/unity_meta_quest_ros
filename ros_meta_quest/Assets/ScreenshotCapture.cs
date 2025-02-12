using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotCapture : MonoBehaviour
{
    public int frameRate = 30;
    public int totalFrames = 300;
    private int frameCount = 0;
    private string savePath;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "user_study");
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
