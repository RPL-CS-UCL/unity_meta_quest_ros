using UnityEngine;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;
//using RosImage = RosMessageTypes.UnityMetaQuest.MyImageMsg;
//using RosImage = RosMessageTypes.Sensor.ImageMsg;
using RosImage = RosMessageTypes.Sensor.CompressedImageMsg;
using System.Collections;
using System.IO;
using System.Drawing;
using Unity.Barracuda;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Diagnostics;
using System;
using UnityEngine.Rendering;

public class ImageSubscriber: MonoBehaviour
{
    [SerializeField] private Image m_displayImage; // Reference to a UI Image component to display the texture

    //public UnityEngine.UI.RawImage rawImage;

    //public NNModel modelAsset;
    //private Model m_RuntimeModel;


    static int width = 320;//640; //1920
    static int height = 240;//480; //1080

    //static int scaling_factor = 4;

    //static int new_width = width * scaling_factor;
    //static int new_height = height * scaling_factor;

    // Decode the ROS image message to a Texture2D
    Texture2D texture;

    //Texture2D upsampled_texture;

    //public Unity.Barracuda.IWorker m_Worker;

    // Populate the Texture2D with the raw byte data
    //Color32[] pixelData;

    public bool TextureUpdated = true;

    //RenderTexture rTexture;

    //Rect emptyRect = new Rect(0, 0, new_width, new_height);

    //Tensor input_tensor;
    //Tensor output_tensor;


    [SerializeField] private Material m_material;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("start");
        //m_RuntimeModel = Unity.Barracuda.ModelLoader.Load(modelAsset, false, false);
        //m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel); //ComputePrecompiled for GPU, CSharpBurst for CPU
        //m_Worker = WorkerFactory.CreateWorker(m_RuntimeModel, WorkerFactory.Device.GPU);

        texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        //upsampled_texture = new Texture2D(new_width, new_height, TextureFormat.RGB24, false); // multiply height and width by model upscaling factor 

        ROSConnection.GetOrCreateInstance().Subscribe<RosImage>("/camera/image_compressed2", ImgCallback); //("/robot/front_ptz_camera/image_color/compressed", ImgCallback);

        //rTexture = new RenderTexture(new_width, new_height, 0, RenderTextureFormat.Default);
        //rTexture = new RenderTexture(new_width, new_height, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.R8_SRGB);//, readWrite: RenderTextureReadWrite.sRGB);
        //rTexture.enableRandomWrite = true; // Enable random write if needed
        //rTexture.Create();
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (TextureUpdated)
        {
            /*
            // convert texture to tensor
            float[] floatValues = new float[texture.width * texture.height * 3];
            UnityEngine.Color pixel;
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    pixel = texture.GetPixel(x, y);
                    int index = (y * texture.width + x) * 3;
                    floatValues[index] = pixel.r; // Red channel
                    floatValues[index + 1] = pixel.g; // Green channel
                    floatValues[index + 2] = pixel.b; // Blue channel
                }
            }
            */

            
            //input_tensor = new Tensor(texture);
            //print("input tensor " + input_tensor.shape);

            // run the model on the tensor
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //m_Worker.Execute(input_tensor); //FIX
            //stopwatch.Stop();
            //TimeSpan elapsedTime = stopwatch.Elapsed;
            //print($"Elapsed Time: {elapsedTime.TotalMilliseconds} ms");

            //input_tensor.Dispose();
            //output_tensor = m_Worker.PeekOutput();
            //print("output tensor " + output_tensor.shape);

            /*
            UnityEngine.Debug.Log("Output Tensor Shape: " + output_tensor.shape);
            for (int i = 0; i < Mathf.Min(10, output_tensor.length); i++)
            {
                UnityEngine.Debug.Log("Tensor Value [" + i + "]: " + output_tensor[i]);
            }
            */
            // convert output tensor to RenderTexture
            //output_tensor.ToRenderTexture(rTexture);
            //output_tensor.Dispose();


            /*
            // convert RenderTexture to Texture2D
            RenderTexture.active = rTexture;
            upsampled_texture.ReadPixels(emptyRect, 0, 0);
            upsampled_texture.Apply();
            RenderTexture.active = null;
            rTexture.Release();
            */

            /*
            // Print some pixel values for verification
            for (int y = 0; y < Mathf.Min(10, rTexture.height); y++)
            {
                for (int x = 0; x < Mathf.Min(10, rTexture.width); x++)
                {
                    UnityEngine.Color pixelColor = upsampled_texture.GetPixel(x, y);
                    UnityEngine.Debug.Log("Pixel Color [" + x + ", " + y + "]: " + pixelColor);
                }
            }
            */
            //TextureConverter.RenderToTexture(texture, tensor)



            //upsampled_texture = texture;




            // convert output tensor to texture -- TOO SLOW
            /*
            for (int y = 0; y < new_height; y++)
            {
                for (int x = 0; x < new_width; x++)
                {
                    float r = output_tensor[0, y, x, 0];
                    float g = output_tensor[0, y, x, 1];
                    float b = output_tensor[0, y, x, 2];
                    UnityEngine.Color color = new UnityEngine.Color(r, g, b, 1.0f);
                    upsampled_texture.SetPixel(x, new_height - 1 - y, color); 
                }
            }
            */


            //input_tensor.Dispose();
            //output_tensor.Dispose();
            //upsampled_texture.Apply();

            m_material.mainTexture = texture;//rTexture;

            //RenderTexture.active = null;
            
            // Assign the texture to the UI Image component
            //m_displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            TextureUpdated = false;
        }
    }

    private void ImgCallback(RosImage msgIn)
    {
        //UnityEngine.Debug.Log("callback");
        
        if (TextureUpdated)
            return;
        byte[] jpegData = msgIn.data;
        texture.LoadImage(jpegData);
        //print("loaded image " + texture.Size());

        
        // Optional: Manually adjust color channels if necessary
        
        UnityEngine.Color[] pixels = texture.GetPixels();
        
        for (int i = 0; i < pixels.Length; i++)
        {
            UnityEngine.Color pixel = pixels[i];
            pixels[i] = new UnityEngine.Color(pixel.b, pixel.g, pixel.r); // Swap R and B channels
        }
        
        texture.SetPixels(pixels);
        texture.Apply();

        //m_material.mainTexture = texture;


        // Set flag to update texture in the main thread
        TextureUpdated = true;
    }

    //public void OnDestroy()
    //{
        //Debug.Log("Destroy");
        //m_Worker.Dispose();
        //input_tensor.Dispose();
        //output_tensor.Dispose();
    //}
}
