using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
public class RopeVertBuffer : VerletVertBuffer {

  public override void SetOriginalValues(){

    int index = 0;
    for( int i = 0; i < count; i++ ){

      // positions
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // positions
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 1;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 0;
      values[ index++ ] = 1;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 1;
      
      // ID Down
      values[ index++ ] = i-1 >= 0 ? i-1 : -10 ;

      // ID Up
      values[ index++ ] = (i+1) < count ? i+1 : -1 ;

      // uvs
      values[ index++ ] = i/count;
      values[ index++ ] = i;

      // debug
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

    }

    SetData();

  }

}
}
