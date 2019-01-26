using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndulatingVertices : MonoBehaviour
{
    Mesh Mesh;
    Vector3[] OriginalVertices;
    Vector3[] Vertices;
    float UndulatingFreq = 5.0f;
    float UndulatingSpeed = 5.0f;

    private void Awake()
    {
        Mesh = GetComponent<MeshFilter>().mesh;
        OriginalVertices = new Vector3[Mesh.vertices.Length];
        Vertices = new Vector3[Mesh.vertices.Length];
        for ( int i = 0; i < OriginalVertices.Length; ++i )
        {
            OriginalVertices[i] = Mesh.vertices[i];
        }
    }

    void Update()
    {
        for (var i = 0; i < OriginalVertices.Length; ++i )
        {
            Vertices[i] = OriginalVertices[i] + Vector3.up * Mathf.Sin(OriginalVertices[i].z * UndulatingFreq + Time.time * UndulatingSpeed);
        }

        Mesh.vertices = Vertices;
        Mesh.RecalculateBounds();
    }
}
