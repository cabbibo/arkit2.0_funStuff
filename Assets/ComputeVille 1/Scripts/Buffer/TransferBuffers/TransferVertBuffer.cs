using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class TransferVertBuffer :  FloatBuffer {

  struct Vert{
    public Vector3 pos;
    public Vector3 nor;
    public Vector2 uv;
  }

  public override void SetStructSize(){ structSize = 8; }


}
}
