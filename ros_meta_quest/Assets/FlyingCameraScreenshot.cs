using UnityEngine;
using System.IO;
using System.Collections;

public class FlyingCameraScreenshot : MonoBehaviour
{
    public Camera captureCamera; // Assign your camera in the Inspector
    public int totalImagesPerRotation = 30; // Number of images per full rotation
    public int totalRings = 3; // Capture at 3 height levels
    public float radius = 0.2f; // Distance from the center
    public Vector3 center = new Vector3(0f,0.5f,0f);//Vector3.zero; // Center of the circle
    private string savePath;
    private Vector3 newPosition;
    private float[] heights;
    private int imgcount = 0;

    void Start()
    {
        heights = new float[3];
        heights[0] = 1.0f;
        heights[1] = 1.2f;
        heights[2] = 1.5f;

        captureCamera.transform.position = new Vector3(0.2f,1.2f,0.3f);

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
        for (int ring = 0; ring < totalRings; ring++)
        {
            //float height = Mathf.Lerp(1f, 5f, ring / (float)(totalRings - 1)); // Adjust heights
            float height = heights[ring];

            for (int i = 0; i < totalImagesPerRotation; i++)
            {
                float angle = (i / (float)totalImagesPerRotation) * 360f;
                float radian = angle * Mathf.Deg2Rad;

                /*
                UnityEngine.Debug.Log("---------------------------");
                UnityEngine.Debug.Log("angle: " + angle);
                UnityEngine.Debug.Log("radian: " + radian);
                UnityEngine.Debug.Log("cos(radian): " + Mathf.Cos(radian));
                UnityEngine.Debug.Log("sin(radian): " + Mathf.Sin(radian));
                UnityEngine.Debug.Log("center x: " + center.x);
                UnityEngine.Debug.Log("center y: "+ center.y);
                UnityEngine.Debug.Log("radius: " + radius);
                */

                // Calculate new position around the center
                newPosition = new Vector3(
                    (center.x + radius * Mathf.Cos(radian)),
                    height,
                    (center.z + radius * Mathf.Sin(radian))
                );
                captureCamera.transform.position = newPosition;
                //UnityEngine.Debug.Log("new position: "+ newPosition);

                // Look at the center
                captureCamera.transform.LookAt(center);

                //Debug.Log($"Capturing at ring {ring}, angle {angle} degrees");

                yield return new WaitForEndOfFrame();
                //SaveScreenshot(ring, i);
                SaveScreenshot();
            }
        }
    }

    private void SaveScreenshot()//int ring, int index)
    {
        //string filename = Path.Combine(savePath, $"ring{ring}_frame{index:D02}.jpg");

        //string filename = Path.Combine(savePath, $"frame_{imgcount:D04}.jpg");
        // Format the image name to be 4 digits with leading zeros
        string imgname = imgcount.ToString("D4") + ".jpg";
        string filename = Path.Combine(savePath, imgname);
        ScreenCapture.CaptureScreenshot(filename);
        imgcount++;
    }
}