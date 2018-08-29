using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class MeshTransfer12VertBuffer : TransferVertBuffer {

  public MeshFilter meshFilter;
  public override void SetStructSize(){ structSize = 12; }
  public override void SetCount(){ count = meshFilter.mesh.vertices.Length; }

}
}
