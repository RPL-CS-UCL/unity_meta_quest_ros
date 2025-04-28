using UnityEngine;
using System.IO;
using Cinemachine;

public class DistinctCapture : MonoBehaviour
{
    public int totalImagesPerRotation = 30; // Number of distinct images per full rotation
    public int totalRings = 3; // Capture at 3 height levels
    public CinemachineFreeLook freeLookCamera; // Assign the Cinemachine FreeLook Camera or the main camera
    private int imageIndex = 0;
    private string savePath;

    void Start()
    {
        freeLookCamera.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        freeLookCamera.m_XAxis.m_InputAxisName = ""; // Completely remove input
        freeLookCamera.m_XAxis.m_InputAxisValue = 0; // Prevent input override
        // Create save directory
        //savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "user_study");
        string unityProjectPath = Directory.GetParent(Application.dataPath).FullName;
        string mainProjectPath = Directory.GetParent(unityProjectPath).FullName;
        savePath = Path.Combine(mainProjectPath, "gaussian_splatting_setup", "splat_input", "input");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        StartCoroutine(CaptureAllRings());
    }

    private System.Collections.IEnumerator CaptureAllRings()
    {
        for (int ring = 0; ring < totalRings; ring++) // Loop through 3 rings
        {
            SetCameraHeight(ring);

            for (int i = 0; i < totalImagesPerRotation; i++) // Full rotation
            {
                //float angle = (360f / totalImagesPerRotation) * i;
                //SetCameraRotation(angle);

                float angle = (i / (float)totalImagesPerRotation) * 360f;
                freeLookCamera.m_XAxis.Value = angle;
                //UnityEngine.Debug.Log(freeLookCamera.m_XAxis.Value = angle * 360f);
                //UnityEngine.Debug.Log("before: "+freeLookCamera.m_XAxis.Value);
                //freeLookCamera.ForceCameraPosition(freeLookCamera.transform.position, freeLookCamera.transform.rotation);
                //UnityEngine.Debug.Log("after: "+freeLookCamera.m_XAxis.Value);
                Debug.Log($"Frame {i}: XAxis {freeLookCamera.m_XAxis.Value}, World Rotation {freeLookCamera.transform.rotation.eulerAngles}");

                yield return new WaitForEndOfFrame(); // Wait for Unity to update frame
                SaveScreenshot(ring, i);
            }
        }
    }

    private void SetCameraHeight(int ring)
    {
        float yValue = ring / (float)(totalRings - 1); // 0 (Bottom), 0.5 (Middle), 1 (Top)
        //freeLookCamera.position = new Vector3(freeLookCamera.position.x, yValue * 2, freeLookCamera.position.z);
        freeLookCamera.m_YAxis.Value = yValue;
    }

    /*
    private void SetCameraRotation(float angle)
    {
        freeLookCamera.eulerAngles = new Vector3(freeLookCamera.eulerAngles.x, angle, freeLookCamera.eulerAngles.z);
    }
    */

    private void SaveScreenshot(int ring, int index)
    {
        string filename = Path.Combine(savePath, $"ring{ring}_frame{index:D02}.jpg");
        ScreenCapture.CaptureScreenshot(filename);
    }
}