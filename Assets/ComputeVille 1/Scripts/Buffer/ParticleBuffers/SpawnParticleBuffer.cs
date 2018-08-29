using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class SpawnParticleBuffer : VertBuffer {

  struct Particle{
    public Vector3 pos;
    public Vector3 vel;
    public Vector3 ogPos;
    public Vector2 uv;
    public float life;  // lifetime of particle
    public float cap;   // connection information
    public Vector3 debug; //
  }

  public override void SetStructSize(){
    structSize = 3 +3+ 3 + 2 + 1 + 1 + 3;
  }

  public override void SetOriginalValues(){

    int index = 0;
    for( int i = 0; i < count; i++ ){

      Vector3 p = Random.insideUnitSphere;
      // positions
      values[index++]= p.x;
      values[index++]= p.y;
      values[index++]= p.z;

      // velocity
      values[index++]= 0;
      values[index++]= 0;
      values[index++]= 0;


      // ogPosition
      values[index++]=  p.x;
      values[index++]=  p.y;
      values[index++]=  p.z;

      values[index++]= (float)i / (float)count;
      values[index++]= 0;

      values[index++] = Random.Range(0,.999999f);
       values[index++] = i;


      // debug
      values[index++]= 0;
      values[index++]= 0;
      values[index++]= 0;

    }

  }

}
}
