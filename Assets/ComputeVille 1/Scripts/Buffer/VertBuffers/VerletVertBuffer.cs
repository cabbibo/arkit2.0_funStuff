using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class VerletVertBuffer : VertBuffer {

  struct Vert{
    public Vector3 pos;
    public Vector3 oPos;
    public Vector3 nor;
    public Vector3 tangent;
    public Vector3 bitangent;
    public float idUp;
    public float idDown;
    public Vector2 uv;
    public Vector3 debug;

  };

  public override void SetStructSize(){
    structSize = 3+3+3+1+1+2+3+3+3;
  }



}
}
