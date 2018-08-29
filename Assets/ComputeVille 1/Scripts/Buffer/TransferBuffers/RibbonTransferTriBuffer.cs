using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class RibbonTransferTriBuffer :  TransferTriBuffer {

  public TrailBuffer trail;
  public int length;


  public override void GetBuffer(){
    if( trail == null ){
      trail = GetComponent<TrailBuffer>();
    }
  }



  public override void SetCount(){ count = trail.particles.count * (length-1) * 3 * 2; }


  public override void MakeMesh(){

    int index = 0;

    for( int i = 0; i < trail.particles.count; i++ ){
        for( int j = 0; j < length-1; j++ ){

                int baseID = i * 2* (length);


                  /*
                      2-3
                      |/|
                      0-1
                  */

                values[index++] = baseID + j*2 + 0;
                values[index++] = baseID + j*2 + 1;
                values[index++] = baseID + (j+1)*2 + 1;
                values[index++] = baseID + j*2 + 0;
                values[index++] = baseID + (j+1)*2 + 1;
                values[index++] = baseID + (j+1)*2 + 0;
            
        }
    }


  }



}
}