using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ComputeVille{
public class TrailBuffer :  FloatBuffer {

  public VertBuffer particles;
  public int vertsPerParticle;

  struct Vert{
    public Vector3 pos;
    public float id;
  };


  public override void BeforeBuffer(){
    if( particles == null ){
      particles = GetComponent<VertBuffer>();
    }
  }

  public override void SetCount(){ count = particles.count * vertsPerParticle;}
  public override void SetStructSize(){ structSize = 4; }


}
}
