using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class PlaneVertBuffer : VertBuffer {
  public int Width = 10;
  public int Height = 10;
  public Vector2 Size;
  public Vector3 Normal;
  public Vector3 xDir;
  public Vector3 yDir;

  struct Vert{
    public Vector3 pos;
    public Vector3 nor;
    public Vector2 uv;
  };


  public override void SetStructSize(){
    structSize = 3+3+2;
  }
public override void SetCount(){
  count = Width * Height;
}





  public override void SetOriginalValues(){

    Vector3 t1;
    Vector3 t2;
    int index = 0;


    xDir = Vector3.Cross( Normal , Vector3.up );

    if( xDir.magnitude == 0 ){
      xDir = Vector3.Cross( Normal , Vector3.forward);
    }
    xDir = xDir.normalized;

    yDir = Vector3.Cross( xDir , Normal).normalized;


    for (int i = 0; i < Width; i++){
    for (int j = 0; j < Height; j++){

      Vector3 vec = transform.position;//Vector3.zero;
      vec  += xDir * Size.x *((float)i / (Height-1) - 0.5f);
      vec  += yDir * Size.y *((float)j / (Width-1) - 0.5f);

      // positions
      values[index++] = vec.x;
      values[index++] = vec.y;
      values[index++] = vec.z;

      // normals
      values[index++] = Normal.x;
      values[index++] = Normal.y;
      values[index++] = Normal.z;

      // uvs
      values[index++] = (float)i / (Width - 1);
      values[index++] = (float)j / (Height - 1);

    }
    }
  }


}
}
