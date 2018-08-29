using UnityEngine;
using System.Collections;


namespace ComputeVille{
public class BasicTriangleBuffer : MeshTriangleBuffer {

  public override void GetMesh(){
    if( mesh == null){
      mesh = gameObject.GetComponent<MeshFilter>().mesh;
    }
  }

}
}
