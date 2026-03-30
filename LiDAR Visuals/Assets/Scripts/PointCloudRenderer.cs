using UnityEngine;
using System.Collections.Generic;

public class PointCloudRenderer : MonoBehaviour {
    [Header("Appearance")]
    public int maxPoints = 200000;
    public float pointSize = 0.02f;
    public Gradient colorByDistance;    // set in Inspector

    [Header("Filter")]
    public float minDist = 0.1f;
    public float maxDist = 8f;

    Mesh mesh;
    List<Vector3> vertices = new List<Vector3>();
    List<Color> colors = new List<Color>();
    MeshFilter mf;
    MeshRenderer mr;

    void Awake() {
        mf = gameObject.AddComponent<MeshFilter>();
        mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Particles/Standard Unlit"));

        mesh = new Mesh { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        mf.mesh = mesh;

        // Default distance gradient: blue→green→red
        colorByDistance = new Gradient();
        colorByDistance.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.blue,  0f),
                new GradientColorKey(Color.green, 0.5f),
                new GradientColorKey(Color.red,   1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
    }

    public void AddPoint(PointData pt) {
        if (pt.dist < minDist || pt.dist > maxDist) return;

        Vector3 pos = new Vector3(pt.x, pt.y, pt.z);
        float t = Mathf.InverseLerp(minDist, maxDist, pt.dist);

        vertices.Add(pos);
        colors.Add(colorByDistance.Evaluate(t));

        if (vertices.Count > maxPoints) {
            vertices.RemoveAt(0);
            colors.RemoveAt(0);
        }

        RebuildMesh();
    }

    void RebuildMesh() {
        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetColors(colors);

        int[] indices = new int[vertices.Count];
        for (int i = 0; i < indices.Length; i++) indices[i] = i;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }

    public void ClearPoints() {
        vertices.Clear();
        colors.Clear();
        RebuildMesh();
    }
}