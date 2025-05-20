using UnityEngine;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityGLTF;

public class GLTFExportButton : MonoBehaviour
{
    public Button exportButton;

    void Start()
    {
        if (exportButton != null)
            exportButton.onClick.AddListener(ExportBeadToGLTF);
    }

    void ExportBeadToGLTF()
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

        // ✅ Use the modern, recommended constructor
        var options = new ExportContext
        {
            //ExportInactivePrimitives = false,
            //ShouldExportExtensions = false,
            //ExportOnlySelected = false
        };

        var exporter = new GLTFSceneExporter(new[] { bead.transform }, options);

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
