using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ProceduralMeshRender : ProceduralRender {

  public TriangleBuffer tBuffer;

  public override void GetBuffer(){
    if( buffer == null ){ buffer = GetComponent<VertBuffer>(); }
    if( tBuffer == null ){ tBuffer = GetComponent<TriangleBuffer>(); }
  }
  public override bool CheckNull(){
    return ( tBuffer._buffer != null && buffer._buffer != null );
  }

  public override void Render(){
    material.SetPass(0);
    material.SetBuffer("_vertBuffer", buffer._buffer);
    material.SetBuffer("_triBuffer", tBuffer._buffer);
    material.SetInt("_VertCount",buffer.count);
    Graphics.DrawProcedural(MeshTopology.Triangles, tBuffer.count );
  }



}
}
