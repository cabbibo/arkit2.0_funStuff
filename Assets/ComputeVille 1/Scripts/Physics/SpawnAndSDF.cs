using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(SpawnParticleBuffer))]
public class SpawnAndSDF : Physics {

public BufferSDF volBuffer;
public override void GetBuffer(){
  if( buffer == null ){ buffer = GetComponent<SpawnParticleBuffer>(); }
  if( volBuffer == null ){ volBuffer = GetComponent<BufferSDF>(); }
}

public override bool CheckNull(){
  return ( buffer._buffer != null && volBuffer._texture != null && volBuffer._buffer != null );
}

public override void Dispatch(){

// print("Dispatch");
  AssignInverseTransform( volBuffer.gameObject.transform );
  AssignTransform( volBuffer.gameObject.transform );
  shader.SetInt("_VolDim", (int)volBuffer.Dimensions.x );
  shader.SetInt("_ParticleCount",buffer.count);
  shader.SetFloat("_DT",Time.deltaTime);
  shader.SetVector("_Extents",volBuffer.Extents);
  shader.SetVector("_Center",volBuffer.Center);
  shader.SetVector("_Dimensions",volBuffer.Center);

  shader.SetTexture(kernel,"sdfTexture",volBuffer._texture );
  shader.SetBuffer(kernel, "volumeBuffer"  , volBuffer._buffer );
  shader.SetBuffer(kernel, "vertBuffer"    , buffer._buffer    );

  shader.Dispatch(kernel,numGroups,1,1);
}

}}
