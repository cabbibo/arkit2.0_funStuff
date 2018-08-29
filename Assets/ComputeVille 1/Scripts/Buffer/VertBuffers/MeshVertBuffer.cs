using UnityEngine;
using System.Collections;


namespace ComputeVille{
public class MeshVertBuffer : VertBuffer {

  public bool rotateMesh;
  public bool scaleMesh;
  public bool translateMesh;

  public Mesh mesh;

  protected Vector3[] vertices;
  protected Vector3[] normals;
  protected Vector2[] uvs;

  public override void GetVerts(){
    GetMesh();
  }

  public virtual void GetMesh(){

    if( mesh == null){
      mesh = gameObject.GetComponent<MeshFilter>().mesh;
    }

    vertices = mesh.vertices;
    normals = mesh.normals;
    uvs = mesh.uv;

  }

  public override void SetCount(){
    count = vertices.Length;
  }
}
}
