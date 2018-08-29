using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class MeshTransferTriBuffer: TransferTriBuffer  {

  public MeshFilter meshFilter;
 
  public override void SetCount(){ count = meshFilter.mesh.triangles.Length; }
  public override void MakeMesh(){ 
//    print("values");
 //   print(meshFilter.mesh.triangles.Length );
    values = meshFilter.mesh.triangles; 
  }


}
}