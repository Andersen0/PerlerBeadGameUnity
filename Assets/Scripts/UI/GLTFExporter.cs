#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using UnityEngine;
using UnityGLTF;

public class GLTFExporter : MonoBehaviour
{
    public void ExportBeadToGLTF()
    {
        GameObject bead = GameObject.Find("lowVertixPerler(Clone)");
        if (bead == null)
        {
            Debug.LogError("Could not find lowVertixPerler(Clone) in the scene.");
            return;
        }

#if UNITY_EDITOR
        string folderPath = EditorUtility.SaveFolderPanel("Choose Export Folder", "", "");
        if (string.IsNullOrEmpty(folderPath)) return;

        string filePath = Path.Combine(folderPath, bead.name + ".glb");

        // ✅ Fully modern, no obsolete usage
        var context = new ExportContext();  // Replace or customize if needed
        var exporter = new GLTFSceneExporter(new[] { bead.transform }, context);

        try
        {
            exporter.SaveGLB(filePath, "");
            Debug.Log("✅ GLB exported to: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Export failed: " + ex.Message);
        }
#else
        Debug.LogWarning("GLTF export only works in the Unity Editor.");
#endif
    }
}
