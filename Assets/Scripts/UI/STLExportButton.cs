using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class STLExportButton : MonoBehaviour
{
    public Button exportButton;

    void Start()
    {
        if (exportButton != null)
            exportButton.onClick.AddListener(ExportBeadToSTL);
    }

    void ExportBeadToSTL()
    {
#if UNITY_EDITOR
        GameObject[] beads = GameObject.FindGameObjectsWithTag("PerlerTag");  // assign "Bead" tag to all bead objects

        if (beads == null || beads.Length == 0)
        {
            Debug.LogError("No beads found with tag 'PerlerTag'.");
            return;
        }

        string filePath = EditorUtility.SaveFilePanel("Save STL File", "", "AllBeads", "stl");
        if (string.IsNullOrEmpty(filePath)) return;
        if (Path.GetExtension(filePath).ToLower() != ".stl")
            filePath += ".stl";

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.ASCII))
            {
                writer.WriteLine("solid UnityExport");

                foreach (var bead in beads)
                {
                    MeshFilter mf = bead.GetComponent<MeshFilter>();
                    if (mf == null || mf.sharedMesh == null) continue;

                    Mesh mesh = mf.sharedMesh;
                    Vector3[] vertices = mesh.vertices;
                    int[] triangles = mesh.triangles;

                    for (int i = 0; i < triangles.Length; i += 3)
                    {
                        Vector3 v0 = bead.transform.TransformPoint(vertices[triangles[i]]);
                        Vector3 v1 = bead.transform.TransformPoint(vertices[triangles[i + 1]]);
                        Vector3 v2 = bead.transform.TransformPoint(vertices[triangles[i + 2]]);
                        Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

                        writer.WriteLine($"  facet normal {normal.x.ToString(CultureInfo.InvariantCulture)} {normal.y.ToString(CultureInfo.InvariantCulture)} {normal.z.ToString(CultureInfo.InvariantCulture)}");
                        writer.WriteLine("    outer loop");
                        writer.WriteLine($"      vertex {v0.x.ToString(CultureInfo.InvariantCulture)} {v0.y.ToString(CultureInfo.InvariantCulture)} {v0.z.ToString(CultureInfo.InvariantCulture)}");
                        writer.WriteLine($"      vertex {v1.x.ToString(CultureInfo.InvariantCulture)} {v1.y.ToString(CultureInfo.InvariantCulture)} {v1.z.ToString(CultureInfo.InvariantCulture)}");
                        writer.WriteLine($"      vertex {v2.x.ToString(CultureInfo.InvariantCulture)} {v2.y.ToString(CultureInfo.InvariantCulture)} {v2.z.ToString(CultureInfo.InvariantCulture)}");
                        writer.WriteLine("    endloop");
                        writer.WriteLine("  endfacet");
                    }
                }

                writer.WriteLine("endsolid UnityExport");
            }

            Debug.Log("✅ STL exported to: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ STL export failed: " + ex.Message);
        }
#else
        Debug.LogWarning("STL export only works in the Unity Editor.");
#endif
    }
}
