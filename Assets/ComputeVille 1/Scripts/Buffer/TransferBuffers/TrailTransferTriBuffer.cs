using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class TrailTransferTriBuffer :  TransferTriBuffer {

  public TrailBuffer trail;
  public int tubeWidth;
  public int tubeLength;


  public override void GetBuffer(){
    if( trail == null ){
      trail = GetComponent<TrailBuffer>();
    }
  }



  public override void SetCount(){ count = trail.particles.count * (tubeLength-1) * tubeWidth * 3 * 2; }


  public override void MakeMesh(){

    int index = 0;

    for( int i = 0; i < trail.particles.count; i++ ){
        for( int j = 0; j < tubeLength-1; j++ ){
            for( int k = 0; k < tubeWidth; k++ ){

                int baseID = i * tubeWidth * (tubeLength);

                int rID1 = j * tubeWidth;
                int rID2 = (j+1) * tubeWidth;

                int cID1 = k % tubeWidth;
                int cID2 = (k+1) % tubeWidth;

                  /*
                      2-3
                      |/|
                      0-1
                  */

                values[index++] = baseID + rID1 + cID1;
                values[index++] = baseID + rID2 + cID1;
                values[index++] = baseID + rID2 + cID2;
                values[index++] = baseID + rID1 + cID1;
                values[index++] = baseID + rID2 + cID2;
                values[index++] = baseID + rID1 + cID2;
            }
        }
    }


  }



}
}
