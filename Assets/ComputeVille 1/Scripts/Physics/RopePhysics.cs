using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ComputeVille{
[RequireComponent(typeof(RopeVertBuffer))]
public class RopePhysics: VerletPhysics{

  public float ropeLength;
  public Transform startPoint;
  public Transform endPoint;
  public override void GetBuffer(){ buffer = GetComponent<RopeVertBuffer>(); }
  public override void SetShaderValues(){

//    print("hmmmm1");
    shader.SetVector("_Start" , startPoint.position);
    shader.SetVector("_End" , endPoint.position);
    shader.SetFloat("_Length" , ropeLength );
    shader.SetFloat("_Count" , buffer.count );
    shader.SetFloat("_SpringDistance", ropeLength / (float)buffer.count );

  }

}
}

