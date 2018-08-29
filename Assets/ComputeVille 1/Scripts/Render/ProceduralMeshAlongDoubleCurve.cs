using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ProceduralMeshAlongDoubleCurve : ProceduralRender {

  public GameObject meshObject;
  public TriangleBuffer tBuffer;
  public VerletVertBuffer cBuffer1;
  public VerletVertBuffer cBuffer2;
  public float modelLength;

  public void OnDrawGizmos(){

    Vector3 p = transform.position;
    Vector3 p1 = p+ Vector3.forward * modelLength;
    Gizmos.DrawLine(p,p1);
    Gizmos.DrawLine(p1 , p1 + Vector3.up * modelLength * .2f);
    Gizmos.DrawLine(p1 , p1 - Vector3.up * modelLength * .2f);
    Gizmos.DrawLine(p1 , p1 + Vector3.right * modelLength * .2f);
    Gizmos.DrawLine(p1 , p1 - Vector3.right * modelLength * .2f);
  }

  public override void GetBuffer(){
    if( buffer == null ){ buffer = meshObject.GetComponent<VertBuffer>(); }
    if( tBuffer == null ){ tBuffer = meshObject.GetComponent<TriangleBuffer>(); }
    //if( cBuffer == null ){ cBuffer = GetComponent<VerletVertBuffer>(); }
  }
  public override bool CheckNull(){
    return ( tBuffer._buffer != null && buffer._buffer != null && cBuffer1._buffer != null&& cBuffer2._buffer != null  );
  }

  public override void Render(){
    material.SetPass(0);
    material.SetBuffer("_vertBuffer", buffer._buffer);
    material.SetBuffer("_curveBuffer1", cBuffer1._buffer);
    material.SetBuffer("_curveBuffer2", cBuffer2._buffer);
    material.SetBuffer("_triBuffer", tBuffer._buffer);
    material.SetInt("_VertCount",buffer.count);
    material.SetInt("_CurveCount",cBuffer1.count);
    material.SetFloat("_ModelLength", modelLength);
    Graphics.DrawProcedural(MeshTopology.Triangles, tBuffer.count );
  }



}
}