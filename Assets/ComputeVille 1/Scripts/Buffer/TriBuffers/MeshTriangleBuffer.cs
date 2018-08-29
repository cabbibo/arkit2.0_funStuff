using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class MeshTriangleBuffer : TriangleBuffer {

  public Mesh mesh;

  public override void GetTriangles(){
    GetMesh();
    values =  mesh.triangles;
    count = values.Length;
  }

  public override void AfterBuffer(){
    values =  mesh.triangles;
    SetData();
  }

  public virtual void GetMesh(){
    if( mesh == null){
      mesh = gameObject.GetComponent<MeshFilter>().mesh;
    }
  }

}
}
