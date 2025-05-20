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
        GameObject[] beads = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                                   .Where(go => go.name.StartsWith("lowVertixPerler"))
                                   .ToArray();

        if (beads.Length == 0)
        {
            Debug.LogError("No beads found in the scene with name starting 'lowVertixPerler'.");
            return;
        }

        string folderPath = EditorUtility.SaveFolderPanel("Choose Export Folder", "", "");
        if (string.IsNullOrEmpty(folderPath)) return;

        string filePath = Path.Combine(folderPath, "AllBeads.glb");

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
