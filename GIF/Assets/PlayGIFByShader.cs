using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayGIFByShader : MonoBehaviour
{
    private System.Drawing.Image image;

    private List<Texture2D> tex2DList = new List<Texture2D>();

    // Start is called before the first frame update
    void Start()
    {
        image = System.Drawing.Image.FromFile(Application.dataPath + "/Test.gif");

        Bytes2Texture2D(GetBytesFromImage(image));

        var texArray = new Texture2DArray(tex2DList[0].width, tex2DList[0].height, tex2DList.Count, TextureFormat.ARGB32, false);

        for (int i = 0; i < tex2DList.Count; i++)
        {
            Graphics.CopyTexture(tex2DList[i], 0, texArray, i);
        }

        var material = gameObject.GetComponent<Image>().material;

        material.SetTexture("_TextureArray", texArray);
        material.SetInt("_TexturesCount", Mathf.Max(1, texArray.depth));
    }

    private List<byte[]> GetBytesFromImage(System.Drawing.Image image)
    {
        List<byte[]> tex = new List<byte[]>();
        if (image != null)
        {
            FrameDimension frame = new FrameDimension(image.FrameDimensionsList[0]);
            //获取维度帧数
            int frameCount = image.GetFrameCount(frame);
            for (int i = 0; i < frameCount; ++i)
            {
                image.SelectActiveFrame(frame, i);
                using (MemoryStream stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Png);
                    byte[] data = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(data, 0, Convert.ToInt32(stream.Length));
                    tex.Add(data);
                }
            }
        }
        return tex;
    }

    private void Bytes2Texture2D(List<byte[]> bytes)
    {
        foreach (var t in bytes)
        {
            Texture2D frameTexture2D = new Texture2D(image.Width, image.Height, TextureFormat.ARGB32, false);
            frameTexture2D.LoadImage(t);
            tex2DList.Add(frameTexture2D);
        }
    }
}
