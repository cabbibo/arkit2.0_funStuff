using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
public class BufferSDF : Buffer3DWithTexture {


    public override void SetStructSize(){
        structSize = 8;
    }

    public override void MakeValueArray(){
        values = new float[ count * structSize ];
    }

    public override void SetOriginalValues(){

        int index = 0;
        for( int i = 0; i < count*structSize; i++ ){
            values[index++] = 1000;
        }
    }

    public virtual void OnSave(){
        RemakeTexture();
        Save();
    }


}
}
