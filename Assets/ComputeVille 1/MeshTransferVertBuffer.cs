using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class MeshTransferVertBuffer : TransferVertBuffer {

  public MeshFilter meshFilter;
  public override void SetCount(){ count = meshFilter.mesh.vertices.Length; }

}
}
