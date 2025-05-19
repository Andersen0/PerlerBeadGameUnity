using UnityEngine;
using Siccity.GLTFUtility;

public class GLTFExporter : MonoBehaviour
{
    public GameObject objectToExport;

    public void ExportToGLB()
    {
        if (objectToExport == null)
        {
            Debug.LogError("No object assigned to export.");
            return;
        }

#if UNITY_EDITOR
        // This method opens a save dialog automatically in the Editor
        Exporter.ExportGLB(objectToExport);
#else
        Debug.LogError("GLB export only works in the Unity Editor.");
#endif
    }
}
