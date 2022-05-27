using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class exportDragonPng : MonoBehaviour
{
    public RenderTexture rTex;
    private int i = 0;
    private int sqr = 2048;

    public void ExportAsPng() {
        Texture2D tex = new Texture2D(sqr, sqr, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        
        tex.ReadPixels( new Rect(0, 0, sqr,sqr), 0, 0);

        byte[] bytes = tex.EncodeToPNG();
     
        File.WriteAllBytes("C:/Projects/unity-projects/Town/DragonPng/dragon_" + i + ".png", bytes);
    }
}
