using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class FloatBuffer : Buffer {

  protected float[] values;

  public override void CreateBuffer(){
    MakeValueArray();
    print("buffer created : " + gameObject.name );
    print( structSize );
    print( count  );
    _buffer = new ComputeBuffer( count , sizeof(float) * structSize );
  }

  public virtual void MakeValueArray(){
    if( values == null ){ values = new float[ count * structSize ]; }else{
      print( "Values already created!");
    }
  }


  public float[] GetValues(){
    return values;
  }

  public float[] GetNewValues(){
    GetData();
    return GetValues();
  }

  public void SetValues( float[] vals ){
    values = vals;
  }

  public void SetData(){
    _buffer.SetData( values );
  }

   public void GetData(){
    _buffer.GetData( values );
  }


}
}
