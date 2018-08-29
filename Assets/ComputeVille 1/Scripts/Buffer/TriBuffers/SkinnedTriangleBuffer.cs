using UnityEngine;
using System.Collections;


namespace ComputeVille{
public class SkinnedTriangleBuffer : MeshTriangleBuffer {

  public override void GetMesh(){
    if( mesh == null){
      mesh = gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
      print( mesh.triangles[10] );
    }
  }
}
}
