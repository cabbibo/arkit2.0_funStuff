using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions{

  public static Vector3 GetRandomPointInTriangle( int seed, Vector3 v1 , Vector3 v2 , Vector3 v3 ){
   
    /* Triangle verts called a, b, c */

    Random.InitState(seed* 14145);
    float r1 = Random.value;

    Random.InitState(seed* 19247);
    float r2 = Random.value;
    //float r3 = Random.value;

    return (1 - Mathf.Sqrt(r1)) * v1 + (Mathf.Sqrt(r1) * (1 - r2)) * v2 + (Mathf.Sqrt(r1) * r2) * v3;
     
    ///return (r1 * v1 + r2 * v2 + r3 * v3) / (r1 + r2 + r3);
  }

  public static float AreaOfTriangle( Vector3 v1 , Vector3 v2 , Vector3 v3 ){
     Vector3 v = Vector3.Cross(v1-v2, v1-v3);
     float area = v.magnitude * 0.5f;
     return area;
  }


  public static Vector3 ToV3( Vector4 parent)
  {
     return new Vector3(parent.x, parent.y, parent.z);
  }

  public static float getRandomFloatFromSeed( int seed ){
    Random.InitState(seed);
    return Random.value;
  }

  public static int getTri(float randomVal, float[] triAreas){


    int triID = 0;
    float totalTest = 0;
    for( int i = 0; i < triAreas.Length; i++ ){

      totalTest += triAreas[i];
      if( randomVal <= totalTest){
        triID = i;
        break;
      }

    }

    return triID;

  }

  public static float[] GetTriAreas(int[] triangles , Vector3[] vertices){

    float[] triAreas = new float[triangles.Length/3];


    float totalArea = 0;
    for( int i = 0; i < triangles.Length/3; i++ ){
      int tri0 = i * 3;
      int tri1 = tri0 + 1;
      int tri2 = tri0 + 2;
      tri0 = triangles[tri0];
      tri1 = triangles[tri1];
      tri2 = triangles[tri2];
      float area = HelperFunctions.AreaOfTriangle( vertices[tri0] , vertices[tri1] , vertices[tri2] );
      triAreas[i] = area;
      totalArea += area;
    }

    for( int i = 0; i < triAreas.Length; i++ ){
      triAreas[i] /= totalArea;
    }


    return triAreas;

  }


  public static Vector3[] GetIDsAndWeights( int seed , float[] triAreas , int[] triangles , Vector3[] vertices ){

    float randomVal = getRandomFloatFromSeed( seed * 20 );
    
    int tri0 = 3 * getTri( randomVal ,triAreas );
    int tri1 = tri0 + 1;
    int tri2 = tri0 + 2;

    tri0 = triangles[tri0];
    tri1 = triangles[tri1];
    tri2 = triangles[tri2];

    Vector3 pos = GetRandomPointInTriangle( seed , vertices[ tri0 ] , vertices[ tri1 ]  , vertices[ tri2 ]  );
    
    float a0 = HelperFunctions.AreaOfTriangle( pos , vertices[tri1] , vertices[tri2] );
    float a1 = HelperFunctions.AreaOfTriangle( pos , vertices[tri0] , vertices[tri2] );
    float a2 = HelperFunctions.AreaOfTriangle( pos , vertices[tri0] , vertices[tri1] );
    float aTotal = a0 + a1 + a2;

    float p0 = a0 / aTotal;
    float p1 = a1 / aTotal;
    float p2 = a2 / aTotal;

    return new [] {new Vector3(tri0,tri1,tri2) , new Vector3(p0,p1,p2) };

  }


}
