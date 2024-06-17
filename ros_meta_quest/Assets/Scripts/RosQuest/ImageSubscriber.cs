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

public class ImageSubscriber: MonoBehaviour
{
    [SerializeField] private Image m_displayImage; // Reference to a UI Image component to display the texture

    //public UnityEngine.UI.RawImage rawImage;

    public NNModel modelAsset;
    private Model m_RuntimeModel;


    static int width = 640; //1920
    static int height = 480; //1080

    static int new_width = width * 3;
    static int new_height = height * 3;

    // Decode the ROS image message to a Texture2D
    Texture2D texture;

    Texture2D upsampled_texture;

    public Unity.Barracuda.IWorker m_Worker;

    // Populate the Texture2D with the raw byte data
    //Color32[] pixelData;

    public bool TextureUpdated = true;

    RenderTexture rTexture;

    Rect emptyRect = new Rect(0, 0, width*3, height*3);

    Tensor input_tensor;
    Tensor output_tensor;

    //UnityEngine.Color[] pixels = new UnityEngine.Color[new_width * new_height];

    // Use this for initialization
    void Start()
    {
        Debug.Log("start");
        m_RuntimeModel = Unity.Barracuda.ModelLoader.Load(modelAsset, false, false);
        m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel); //ComputePrecompiled for GPU, CSharpBurst for CPU

        texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        upsampled_texture = new Texture2D(width*3, height*3, TextureFormat.RGB24, false); // multiply height and width by model upscaling factor 

        ROSConnection.GetOrCreateInstance().Subscribe<RosImage>("/camera/image_compressed2", ImgCallback);

        // Populate the Texture2D with the raw byte data
        //pixelData = new Color32[width * height];

        //for (int i = 0; i < pixelData.Length; i++)
        //{
        //int index = i * 3;
        //pixelData[i] = new Color32(255,255,255,255);
        //}

        rTexture = new RenderTexture(width * 3, height * 3, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("Update");
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

            /* -- UPSIDE DOWN
            // Get pixel data from texture
            UnityEngine.Color[] pixels = texture.GetPixels();

            // Convert pixel data to float array
            float[] floatArray = new float[pixels.Length * 3];
            for (int i = 0; i < pixels.Length; i++)
            {
                floatArray[i * 3] = pixels[i].r;
                floatArray[i * 3 + 1] = pixels[i].g;
                floatArray[i * 3 + 2] = pixels[i].b;
            }

            // Create tensor from float array
            input_tensor = new Tensor(1, texture.height, texture.width, 3, floatArray);
            */

            
            input_tensor = new Tensor(texture);

            // run the model on the tensor
            m_Worker.Execute(input_tensor); //FIX
            input_tensor.Dispose();
            output_tensor = m_Worker.PeekOutput();
            
            // convert output tensor to RenderTexture
            output_tensor.ToRenderTexture(rTexture);
            output_tensor.Dispose();

            // convert RenderTexture to Texture2D
            RenderTexture.active = rTexture;
            upsampled_texture.ReadPixels(emptyRect, 0, 0);
            
            //upsampled_texture = texture;




            // convert output tensor to texture -- UPSIDE DOWN
            /*
            for (int y = 0; y < height*3; y++)
            {
                for (int x = 0; x < width*3; x++)
                {
                    //int index = (y * (width*3) + x) * 3;
                    float r = output_tensor[0, y, x, 0];
                    float g = output_tensor[0, y, x, 1];
                    float b = output_tensor[0, y, x, 2];

                    upsampled_texture.SetPixel(x, y, new UnityEngine.Color(r, g, b), 0); 
                }
            }
            */


            /* -- UPSIDE DOWN
            // Extract data from tensor
            float[] data = output_tensor.ToReadOnlyArray();
            for (int y = 0; y < output_tensor.height; y++)
            {
                for (int x = 0; x < output_tensor.width; x++)
                {
                    int i = (y * output_tensor.width + x) * 3;
                    pixels[y * output_tensor.width + x] = new UnityEngine.Color(
                        data[i],
                        data[i + 1],
                        data[i + 2]
                    );
                }
            }
            // Set pixel data and apply changes
            upsampled_texture.SetPixels(pixels);
            */

            //input_tensor.Dispose();
            //output_tensor.Dispose();
            upsampled_texture.Apply();

            // Assign the texture to the UI Image component
            m_displayImage.sprite = Sprite.Create(upsampled_texture, new Rect(0, 0, upsampled_texture.width, upsampled_texture.height), new Vector2(0.5f, 0.5f));
            TextureUpdated = false;
        }
    }

    private void ImgCallback(RosImage msgIn)
    {
        Debug.Log("Callback");

        texture.LoadImage(msgIn.data);

        // Set flag to update texture in the main thread
        TextureUpdated = true;
    }

    public void OnDestroy()
    {
        Debug.Log("Destroy");
        m_Worker.Dispose();
        //input_tensor.Dispose();
        //output_tensor.Dispose();
    }
}
