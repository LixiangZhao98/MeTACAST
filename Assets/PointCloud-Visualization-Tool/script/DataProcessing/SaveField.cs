using System;
using System.IO;
using UnityEngine;

using System.Diagnostics;
using UnityEditor;
public class SaveField : MonoBehaviour
{
    static public void FloatsToBytes(string filename, float[] vs)
    {
        try
        {
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            byte[] byteArray = new byte[sizeof(float) * vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                byte[] byte_ = BitConverter.GetBytes(vs[i]);
                byte_.CopyTo(byteArray, sizeof(float) * i);
            }
            fs.Write(byteArray, 0, byteArray.Length);
           UnityEngine. Debug.Log(vs.Length + "floats are written into " + filename + " successfully.");
        }
        catch
        {
            UnityEngine.Debug.LogError("Write fail");
        }




    }

    static public void SaveAsTexture3D(string filename, Texture3D texture3D)
    {

        Texture3D textureCopy = new Texture3D(texture3D.width, texture3D.height, texture3D.depth, texture3D.format, texture3D.mipmapCount > 1);
        textureCopy.SetPixels(texture3D.GetPixels());
        textureCopy.Apply();

        AssetDatabase.CreateAsset(textureCopy, filename);
        AssetDatabase.SaveAssets();

        UnityEngine.Debug.Log("Export completed. Asset saved at: " + filename);
    }
    static public void SaveAsRaw(string filename, UInt32[] uis)
    {
        FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
        byte[] byteArray = new byte[sizeof(UInt32) * uis.Length];
        for (int i = 0; i < uis.Length; i++)
        {
            byte[] bytex = System.BitConverter.GetBytes(uis[i]);
            bytex.CopyTo(byteArray, sizeof(UInt32) * i);
        }
        fs.Write(byteArray, 0, byteArray.Length);
        UnityEngine.Debug.Log("Export completed. Raw saved at: " + filename);
    }
}
