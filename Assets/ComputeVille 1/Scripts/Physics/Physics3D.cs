using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ComputeVille{
[RequireComponent(typeof(Buffer3D))]
public class Physics3D: Physics{

  protected Buffer3D buffer3D;
  public override void GetBuffer(){ buffer = GetComponent<Buffer3D>(); }
  public override void CastBuffer(){ buffer3D = (Buffer3D)buffer; }

}
}

