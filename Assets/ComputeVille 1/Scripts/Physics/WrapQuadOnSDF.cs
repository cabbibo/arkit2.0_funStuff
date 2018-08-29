using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
[RequireComponent(typeof(PlaneVertBuffer))]
public class WrapQuadOnSDF : Physics {

public MeshIntersection mesh;

public BufferSDF volBuffer;
private int setKernel;


public override void GetBuffer(){
  if( buffer == null ){ buffer = GetComponent<PlaneVertBuffer>(); }
  if( volBuffer == null ){ volBuffer = GetComponent<BufferSDF>(); }
}

public override bool CheckNull(){
  return ( buffer._buffer != null && volBuffer._texture != null && volBuffer._buffer != null );
}

public override void AfterLoaded(){
    print("hi");

}

public override void FindKernel(){
  kernel = shader.FindKernel( kernelName );
  setKernel = shader.FindKernel( "Set" );
}

public override void Dispatch(){

// print("Dispatch");
  shader.SetInt("_VolDim", (int)volBuffer.Dimensions.x );
  shader.SetVector("_Dimensions", volBuffer.Dimensions );
  shader.SetInt("_ParticleCount",buffer.count);
  shader.SetFloat("_DT",Time.deltaTime);
  shader.SetVector("_Extents",volBuffer.Extents);
  shader.SetVector("_Center",volBuffer.Center);

  shader.SetTexture(kernel,"sdfTexture",volBuffer._texture );
  shader.SetBuffer(kernel, "volumeBuffer"  , volBuffer._buffer );
  shader.SetBuffer(kernel, "vertBuffer"    , buffer._buffer    );

  shader.Dispatch(kernel,numGroups,1,1);
}

public void SetPlane( Vector3 p , Vector3 n ){

  print(p);
  print(n);

  shader.SetVector("_Pos" , p+n * .04f);
  shader.SetVector("_Nor" , n);
  shader.SetInt("_ParticleCount",buffer.count);
  shader.SetInt("_Width",((PlaneVertBuffer)buffer).Width);
  shader.SetInt("_Height",((PlaneVertBuffer)buffer).Height);
  shader.SetVector("_Size",((PlaneVertBuffer)buffer).Size);


  shader.SetBuffer(setKernel, "vertBuffer"    , buffer._buffer    );

  shader.Dispatch(setKernel,numGroups,1,1);
}


}
}
