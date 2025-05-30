using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;  // <-- Add this

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityGLTF;

public class GLBExportButton : MonoBehaviour
{
    public Button exportButton;

    void Start()
    {
        if (exportButton != null)
            exportButton.onClick.AddListener(ExportBeadToGLB);
    }

    void ExportBeadToGLB()
    {
#if UNITY_EDITOR
        // Find all beads in the scene whose names start with "lowVertixPerler"
        GameObject[] beads = GameObject.FindGameObjectsWithTag("PerlerTag");  // assign "Bead" tag to all bead objects

        if (beads == null || beads.Length == 0)
        {
            Debug.LogError("No beads found in the scene with name starting 'lowVertixPerler'.");
            return;
        }

        // Allows the user to choose folder path and file name
        string filePath = EditorUtility.SaveFilePanel("Save GLB File", "", "AllBeads", "glb");
        if (string.IsNullOrEmpty(filePath)) return;
        if (Path.GetExtension(filePath).ToLower() != ".glb")
            filePath += ".glb";

        // Collect all bead transforms
        Transform[] beadTransforms = new Transform[beads.Length];
        for (int i = 0; i < beads.Length; i++)
            beadTransforms[i] = beads[i].transform;

        var context = new ExportContext();
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
