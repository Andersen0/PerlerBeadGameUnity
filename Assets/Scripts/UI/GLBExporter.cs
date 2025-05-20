#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using UnityEngine;
using UnityGLTF;

public class GLBExporter : MonoBehaviour
{
    public void ExportBeadToGLB()
    {
        GameObject[] beads = GameObject.FindGameObjectsWithTag("PerlerTag");  // assign "Bead" tag to all bead objects

        if (beads == null || beads.Length == 0)
        {
            Debug.LogError("No beads found in the scene with tag 'Bead'.");
            return;
        }

#if UNITY_EDITOR
        string folderPath = EditorUtility.SaveFolderPanel("Choose Export Folder", "", "");
        if (string.IsNullOrEmpty(folderPath)) return;

        // Use a fixed filename or prompt for name, not beads.ToString()
        string filePath = Path.Combine(folderPath, "AllBeads.glb");

        // Create a Transform[] from the GameObject[]
        Transform[] beadTransforms = new Transform[beads.Length];
        for (int i = 0; i < beads.Length; i++)
        {
            beadTransforms[i] = beads[i].transform;
        }

        var context = new ExportContext();

        // Pass the transforms and the context to the constructor
        var exporter = new GLTFSceneExporter(beadTransforms, context);

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
