using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{

[RequireComponent(typeof(MarchingCubes))]
public class MarchingCubesMeshMaker : MonoBehaviour {

  public MarchingCubes marchingCubes;
  public Material material;

  public void OnEnable(){
    if( marchingCubes == null ){ marchingCubes = GetComponent<MarchingCubes>();}
  }

  public void MakeMesh(){

    marchingCubes.UpdatePhysics();

    float[] values = marchingCubes.verts.GetNewValues();

    //Extract the positions, normals and indexes.
    List<Vector3> positions = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<int> indices = new List<int>();

    int index = 0;
    for( int i  = 0; i < marchingCubes.verts.count; i++ ){

      if( values[ i * 6 + 0 ] != -1 ){
        Vector3 p = new Vector3( values[ i * 6 + 0 ] ,values[ i * 6 + 1 ],values[ i * 6 + 2 ]);
        Vector3 n = -1 * new Vector3( values[ i * 6 + 3 ] ,values[ i * 6 + 4 ],values[ i * 6 + 5 ]);

        if( index < 100 ){
          print( n );
        }

        positions.Add( p );
        normals.Add( n );
        indices.Add( index);
        index++;
      }

       int maxTriangles = 65000 / 3;

        if(index >= maxTriangles){
          MakeGameObject(positions, normals, indices );
          index = 0;
          positions.Clear();
          normals.Clear();
          indices.Clear();
        }

    }

    MakeGameObject(positions, normals, indices);

  }

  void MakeGameObject(List<Vector3> positions, List<Vector3> normals, List<int> indices){


    GameObject go = new GameObject("Voxel Mesh");
    Mesh mesh = new Mesh();

    mesh.vertices = positions.ToArray();
    mesh.normals = normals.ToArray();
    mesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(100, 100, 100));
    mesh.SetTriangles(indices.ToArray(), 0);

    go.AddComponent<MeshFilter>();
    go.AddComponent<MeshRenderer>();
    go.GetComponent<Renderer>().material =  material;//new Material(Shader.Find("Custom/CelShadingForward"));
    go.GetComponent<MeshFilter>().mesh = mesh;
    go.transform.parent = transform;

  }
}
}
