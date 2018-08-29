using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class LifeParticleBuffer : ParticleBuffer {

  public override void SetOriginalValues(){

    int index = 0;
    for( int i = 0; i < count; i++ ){

      // positions
      values[index++]= 0;
      values[index++]= 0;
      values[index++]= 0;

      // velocity
      values[index++]= 0;
      values[index++]= 0;
      values[index++]= 0;

      // id
      values[index++] = i;
      values[index++] = Random.Range(0,.999999f);
      values[index++]= 0;

      // debug
      values[index++]= 0;
      values[index++]= 0;
      values[index++]= 0;


    }

  }

}
}
