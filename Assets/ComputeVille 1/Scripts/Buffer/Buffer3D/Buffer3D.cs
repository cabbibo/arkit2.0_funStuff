using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class Buffer3D : SaveableFloatBuffer {


  public Vector3 Dimensions;
  public Vector3 Center;
  public Vector3 Extents;
  public int dimension;
  public bool showGrid = false;

  public void OnDrawGizmos(){
    Gizmos.matrix = transform.localToWorldMatrix;

    if( showGrid ){
    Gizmos.color = new Vector4(0.2f,.5f,1.2f,1);

      for( int s = 0; s < 3; s++){




        float DimensionsX = Dimensions.x;
        float DimensionsY = Dimensions.y;

        float extentsX = Extents.x;
        float extentsY = Extents.y;
        float extentsZ = Extents.z;

        Vector3 xDir = Vector3.left;
        Vector3 yDir = Vector3.up;
        Vector3 zDir = Vector3.forward;


        if( s == 1 ){
          DimensionsX = Dimensions.x;
          DimensionsY = Dimensions.z;
          extentsX = Extents.x;
          extentsY = Extents.z;
          extentsZ = Extents.y;

          xDir = Vector3.left;
          yDir = Vector3.forward;
          zDir = Vector3.up;
        }

        if( s == 2 ){
          DimensionsX = Dimensions.z;
          DimensionsY = Dimensions.y;
          extentsX = Extents.z;
          extentsY = Extents.y;
          extentsZ = Extents.x;
           xDir = Vector3.forward;
          yDir = Vector3.up;
          zDir = Vector3.left;
        }

        float sizeX = extentsX * 2 / DimensionsX;
        float sizeY = extentsY * 2 / DimensionsY;



        for(float i  = 0; i < 2; i++ ){

             float z = (i - .5f) *2 * extentsZ;
          for( float j = 0; j< DimensionsX;j++){
            for( float k = 0; k < DimensionsY; k++ ){


            Vector3 p1 = Center + (sizeX * j     - extentsX)*xDir + (sizeY * k     - extentsY)*yDir + z*zDir;
            Vector3 p2 = Center + (sizeX * (j+1) - extentsX)*xDir + (sizeY * k     - extentsY)*yDir + z*zDir;
            Vector3 p3 = Center + (sizeX * j     - extentsX)*xDir + (sizeY * (k+1) - extentsY)*yDir + z*zDir;
            if( k != 0 ) Gizmos.DrawLine(p1, p2);
            if( j != 0 ) Gizmos.DrawLine(p1, p3);
           }
          }
        }
      }
    }


    Gizmos.color = new Vector4(1,.6f,.2f,1);
    Gizmos.DrawWireCube(Center, Extents*2);
  }

  public override void SetCount(){ count =(int)(Dimensions.x * Dimensions.y * Dimensions.z); }


  public override void AfterBuffer(){
    Load();
  }

  public override void CreateNewValues(){
    SetOriginalValues();
    SetData();
  }


  public virtual void SetOriginalValues(){}

}
}
