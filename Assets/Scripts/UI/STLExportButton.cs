using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class STLExportButton : MonoBehaviour
{
    public Button exportButton;
    public MeshFilter baseMeshFilter; // Assign this in the Inspector (e.g., lowVertixPerler prefab)

    void Start()
    {
        if (exportButton != null)
            exportButton.onClick.AddListener(ExportSTLUsingBaseMesh);
    }

    void ExportSTLUsingBaseMesh()
    {
#if UNITY_EDITOR
        if (baseMeshFilter == null || baseMeshFilter.sharedMesh == null)
        {
            Debug.LogError("❌ Base mesh filter is not assigned or empty.");
            return;
        }

        GameObject[] beads = GameObject.FindGameObjectsWithTag("PerlerTag");
        if (beads.Length == 0)
        {
            Debug.LogError("❌ No beads found with tag 'PerlerTag'.");
            return;
        }

        string filePath = EditorUtility.SaveFilePanel("Save STL File", "", "PerlerExport", "stl");
        if (string.IsNullOrEmpty(filePath)) return;
        if (Path.GetExtension(filePath).ToLower() != ".stl") filePath += ".stl";

        try
        {
            Mesh mesh = baseMeshFilter.sharedMesh;
            Vector3[] baseVertices = mesh.vertices;
            int[] baseTriangles = mesh.triangles;

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.ASCII))
            {
                writer.WriteLine("solid UnityExport");

                foreach (var bead in beads)
                {
                    Matrix4x4 matrix = bead.transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Euler(90f, 0f, 0f));

                    for (int i = 0; i < baseTriangles.Length; i += 3)
                    {
                        Vector3 v0 = TransformVertex(baseVertices[baseTriangles[i]], matrix);
                        Vector3 v1 = TransformVertex(baseVertices[baseTriangles[i + 1]], matrix);
                        Vector3 v2 = TransformVertex(baseVertices[baseTriangles[i + 2]], matrix);

                        Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

                        writer.WriteLine($"  facet normal {FormatVec(normal)}");
                        writer.WriteLine("    outer loop");
                        writer.WriteLine($"      vertex {FormatVec(v0)}");
                        writer.WriteLine($"      vertex {FormatVec(v1)}");
                        writer.WriteLine($"      vertex {FormatVec(v2)}");
                        writer.WriteLine("    endloop");
                        writer.WriteLine("  endfacet");
                    }
                }


                writer.WriteLine("endsolid UnityExport");
                Debug.Log($"✅ STL exported with {beads.Length} instances to: {filePath}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ STL export failed: " + ex.Message);
        }
#else
        Debug.LogWarning("STL export only works in the Unity Editor.");
#endif
    }

    private static Vector3 TransformVertex(Vector3 local, Matrix4x4 matrix)
    {
        Vector3 world = matrix.MultiplyPoint3x4(local);

        // STL expects Z-up, Unity is Y-up. In adition we scale with *1000, taking it from m -> mm
        return new Vector3(world.x, world.z, -world.y)*1000;
    }

    private static string FormatVec(Vector3 v)
    {
        return $"{v.x.ToString("G", CultureInfo.InvariantCulture)} {v.y.ToString("G", CultureInfo.InvariantCulture)} {v.z.ToString("G", CultureInfo.InvariantCulture)}";
    }
}
