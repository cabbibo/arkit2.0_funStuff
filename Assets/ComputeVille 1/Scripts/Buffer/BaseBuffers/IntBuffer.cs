using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille {

public class IntBuffer : Buffer {

  protected int[] values;


  public virtual void MakeValueArray(){
    values = new int[ count * structSize ];
  }

  public int[] GetValues(){
    return values;
  }

  public void SetValues( int[] vals ){
    values = vals;
  }

  public override void CreateBuffer(){
    MakeValueArray();
    _buffer = new ComputeBuffer( count , sizeof(int) * structSize );
  }

  public void SetData(){
    _buffer.SetData( values );
  }

  public void GetData(){
    _buffer.GetData( values );
  }

}
}
