using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class PentakisSphere : MonoBehaviour
{

    [Range(0.01f, 5f)]
    public float radius;
    [Range(1, 10)]
    public int resolution;

    public void Generate()
    {
        GetComponent<MeshFilter>().sharedMesh = PentakisSphereBuilder.PentakisSphere(resolution - 1, radius);
    }
}
