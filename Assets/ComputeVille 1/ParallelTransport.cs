using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComputeVille;

public class ParallelTransport : MonoBehaviour {

  public VerletVertBuffer buffer;
  public ComputeShader shader;
  public Transform start;
  public Transform end;


  void Dispatch(){

    shader.SetInt("_Count", buffer.count);
    shader.SetBuffer(0,"vertBuffer", buffer._buffer);
    shader.SetVector("_Start", start.position);
    shader.SetVector("_End", end.position);
    shader.Dispatch(0,1,1,1);

  }

  void LateUpdate(){
    Dispatch();
  }
}
