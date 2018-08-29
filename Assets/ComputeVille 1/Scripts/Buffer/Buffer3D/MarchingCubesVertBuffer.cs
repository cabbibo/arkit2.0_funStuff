using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class MarchingCubesVertBuffer : VertBuffer {

  public struct Vert
  {
    public Vector3 pos;
    public Vector3 nor;
  };

  public BufferSDF sdf;

  public override void BeforeBuffer(){
    if( sdf == null ){ sdf = GetComponent<BufferSDF>(); }
  }

  public override void SetStructSize(){
    structSize = 3+3;
  }

  public override void SetCount(){
    count = sdf.count*5*3;
  }

  public override void SetOriginalValues(){
    for( int i = 0; i< count*6;i++){
      values[i] = -1;
    }
  }


}}
