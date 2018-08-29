using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
[RequireComponent(typeof(BoneBuffer))]
[RequireComponent(typeof(SkinnedVertBuffer))]
public class Skin : Physics {

  private BoneBuffer  bBuffer;

  public override void GetBuffer(){
    if( buffer == null ){ buffer = GetComponent<SkinnedVertBuffer>(); }
    if( bBuffer == null ){ bBuffer = GetComponent<BoneBuffer>(); }
  }

  public override bool CheckNull(){
    return ( buffer._buffer != null && bBuffer._buffer != null );
  }


  public override void Dispatch(){
    shader.SetInt("_Count", buffer.count );
    shader.SetBuffer(kernel, "vertBuffer" , buffer._buffer );
    shader.SetBuffer(kernel, "boneBuffer" , bBuffer._buffer );
    shader.Dispatch(kernel,numGroups,1,1);
  }

}
}
