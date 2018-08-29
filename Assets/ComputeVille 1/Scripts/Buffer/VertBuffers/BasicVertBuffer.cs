using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class BasicVertBuffer : MeshVertBuffer {

  struct Vert{
    public Vector3 pos;
    public Vector3 nor;
    public Vector2 uv;
  };


  public override void SetStructSize(){
    structSize = 3+3+2;
  }

  public override void SetOriginalValues(){

    Vector3 t1;
    Vector3 t2;
    int index = 0;


    for( int i = 0; i < count; i++ ){

      if( rotateMesh == true ){
        t1 = transform.TransformPoint( vertices[i] );
        t2 = transform.TransformDirection( normals[i] );
      }else{
        t1 = vertices[i];
        t2 = normals[i];
      }

      // positions
      values[ index++ ] = t1.x;
      values[ index++ ] = t1.y;
      values[ index++ ] = t1.z;

      // normals
      values[ index++ ] = t2.x;
      values[ index++ ] = t2.y;
      values[ index++ ] = t2.z;

      // uvs
      values[ index++ ] =0;// uvs[i].x;
      values[ index++ ] =0;// uvs[i].y;


    }

  }

}
}
