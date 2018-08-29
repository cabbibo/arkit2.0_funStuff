using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class RibbonTransferVertBuffer :  TransferVertBuffer{

  public TrailBuffer trail;
  public int length;

  public override void BeforeBuffer(){
    if( trail == null ){
      trail = GetComponent<TrailBuffer>();
    }
  }

  public override void SetCount(){ count = trail.particles.count * length * 2; }

}
}
