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

    // Decode the ROS image message to a Texture2D
    Texture2D texture;

    Texture2D upsampled_texture;

    public Unity.Barracuda.IWorker m_Worker;

    // Populate the Texture2D with the raw byte data
    Color32[] pixelData;

    private bool TextureUpdated = true;

    // Use this for initialization
    void Start()
    {
        m_RuntimeModel = Unity.Barracuda.ModelLoader.Load(modelAsset, false, false);
        m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel); //change type to ComputePrecompiled for GPU

        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        upsampled_texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Populate the Texture2D with the raw byte data
        pixelData = new Color32[width * height];
        print("subscribing");
        ROSConnection.GetOrCreateInstance().Subscribe<RosImage>("/camera/image_compressed2", ImgCallback);
        print("subscribed");
        for (int i = 0; i < pixelData.Length; i++)
        {
            int index = i * 3;
            pixelData[i] = new Color32(255,255,255,255);
        }
        print("end of start function");
    }

    // Update is called once per frame
    private void Update()
    {
        if (TextureUpdated)
        {
            // Assign the texture to the UI Image component
            m_displayImage.sprite = Sprite.Create(upsampled_texture, new Rect(0, 0, upsampled_texture.width, upsampled_texture.height), new Vector2(0.5f, 0.5f));
            TextureUpdated = false;
        }
        print("end of update function");
    }

    private void ImgCallback(RosImage msgIn)
    {
        print("start of callback function");

        /*

        texture.LoadImage(msgIn.data);

        // convert texture to tensor
        float[] floatValues = new float[texture.width * texture.height * 3];

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                UnityEngine.Color pixel = texture.GetPixel(x, y);
                int index = (y * texture.width + x) * 3;
                floatValues[index] = pixel.r; // Red channel
                floatValues[index + 1] = pixel.g; // Green channel
                floatValues[index + 2] = pixel.b; // Blue channel
            }
        }

        Tensor input_tensor = new Tensor(1, texture.height, texture.width, 3, floatValues);

        // run the model on the tensor
        m_Worker.Execute(input_tensor);
        Tensor output_tensor = m_Worker.PeekOutput();

        // convert output tensor to texture
        int height = output_tensor.shape[1];
        int width = output_tensor.shape[2];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //int index = (y * width + x) * 3;
                float r = output_tensor[0, y, x, 0];
                float g = output_tensor[0, y, x, 1];
                float b = output_tensor[0, y, x, 2];

                upsampled_texture.SetPixel(x, y, new UnityEngine.Color(r, g, b), 255);
            }
        }

        input_tensor.Dispose();
        output_tensor.Dispose();
        upsampled_texture.Apply();

        // Set flag to update texture in the main thread
        TextureUpdated = true;
        */
    }

    public void OnDestroy()
    {
        m_Worker.Dispose();
        print("worker destroyed");
    }
}
