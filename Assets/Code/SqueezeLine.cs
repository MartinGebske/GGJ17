using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is only used for the visuals.
 * When the bottle is squeezed, this object will be created
 * and points are added, updating this mesh.
 */

public class SqueezeLine : MonoBehaviour
{
    [Header("Config")]
    public Material Material;
    public float Width = 0.25f;
    public float Height = 0.2f;

    private List<Vector3> Points = new List<Vector3>();

    private MeshBuilder meshBuilder;
    private Mesh currentMesh;

    public void AddNewPoint(Vector3 Point)
    {
        Points.Add(Point);
        UpdateMesh();
    }

    private void Start()
    {
        UpdateMesh();
    }

    public float GetTotalDeviation(List<Vector3> FromPoints)
    {
        float finalDeviation = 0.0f;
        // for every point in FromPoints get distance to nearest point in Points, add all those distances

        foreach(Vector3 CheckPoint in FromPoints)
        {
            float smallestDistance = float.MaxValue;

            for (int i = 0; i < Points.Count; i++)
            {
                float tempDistance = Vector3.Distance(
                    new Vector3(CheckPoint.x, 0.0f, CheckPoint.z), new Vector3(Points[i].x, 0.0f, Points[i].z));
                if (tempDistance < smallestDistance)
                    smallestDistance = tempDistance;
            }

            finalDeviation += Mathf.Pow(smallestDistance, 2.0f);
        }

        // in order for FromPoints.Count == 0 not be 0 Deviation, have this 100 std deviation
        return Mathf.Clamp(100.0f - FromPoints.Count, 0.0f, 100.0f) + finalDeviation;
    }

    private void UpdateMesh()
    {
        if (currentMesh != null)
            Destroy(currentMesh);

        MeshBuilder meshBuilder = new MeshBuilder();

        // Add the vertices per point
        for (int i = 0; i < Points.Count - 1; i++)
        {
            Vector3 forwardDir = Points[i + 1] - Points[i] ;

            Vector3 rightDir = Vector3.Cross(forwardDir.normalized, Vector3.up.normalized);
            Vector3 leftDir = -rightDir;

            Vector3 forward = Points[i] + forwardDir;
            Vector3 left = Points[i] + leftDir * Width;
            Vector3 right = Points[i] + rightDir * Width;

            // Root Location
            meshBuilder.Vertices.Add(Points[i] + Vector3.up * Height);
            meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
            meshBuilder.Normals.Add(Vector3.up);

            // Left from Root
            meshBuilder.Vertices.Add(left);
            meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
            meshBuilder.Normals.Add(Vector3.up);

            // Forward from Root
            meshBuilder.Vertices.Add(forward + Vector3.up * Height);
            meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
            meshBuilder.Normals.Add(Vector3.up);

            // Right from Root
            meshBuilder.Vertices.Add(right);
            meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
            meshBuilder.Normals.Add(Vector3.up);

            int vIndex = i * 4;
            if (vIndex > 0)
            {
                // 0, 1' (-3), 1
                meshBuilder.AddTriangle(vIndex, vIndex + 1, vIndex - 3);

                // 0, 3, 3' (-1)
                meshBuilder.AddTriangle(vIndex, vIndex - 1, vIndex + 3);
            }
            meshBuilder.AddTriangle(vIndex, vIndex + 2, vIndex + 1);
            meshBuilder.AddTriangle(vIndex, vIndex + 3, vIndex + 2);
        }

        Mesh mesh = meshBuilder.CreateMesh();

        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter != null)
        {
            filter.sharedMesh = mesh;
        }
    }
}
