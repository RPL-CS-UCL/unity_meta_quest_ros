using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignRenderTextures : MonoBehaviour
{
    public RenderTexture EERenderTexture;
    public RenderTexture BaseRenderTexture;

    public RawImage EEImage;
    public RawImage BaseImage;

    // Start is called before the first frame update
    void Start()
    {
        if (EEImage != null && EERenderTexture != null)
        {
            EEImage.texture = EERenderTexture;
            Debug.Log("EE RenderTexture assigned to raw image");
        }
        else
        {
            Debug.Log("EE no rendertexture or raw image");
        }
        if (BaseImage != null && BaseRenderTexture != null)
        {
            BaseImage.texture = BaseRenderTexture;
            Debug.Log("Base RenderTexture assigned to raw image");
        }
        else
        {
            Debug.Log("Base no rendertexture or raw image");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
