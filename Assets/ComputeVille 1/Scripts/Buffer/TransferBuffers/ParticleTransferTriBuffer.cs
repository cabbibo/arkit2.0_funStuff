using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ParticleTransferTriBuffer: TransferTriBuffer  {

  public VertBuffer particles;
  private int trianglesPerParticle = 2;


  public override void GetBuffer(){
    if( particles == null ){
      particles = GetComponent<VertBuffer>();
    }
  }


  public override void SetCount(){ count = particles.count * trianglesPerParticle * 3; }
  public override void MakeMesh(){

    int index = 0;

    int id0 = 0;
    int id1 = 1;
    int id2 = 2;
    int id3 = 3;

    for( int i = 0; i < particles.count; i++ ){

        // 4 verts per triangle
        int baseID = 4 * i;

        /*
            2-3
            |/|
            0-1
        */
        values[index++] = baseID + id0;
        values[index++] = baseID + id3;
        values[index++] = baseID + id1;
        values[index++] = baseID + id0;
        values[index++] = baseID + id2;
        values[index++] = baseID + id3;
    }
  }


}
}
