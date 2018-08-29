using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
[RequireComponent(typeof(TrailBuffer))]
public class TransferTrail : Physics {

  private TrailBuffer  tBuffer;

  public override void GetBuffer(){
    if( tBuffer == null ){ tBuffer = GetComponent<TrailBuffer>(); }
    if( buffer == null ){ buffer = tBuffer.particles; }
  }

  public override bool CheckNull(){
    return ( buffer._buffer != null && tBuffer._buffer != null );
  }

  public override void Dispatch(){
    shader.SetInt("_Count", buffer.count );
    shader.SetInt("_NumVertsPerTrail", tBuffer.vertsPerParticle );
    shader.SetBuffer(kernel, "vertBuffer" , buffer._buffer );
    shader.SetBuffer(kernel, "trailBuffer" , tBuffer._buffer );
    shader.Dispatch(kernel,numGroups,1,1);
  }

}
}
