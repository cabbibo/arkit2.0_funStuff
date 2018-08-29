using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ProceduralMeshAlongCurve : ProceduralRender {

  public GameObject meshObject;
  public TriangleBuffer tBuffer;
  public VerletVertBuffer cBuffer;
  public float modelLength;
  public Transform start;
  public Transform end;

  public void OnDrawGizmos(){

    Vector3 p = transform.position;
    Vector3 p1 = p+ Vector3.forward * modelLength;
    Gizmos.DrawLine(p,p1);
    Gizmos.DrawLine(p1 , p1 + Vector3.up * modelLength * .2f);
    Gizmos.DrawLine(p1 , p1 - Vector3.up * modelLength * .2f);
    Gizmos.DrawLine(p1 , p1 + Vector3.right * modelLength * .2f);
    Gizmos.DrawLine(p1 , p1 - Vector3.right * modelLength * .2f);



    Gizmos.DrawLine(p , p + Vector3.up * modelLength * .2f);
    Gizmos.DrawLine(p , p - Vector3.up * modelLength * .2f);
    Gizmos.DrawLine(p , p + Vector3.right * modelLength * .2f);
    Gizmos.DrawLine(p , p - Vector3.right * modelLength * .2f);
  }

  public override void GetBuffer(){
    if( buffer == null ){ buffer = meshObject.GetComponent<VertBuffer>(); }
    if( tBuffer == null ){ tBuffer = meshObject.GetComponent<TriangleBuffer>(); }
    if( cBuffer == null ){ cBuffer = GetComponent<VerletVertBuffer>(); }
  }
  public override bool CheckNull(){
    return ( tBuffer._buffer != null && buffer._buffer != null && cBuffer._buffer != null );
  }

  public override void Render(){
    material.SetPass(0);
    material.SetBuffer("_vertBuffer", buffer._buffer);
    material.SetBuffer("_curveBuffer", cBuffer._buffer);
    material.SetBuffer("_triBuffer", tBuffer._buffer);
    material.SetInt("_VertCount",buffer.count);
    material.SetInt("_CurveCount",cBuffer.count);
    material.SetFloat("_ModelLength", modelLength);
    material.SetVector("_Start", start.position);
    material.SetVector("_End", end.position);
    Graphics.DrawProcedural(MeshTopology.Triangles, tBuffer.count );
  }



}
}
