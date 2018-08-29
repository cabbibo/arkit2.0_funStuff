using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(BufferSDF))]
[RequireComponent(typeof(VertBuffer))]
[RequireComponent(typeof(TriangleBuffer))]
public class SDF_FromMesh : PhysicsSDF {


public int numTrisPerFrame;
public int currentTri;

public float CalculatedPercent;
public bool Calculated;

private TriangleBuffer tBuffer;
private VertBuffer vBuffer;

public override void GetBuffer(){
  if( buffer == null ){ buffer = GetComponent<BufferSDF>(); }
  if( tBuffer == null ){ tBuffer = GetComponent<TriangleBuffer>(); }
  if( vBuffer == null ){ vBuffer = GetComponent<VertBuffer>(); }
}

public override bool CheckNull(){
  return ( buffer != null && tBuffer != null && vBuffer != null  );
}

public override void Dispatch(){


  if( bufferSDF.loadedFromFile == false ){

      // We set if calculated is true below, and if we
      // have finished all of our calculations of the meshes
      // We do our final load step.
      if( Calculated == true ){
        update = false;
        OnCalculated();

        // skip our calculation step!
        return;

      }

      for( int i = 0; i < numTrisPerFrame; i++ ){

        CalculatedPercent = (float) currentTri / (float)tBuffer.count;

        // Skip our calculation step
        //1 = 100!!!
        if( CalculatedPercent >= 1 ){
          Calculated = true;
          return;
        }

        Calculate();

        // increase triangle index
        currentTri +=3;
      }

  }else{
    // if we load from file, set it all to finished
    CalculatedPercent = 1;
    Calculated = true;
  }





}


public void OnCalculated(){


  shader.SetVector("_Dimensions", bufferSDF.Dimensions );
  shader.SetVector("_Extents", bufferSDF.Extents );
  shader.SetVector("_Center", bufferSDF.Center );
  shader.SetInt("_Count", (int)bufferSDF.Dimensions.x * (int)bufferSDF.Dimensions.y *  (int)bufferSDF.Dimensions.z );
  shader.SetInt("_TriCount",tBuffer.count);
  shader.SetInt("_VertCount" ,vBuffer.count);
  shader.SetInt("_CurrentTri" ,currentTri);
  //print( currentTri);


  shader.SetVector("_Dimensions", bufferSDF.Dimensions );
  shader.SetInt("_TriCount",tBuffer.count);
  shader.SetInt("_VertCount",vBuffer.count);

  int k_normal = shader.FindKernel("GetNormal");
  int k_finalDepth = shader.FindKernel("GetFinalDist");

  print( k_normal);

  shader.SetBuffer(k_normal,  "volumeBuffer"   , bufferSDF._buffer );
  shader.SetBuffer(k_normal,  "vertBuffer"     , vBuffer._buffer );
  shader.SetBuffer(k_normal,  "triBuffer"      , tBuffer._buffer );
  shader.Dispatch(k_normal,numGroups,1,1);

  bufferSDF.OnSave();


}


public void Calculate(){
  //shader.SetVector("_VolDim", buffer.Dimensions );


  AssignTransform(transform);
  AssignInverseTransform(transform);

  shader.SetVector("_Dimensions", bufferSDF.Dimensions );
  shader.SetVector("_Extents", bufferSDF.Extents );
  shader.SetVector("_Center", bufferSDF.Center );
  shader.SetInt("_Count", (int)bufferSDF.count );
  shader.SetInt("_TriCount",tBuffer.count);
  shader.SetInt("_VertCount" ,vBuffer.count);
  shader.SetInt("_CurrentTri" ,currentTri);
//  print( currentTri);

  shader.SetBuffer(kernel, "volumeBuffer"  , bufferSDF._buffer );
  shader.SetBuffer(kernel, "vertBuffer"    , vBuffer._buffer );
  shader.SetBuffer(kernel, "triBuffer"     , tBuffer._buffer );
  shader.Dispatch(kernel,numGroups,1,1);
}

}}
