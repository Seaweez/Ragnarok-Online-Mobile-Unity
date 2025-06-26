using UnityEngine;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using System.IO;

public class ExportFBXWithTextures : MonoBehaviour
{
    [MenuItem("Tools/Export FBX With Textures")]
    static void ExportSelectedPrefab()
    {
        GameObject selected = Selection.activeGameObject;

        if (selected == null)
        {
            Debug.LogError("❌ No GameObject selected.");
            return;
        }

        string exportFolder = "Assets/ExportedFBX/";
        string textureFolder = exportFolder + "Textures/";
        string fbxFile = exportFolder + selected.name + ".fbx";

        // Ensure folders exist
        if (!Directory.Exists(exportFolder))
            Directory.CreateDirectory(exportFolder);
        if (!Directory.Exists(textureFolder))
            Directory.CreateDirectory(textureFolder);

        // Export FBX
        ModelExporter.ExportObject(fbxFile, selected);
        Debug.Log($"✅ FBX Exported: {fbxFile}");

        // Copy textures
        Renderer[] renderers = selected.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat == null) continue;

                Texture tex = mat.mainTexture;
                if (tex == null) continue;

                string texPath = AssetDatabase.GetAssetPath(tex);
                if (string.IsNullOrEmpty(texPath)) continue;

                string fileName = Path.GetFileName(texPath);
                string destPath = textureFolder + fileName;

                try
                {
                    File.Copy(texPath, destPath, true);
                    Debug.Log($"🖼️ Copied texture: {fileName}");
                }
                catch (IOException ex)
                {
                    Debug.LogError($"❌ Failed to copy texture: {fileName} — {ex.Message}");
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("🎉 Export complete! Check Assets/ExportedFBX/");
    }
}
