using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class TrailTransferVertBuffer :  TransferVertBuffer{

  public TrailBuffer trail;
  public int tubeWidth;
  public int tubeLength;

  public override void BeforeBuffer(){
    if( trail == null ){
      trail = GetComponent<TrailBuffer>();
    }
  }

  public override void SetCount(){ count = trail.particles.count * tubeLength * tubeWidth; }

}
}
