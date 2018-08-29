using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ConcatedBasicTriBuffer : MeshTriangleBuffer {

  public GameObject[] meshes;

	public override void GetMesh(){}

  public override void BeforeBuffer(){
    structSize = 1;
    GetTriangles();
  }

  public override void AfterBuffer(){
    SetData();
  }

  public override void GetTriangles(){

    count = 0;

    int baseVal = 0;
    for( int i = 0; i < meshes.Length; i++ ){
      int[] tris = meshes[i].GetComponent<MeshFilter>().mesh.triangles;
      count += tris.Length;
    }

    values = new int[ count ];

    int index = 0;

    for( int i = 0; i < meshes.Length; i++ ){

      int[] tris = meshes[i].GetComponent<MeshFilter>().mesh.triangles;

      for( int j = 0; j < tris.Length; j++ ){ values[index++] = tris[j] + baseVal; }

      baseVal += meshes[i].GetComponent<MeshFilter>().mesh.vertices.Length;

    }


  }
}
}
