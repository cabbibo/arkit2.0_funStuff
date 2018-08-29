using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(Buffer))]
[RequireComponent(typeof(TransferVertBuffer))]
[RequireComponent(typeof(TransferTriBuffer))]
public class MeshFromTransferBuffers : Physics {

public Material material;

protected Buffer particleBuffer;
protected TransferTriBuffer triBuffer;

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
    if( particleBuffer == null ){ particleBuffer = GetComponent<VertBuffer>(); }
    material.SetBuffer("_transferBuffer", buffer._buffer);
  }

  public override bool CheckNull(){
    return ( buffer._buffer != null && triBuffer._buffer != null && particleBuffer._buffer != null );
  }


  public override void Dispatch(){
    shader.SetInt("_Count", buffer.count );
    shader.SetVector("_CameraRight", Camera.main.gameObject.transform.right );
    shader.SetVector("_CameraUp", Camera.main.gameObject.transform.up );
    shader.SetBuffer(kernel, "transferBuffer" , buffer._buffer );
    shader.SetBuffer(kernel, "vertBuffer" , particleBuffer._buffer );
    shader.Dispatch(kernel,numGroups,1,1);
  }


public override void AfterLoaded(){
    MakeMesh();
}

void Update(){
    material.SetBuffer("_transferBuffer", buffer._buffer);
}

public virtual void MakeMesh(){

        if( buffer.count == 0){ print("BUFFER NOT MADE YET");}
        if( triBuffer.count == 0){ print("Triangles NOT MADE YET");}


        if( gameObject.GetComponent<MeshFilter>() == null ){
        Mesh mesh = new Mesh ();
        mesh.vertices = new Vector3[buffer.count];
        mesh.triangles =  triBuffer.GetValues();
        mesh.bounds = new Bounds (Vector3.zero, Vector3.one * 1000f);
        mesh.UploadMeshData (true);


        gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;


        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer> ();
        renderer.sharedMaterial = material;

        material.SetBuffer("_transferBuffer", buffer._buffer);
    }

}

}}
