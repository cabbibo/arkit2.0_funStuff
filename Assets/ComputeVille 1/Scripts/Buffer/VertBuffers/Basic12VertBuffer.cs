using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class Basic12VertBuffer : MeshVertBuffer {

  struct Vert{
    public Vector3 pos;
    public Vector3 nor;
    public Vector3 tan;
    public Vector2 uv;
    public float   debug;
  };


  public override void SetStructSize(){
    structSize = 3+3+3+2+1;
  }

  public override void SetOriginalValues(){

    Vector3 t1;
    Vector3 t2;
    Vector3 t3;
    int index = 0;

    Vector4[] tangents = mesh.tangents;

    for( int i = 0; i < count; i++ ){

      if( rotateMesh == true ){
        t1 = transform.TransformPoint( vertices[i] );
        t2 = transform.TransformDirection( normals[i] );
        t3 = transform.TransformDirection( tangents[i] );
      }else{
        t1 = vertices[i];
        t2 = normals[i];
        t3 = tangents[i];
      }

      // positions
      values[ index++ ] = t1.x;
      values[ index++ ] = t1.y;
      values[ index++ ] = t1.z;

      // normals
      values[ index++ ] = t2.x;
      values[ index++ ] = t2.y;
      values[ index++ ] = t2.z;

        // normals
      values[ index++ ] = t3.x;
      values[ index++ ] = t3.y;
      values[ index++ ] = t3.z;

      if( i < uvs.Length ){
      // uvs
      values[ index++ ] = uvs[i].x;
      values[ index++ ] = uvs[i].y;
    }else{

      // uvs
      values[ index++ ] = 0;
      values[ index++ ] = 0;
    }

      values[index ++] = 1;


    }

  }

}
}