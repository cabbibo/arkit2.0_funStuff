using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(TransferVertBuffer))]
[RequireComponent(typeof(TransferTriBuffer))]
public class TransferMeshAlongCurve : Physics {

public Material material;

  protected TransferTriBuffer triBuffer;

  public VerletVertBuffer cBuffer;
  public Basic12VertBuffer  vertBuffer;

  public Transform Start;
  public Transform End;

  public MakeCliff cliffStart;
  public MakeCliff cliffEnd;

  public float modelLength;
  public MeshRenderer renderer;



/*
    The transfer buffers take the particle buffer
    and assign the positions of the verts for
    every particle!

    The particles (whose movements are defined by physics etc. )
    are in particleBuffer

    The verts we assign for the FINAL MESH are in buffer ( a TransferVertBuffer )
    The tris we assign

*/
  public override void GetBuffer(){
    if( buffer == null ){ buffer = GetComponent<TransferVertBuffer>(); }
    if( triBuffer == null ){ triBuffer = GetComponent<TransferTriBuffer>(); }
    material.SetBuffer("_transferBuffer", buffer._buffer);
//   renderer.material.SetBuffer("_transferBuffer", buffer._buffer);
  }

  public override bool CheckNull(){
    return ( buffer._buffer != null && triBuffer._buffer != null && cBuffer._buffer != null);
  }


  public override void Dispatch(){
//    print( modelLength );

    shader.SetInt("_Count", buffer.count );
    shader.SetInt("_CurveCount", cBuffer.count );
    shader.SetFloat("_ModelLength", modelLength );
    shader.SetVector("_Start", Start.position );
    shader.SetVector("_End",End.position );
    shader.SetFloat("_StartWidth",cliffStart.fWidth );
    shader.SetFloat("_EndWidth",cliffEnd.fWidth);
    shader.SetBuffer(kernel, "transferBuffer" , buffer._buffer );
    shader.SetBuffer(kernel, "vertBuffer" , vertBuffer._buffer );
    shader.SetBuffer(kernel, "curveBuffer" , cBuffer._buffer );
    shader.Dispatch(kernel,numGroups,1,1);
  }


public override void AfterLoaded(){
    MakeMesh();
}

void Update(){
    //material.SetBuffer("_transferBuffer", buffer._buffer);
    if( renderer != null ){ renderer.material.SetBuffer("_transferBuffer", buffer._buffer);}
}

public virtual void MakeMesh(){

  material = new Material( material);
        if( buffer.count == 0){ print("BUFFER NOT MADE YET");}
        if( triBuffer.count == 0){ print("Triangles NOT MADE YET");}


        print( vertBuffer.count );
        print( buffer.count );
        print( vertBuffer.GetValues().Length );
        print( ((FloatBuffer)buffer).GetValues().Length );
        if( gameObject.GetComponent<MeshFilter>() == null ){
        Mesh mesh = new Mesh ();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = new Vector3[buffer.count];
        int[] tris = triBuffer.GetValues();
        uint[] utris = new uint[tris.Length];//triBuffer.GetValues();
      

        int max = 0;
        for( int i = 0; i < tris.Length; i++ ){
          utris[i] = (uint)tris[i];
          if( tris[i] > max ){
            max = tris[i];
          }
        }

        mesh.SetTriangles(tris,0);//.triangles = tris;
        
        //
        mesh.bounds = new Bounds (Vector3.zero, Vector3.one * 1000f);
        //



        gameObject.AddComponent<MeshFilter>().mesh = mesh;


        renderer = gameObject.AddComponent<MeshRenderer> ();
        renderer.sharedMaterial = material;

        material.SetBuffer("_transferBuffer", buffer._buffer);
    }

}

}}