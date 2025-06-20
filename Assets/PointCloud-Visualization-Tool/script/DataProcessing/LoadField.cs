using UnityEditor;
using UnityEngine;

public class LoadField : MonoBehaviour
{
    public static Texture3D LoadTexture3D(string filename)
    {
        var loadedTexture3D = AssetDatabase.LoadAssetAtPath<Texture3D>(filename);
        if (loadedTexture3D == null)
        {
            Debug.LogError("Failed to load Texture3D asset at: " + filename);
            return null;
        }

        return loadedTexture3D;
    }
}