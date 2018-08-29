using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ComputeVille{
[RequireComponent(typeof(BufferSDF))]
public class PhysicsSDF: Physics{

  protected BufferSDF bufferSDF;
  public override void GetBuffer(){ buffer = GetComponent<BufferSDF>(); }
  public override void CastBuffer(){ bufferSDF = (BufferSDF)buffer; }

}
}
