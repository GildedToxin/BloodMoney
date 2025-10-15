#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class TextureArrayCreator : MonoBehaviour
{
    [Header("Source Textures (all must be same size & format)")]
    public Texture2D[] sourceTextures;

    [Header("Settings")]
    public TextureFormat textureFormat = TextureFormat.RGBA32;
    public bool mipmaps = false;
    public string savePath = "Assets/MyTextureArray.asset"; // Change as needed

    [ContextMenu("Create Texture2D Array")]
    public void CreateTextureArray()
    {
        if (sourceTextures == null || sourceTextures.Length == 0)
        {
            Debug.LogError("No source textures assigned!");
            return;
        }

        // Ensure all textures match
        int width = sourceTextures[0].width;
        int height = sourceTextures[0].height;
        for (int i = 0; i < sourceTextures.Length; i++)
        {
            if (sourceTextures[i].width != width || sourceTextures[i].height != height)
            {
                Debug.LogError("All textures must have the same width & height!");
                return;
            }
        }

        // Create the Texture2DArray
        Texture2DArray texArray = new Texture2DArray(width, height, sourceTextures.Length, textureFormat, mipmaps);

        // Copy textures into the array
        for (int i = 0; i < sourceTextures.Length; i++)
        {
            Graphics.CopyTexture(sourceTextures[i], 0, 0, texArray, i, 0);
        }

        texArray.Apply();

        // Save as asset
        AssetDatabase.CreateAsset(texArray, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Texture2DArray created and saved at: {savePath}");
    }
}
#endif
