using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(VertBuffer))]
[RequireComponent(typeof(TriangleBuffer))]
public class MeshIntersection : Physics {

public GameObject closestPointPrefab;

public delegate void TouchDown(Transform t , Vector3 p , Vector3 n);
public event TouchDown OnTouchDown;
public delegate void TouchUp(Transform t, Vector3 p , Vector3 n);
public event TouchUp OnTouchUp;

public delegate void ClosestPoint(float d , float oD , Transform t, Vector3 p , Vector3 n);
public event ClosestPoint OnClosestPoint;

public List<Transform> objectsToIntersect;
public float intersectionRadius;

public bool spawnClosest;

public VertBuffer vBuffer;
private ComputeBuffer dataBuffer;
private ComputeBuffer objectBuffer;
private ComputeBuffer gatherBuffer;
private float[] gatherValues;
private float[] objectValues;
private float[] dataValues;
private int gather;
private float[] vertValues;

private float[] distances;
private float[] oDistances;

private GameObject[] closestPoints;

public override void GetBuffer(){
  if( buffer == null ){ buffer = GetComponent<TriangleBuffer>(); }
  if( vBuffer == null ){ vBuffer = GetComponent<VertBuffer>(); }


  vertValues = vBuffer.GetValues();
}

public override void AfterLoaded(){

}


public void AddNewTransform( Transform t ){

if( spawnClosest== true ){
  for( int i = 0; i < objectsToIntersect.Count; i++ ){
    Destroy(closestPoints[i]);
  }
}

  objectsToIntersect.Add( t );
if( spawnClosest== true ){
  closestPoints = new GameObject[objectsToIntersect.Count];
  for( int i = 0; i < objectsToIntersect.Count; i++ ){
    closestPoints[i] = Instantiate( closestPointPrefab );
    //closestPoints[i].GetComponent<SetConnection>().t = objectsToIntersect[i];
  }
}
  objectBuffer = new ComputeBuffer( objectsToIntersect.Count, 3*sizeof(float));
  objectValues = new float[3*objectsToIntersect.Count];//c

  dataBuffer = new ComputeBuffer((int)numGroups*objectsToIntersect.Count,4 *sizeof(float));
  gatherBuffer = new ComputeBuffer(objectsToIntersect.Count,4 *sizeof(float));
  gatherValues = new float[4*objectsToIntersect.Count];
  dataValues = new float[4*(int)numGroups*objectsToIntersect.Count];
  print( buffer.count );
  distances = new float[objectsToIntersect.Count];
  oDistances = new float[objectsToIntersect.Count];


  for( int i = 0; i < distances.Length; i++){
    distances[i] = 1000;
    oDistances[i] = 1000;
  }

  print("hellllooo");

}

  public override void FindKernel(){
    kernel = shader.FindKernel( kernelName );
    gather = shader.FindKernel( "Gather" );
  }


public override bool CheckNull(){
  return ( buffer != null && dataBuffer != null && gatherBuffer != null && vBuffer != null  );
}



  public override  void GetNumGroups(){
    numGroups = ((buffer.count/3)+((int)numThreads-1))/(int)numThreads;
  }

  public override void Dispatch(){



    AssignTransform( transform );

    for( int i = 0; i< objectsToIntersect.Count; i++ ){
      objectValues[i * 3 + 0 ]= objectsToIntersect[i].transform.position.x;
      objectValues[i * 3 + 1 ]= objectsToIntersect[i].transform.position.y;
      objectValues[i * 3 + 2 ]= objectsToIntersect[i].transform.position.z;
    }

    objectBuffer.SetData(objectValues);

    shader.SetFloat("_IntersectRadius", intersectionRadius );
    shader.SetInt("_NumTris", buffer.count*3 );
    shader.SetInt("_NumObjects", objectsToIntersect.Count);
    shader.SetBuffer(kernel, "vertBuffer" , vBuffer._buffer );
    shader.SetBuffer(kernel, "triBuffer" , buffer._buffer );
    shader.SetBuffer(kernel, "dataBuffer" , dataBuffer );
    shader.SetBuffer(kernel, "objectBuffer" , objectBuffer );
    shader.Dispatch(kernel,numGroups,1,1);


    shader.SetInt("_NumObjects", objectsToIntersect.Count);
    shader.SetBuffer(gather, "gatherBuffer" , gatherBuffer );
    shader.SetBuffer(gather, "dataBuffer" , dataBuffer );
    shader.Dispatch(gather,numGroups,1,1);
    gatherBuffer.GetData(gatherValues);
    dataBuffer.GetData(dataValues);

    for(int i =0; i < objectsToIntersect.Count; i++ ){
    float x = vertValues[vBuffer.structSize * (int)gatherValues[i*4+1] + 0];
    float y = vertValues[vBuffer.structSize * (int)gatherValues[i*4+1] + 1];
    float z = vertValues[vBuffer.structSize * (int)gatherValues[i*4+1] + 2];

    float nx = vertValues[vBuffer.structSize * (int)gatherValues[i*4+1] + 3];
    float ny = vertValues[vBuffer.structSize * (int)gatherValues[i*4+1] + 4];
    float nz = vertValues[vBuffer.structSize * (int)gatherValues[i*4+1] + 5];


      Vector3 p = new Vector3(x,y,z);
      Vector3 n = new Vector3(nx,ny,nz);

      Vector3 d = p - objectsToIntersect[i].transform.position;

      distances[i] = d.magnitude;

if( spawnClosest == true ){
      closestPoints[i].transform.position = p;
}
//      print( (int)gatherValues[i*4+1] );

      if( OnClosestPoint != null) OnClosestPoint(distances[i],oDistances[i],objectsToIntersect[i],p,n);


      if( oDistances[i] > intersectionRadius && distances[i] < intersectionRadius ){
         if(OnTouchDown != null) OnTouchDown(objectsToIntersect[i],p,n);
      }

      if( oDistances[i] < intersectionRadius && distances[i] > intersectionRadius ){
        if(OnTouchUp != null) OnTouchUp(objectsToIntersect[i],p,n);
      }


        oDistances[i] = distances[i];

    }




  }






}
}



