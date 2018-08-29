using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class RandomVertBuffer : BasicVertBuffer {

 

  public override void SetOriginalValues(){

    int index = 0;
    for( int i = 0; i < count; i++ ){

      // used 
      values[ index++ ] = Random.Range( 0.01f, .99f);


      // positions
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // vel
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // uvs
      values[ index++ ] = (float)i/(float)count;
      values[ index++ ] = i;


      // target pos
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;


      // Debug
      values[ index++ ] = Random.Range( 0.7f, .99f);
      values[ index++ ] = 0;
      values[ index++ ] = 1;

    }
  }

}
}