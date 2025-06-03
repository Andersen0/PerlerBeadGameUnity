using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        GameObject[] beads = GameObject.FindGameObjectsWithTag("PerlerTag");

        if (beads == null || beads.Length == 0)
        {
            Debug.LogWarning("⚠️ No beads found in the scene with tag 'PerlerTag'. GLB export aborted.");
            return;
        }

        string filePath = EditorUtility.SaveFilePanel("Save GLB File", "", "AllBeads", "glb");
        if (string.IsNullOrEmpty(filePath)) return;
        if (Path.GetExtension(filePath).ToLower() != ".glb") filePath += ".glb";

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
            Debug.LogError("❌ GLB export failed: " + ex.Message);
        }
#else
        Debug.LogWarning("GLB export only works in the Unity Editor.");
#endif
    }
}
