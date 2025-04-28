using UnityEngine;
using System.IO;
using System.Collections;

public class EndEffectorScreenshotCapture : MonoBehaviour
{
    public Camera endEffectorCamera; // Assign in Inspector
    public Transform endEffector; // Assign the end-effector transform
    public int totalImagesPerRotation = 10; // Number of images per full rotation
    public int totalRings = 3; // Capture at 3 height levels
    public float rotationRadius = 0.5f; // Radius of rotation around target
    private string savePath;

    void Start()
    {
        // Create save directory
        string unityProjectPath = Directory.GetParent(Application.dataPath).FullName;
        string mainProjectPath = Directory.GetParent(unityProjectPath).FullName;
        savePath = Path.Combine(mainProjectPath, "gaussian_splatting_setup", "splat_input", "input");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        StartCoroutine(CaptureAllRings());
    }

    private IEnumerator CaptureAllRings()
    {
        Vector3 initialPosition = endEffector.position;
        Quaternion initialRotation = endEffector.rotation;

        for (int ring = 0; ring < totalRings; ring++)
        {
            float heightOffset = (ring / (float)(totalRings - 1)) - 0.5f; // Offset height
            for (int i = 0; i < totalImagesPerRotation; i++)
            {
                float angle = (i / (float)totalImagesPerRotation) * 360f;
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * rotationRadius, heightOffset, Mathf.Sin(angle * Mathf.Deg2Rad) * rotationRadius);
                endEffector.position = initialPosition + offset;
                endEffector.LookAt(initialPosition);

                yield return new WaitForEndOfFrame(); // Wait for Unity to update frame
                SaveScreenshot(ring, i);
            }
        }

        // Reset to initial position
        endEffector.position = initialPosition;
        endEffector.rotation = initialRotation;
    }

    private void SaveScreenshot(int ring, int index)
    {
        string filename = Path.Combine(savePath, $"ring{ring}_frame{index:D02}.jpg");
        ScreenCapture.CaptureScreenshot(filename);
    }
}