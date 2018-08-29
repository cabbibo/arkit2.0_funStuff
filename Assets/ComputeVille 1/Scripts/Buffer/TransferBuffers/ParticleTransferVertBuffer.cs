using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ParticleTransferVertBuffer : TransferVertBuffer {

  public VertBuffer particles;
  private int vertsPerParticle = 4;

  public override void BeforeBuffer(){
    if( particles == null ){
      particles = GetComponent<VertBuffer>();
    }
  }

  public override void SetCount(){ count = particles.count * vertsPerParticle; }

}
}
