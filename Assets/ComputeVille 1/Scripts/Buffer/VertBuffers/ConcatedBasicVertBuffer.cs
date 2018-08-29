using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Array of meshes must be the same in Vert and Tri buffer!!!!
namespace ComputeVille{
public class ConcatedBasicVertBuffer : BasicVertBuffer {

  public GameObject[] meshes;

  public override void GetMesh(){

    count = 0;
    for( int i = 0; i < meshes.Length; i++ ){
      count += meshes[i].GetComponent<MeshFilter>().mesh.vertices.Length;
    }

    vertices = new Vector3[ count ];
    normals = new Vector3[ count ];
    uvs = new Vector2[ count ];

    int index = 0;
    for( int i = 0; i < meshes.Length; i++ ){
      Vector3[] v = meshes[i].GetComponent<MeshFilter>().mesh.vertices;
      Vector3[] n = meshes[i].GetComponent<MeshFilter>().mesh.normals;
      Vector2[] u = meshes[i].GetComponent<MeshFilter>().mesh.uv;

      for( int j = 0; j < v.Length; j++ ){
        vertices[index] = meshes[i].transform.TransformPoint(v[j]);
        normals[index]  = meshes[i].transform.TransformDirection(n[j]);
        uvs[index]      = u[j];
        index += 1;
      }
    }
  }
}
}
