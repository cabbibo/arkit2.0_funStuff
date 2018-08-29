using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
public class HairVertBuffer : VerletVertBuffer {

  public int numHairs;
  public int numVertsPerHair;

  public override void SetCount(){
    count = numHairs * numVertsPerHair;
  }

  public override void SetOriginalValues(){

    int index = 0;
    for( int i = 0; i < numHairs; i++ ){
      for( int j = 0; j < numVertsPerHair; j++ ){
      int bID = i * numVertsPerHair + j;

      // positions
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // positions
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      
      // ID Down
      values[ index++ ] = j-1 >= 0 ? bID - 1 : -10 ;

      // ID Up
      values[ index++ ] = (j+1) < numVertsPerHair ? bID+1 : -20;

      // uvs
      values[ index++ ] = i/numHairs;
      values[ index++ ] = j/numVertsPerHair;

      // debug
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;
    }

    }

  }
}
}