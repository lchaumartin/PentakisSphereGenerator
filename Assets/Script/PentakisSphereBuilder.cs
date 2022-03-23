using System.Collections.Generic;
using UnityEngine;

public class PentakisSphereBuilder
{
    private static bool contains(Vector3Int v, int value)
    {
        return value == v.x || value == v.y || value == v.z;
    }

    public static Mesh PentakisSphere(int resolution, float radius)
    {
        float phi = (1f + Mathf.Sqrt(5f)) / 2f;
        List<Vector3> Vertices = new List<Vector3>();
        List<Vector3Int> Faces = new List<Vector3Int>();
        
        float ratio = radius / phi;
        Vertices.Add(new Vector3(-1f, phi, 0f).normalized * radius);
        Vertices.Add(new Vector3(1f, phi, 0f).normalized * radius);
        Vertices.Add(new Vector3(-1f, -phi, 0f).normalized * radius);
        Vertices.Add(new Vector3(1f, -phi, 0f).normalized * radius);

        Vertices.Add(new Vector3(0f, -1f, phi).normalized * radius);
        Vertices.Add(new Vector3(0f, 1f, phi).normalized * radius);
        Vertices.Add(new Vector3(0f, -1f, -phi).normalized * radius);
        Vertices.Add(new Vector3(0f, 1f, -phi).normalized * radius);

        Vertices.Add(new Vector3(phi, 0f, -1f).normalized * radius);
        Vertices.Add(new Vector3(phi, 0f, 1f).normalized * radius);
        Vertices.Add(new Vector3(-phi, 0f, -1f).normalized * radius);
        Vertices.Add(new Vector3(-phi, 0f, 1f).normalized * radius);

        Faces.Add(new Vector3Int(0, 11, 5));
        Faces.Add(new Vector3Int(0, 5, 1));
        Faces.Add(new Vector3Int(0, 1, 7));
        Faces.Add(new Vector3Int(0, 7, 10));
        Faces.Add(new Vector3Int(0, 10, 11));

        Faces.Add(new Vector3Int(1, 5, 9));
        Faces.Add(new Vector3Int(7, 1, 8));
        Faces.Add(new Vector3Int(9, 8, 1));

        Faces.Add(new Vector3Int(2, 4, 11));
        Faces.Add(new Vector3Int(6, 2, 10));
        Faces.Add(new Vector3Int(11, 10, 2));

        Faces.Add(new Vector3Int(3, 9, 4));
        Faces.Add(new Vector3Int(3, 4, 2));
        Faces.Add(new Vector3Int(3, 2, 6));
        Faces.Add(new Vector3Int(3, 6, 8));
        Faces.Add(new Vector3Int(3, 8, 9));

        Faces.Add(new Vector3Int(4, 9, 5));
        Faces.Add(new Vector3Int(5, 11, 4));

        Faces.Add(new Vector3Int(8, 6, 7));
        Faces.Add(new Vector3Int(10, 7, 6));


        for (int subdivision = 0; subdivision < resolution; subdivision++)
        {
            List<Vector3> AdditionalVertices = new List<Vector3>();
            List<Vector3Int> NewFaces = new List<Vector3Int>();

            foreach (Vector3Int f in Faces)
            {
                AdditionalVertices.Add((Vertices[f.x] + Vertices[f.y] + Vertices[f.z]).normalized * radius);
            }
            for (int i = 0; i < Vertices.Count; i++)
            {
                List<int> adjIndexes = new List<int>();
                int faceIndex = 0;
                foreach (Vector3Int adj in Faces)
                {
                    if (contains(adj, i))
                    {
                        adjIndexes.Add(faceIndex);
                    }
                    faceIndex++;
                }
                for (int j = 0; j < adjIndexes.Count; j++)
                {
                    float edgeSize = Mathf.Sqrt(2f) * Vector3.Distance(Vertices[i], AdditionalVertices[adjIndexes[j]]);
                    for (int k = 0; k < adjIndexes.Count; k++)
                    {
                        if (j != k && Vector3.Distance(AdditionalVertices[adjIndexes[j]], AdditionalVertices[adjIndexes[k]]) <= edgeSize && Vector3.Dot(Vector3.Cross(AdditionalVertices[adjIndexes[j]] - Vertices[i], AdditionalVertices[adjIndexes[k]] - Vertices[i]), Vertices[i]) > 0f)
                            NewFaces.Add(new Vector3Int(i, Vertices.Count + adjIndexes[j], Vertices.Count + adjIndexes[k]));
                    }
                }
            }
            Faces = NewFaces;
            Vertices.AddRange(AdditionalVertices);
        }
        Mesh res = new Mesh();
        if (Vertices.Count > 65535)
            res.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        res.SetVertices(Vertices);
        List<int> tri = new List<int>();
        foreach (Vector3Int v in Faces)
        {
            tri.Add(v.x);
            tri.Add(v.y);
            tri.Add(v.z);
        }
        res.SetTriangles(tri, 0);
        res.RecalculateNormals();
        res.name = "PentakisSphere";
        return res;
    }
}
