using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gray : MonoBehaviour
{
    public Texture InputTex;
    public ComputeShader ComputeShader;
    public RawImage RawImage;

    // Start is called before the first frame update
    void Start()
    {
        RenderTexture t = new RenderTexture(InputTex.width, InputTex.height, 24);
        t.enableRandomWrite = true;
        t.Create();
        RawImage.texture = t;
        RawImage.SetNativeSize();

        int kernal = ComputeShader.FindKernel("Gray");
        ComputeShader.SetTexture(kernal, "inputTexture", InputTex);
        ComputeShader.SetTexture(kernal, "outputTexture", t);
        ComputeShader.Dispatch(kernal, InputTex.width / 8, InputTex.height / 8, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
