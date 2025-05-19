using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Siccity.GLTFUtility;

public class GLTFExportButton : MonoBehaviour
{
    public Button exportButton;

    void Start()
    {
        if (exportButton != null)
            exportButton.onClick.AddListener(ExportBeadToGLB);
    }

    void ExportBeadToGLB()
    {
        GameObject bead = GameObject.Find("lowVertixPerler(Clone)");
        if (bead == null)
        {
            Debug.LogError("Could not find lowVertixPerler(Clone) in the scene.");
            return;
        }

#if UNITY_EDITOR
        // Manually open the Save File dialog and use the correct export method
        string path = EditorUtility.SaveFilePanel("Export Bead as GLB", "", bead.name + ".glb", "glb");
        if (!string.IsNullOrEmpty(path))
        {
            Exporter.ExportGLB(bead, path);
            Debug.Log("✅ Exported to: " + path);
        }
#else
        Debug.LogWarning("GLB export only works in the Unity Editor.");
#endif
    }
}
