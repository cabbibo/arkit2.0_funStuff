using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(TransferVertBuffer))]
[RequireComponent(typeof(TransferTriBuffer))]
[RequireComponent(typeof(TrailBuffer))]
public class RibbonFromTransferBuffers : MeshFromTransferBuffers {

  public int length;
  public TrailBuffer trailBuffer;

 public override void GetBuffer(){
    if( buffer == null ){ buffer = GetComponent<TransferVertBuffer>(); }
    if( triBuffer == null ){ triBuffer = GetComponent<TransferTriBuffer>(); }
    if( particleBuffer == null ){ particleBuffer = GetComponent<TrailBuffer>(); }

    trailBuffer = (TrailBuffer)particleBuffer;
    material.SetBuffer("_transferBuffer", buffer._buffer);
  }

  public override void Dispatch(){
    shader.SetInt("_Count", buffer.count );
    shader.SetVector("_CameraRight", Camera.main.gameObject.transform.right );
    shader.SetVector("_CameraUp", Camera.main.gameObject.transform.up );
    shader.SetInt("_Length", length );
    shader.SetInt("_VertsPerParticle", trailBuffer.vertsPerParticle );
    shader.SetBuffer(kernel, "transferBuffer" , buffer._buffer );
    shader.SetBuffer(kernel, "trailBuffer" , particleBuffer._buffer );
    shader.Dispatch(kernel,numGroups,1,1);
  }


}}

