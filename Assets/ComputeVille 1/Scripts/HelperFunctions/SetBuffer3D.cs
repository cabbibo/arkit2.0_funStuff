using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComputeVille;

public class SetBuffer3D : MonoBehaviour {

  public Buffer3DWithTexture sdf;
  private Material mat;

    // Use this for initialization
    void Start () {

    mat = GetComponent<MeshRenderer>().material;
//    mat = new Material( mat );

    }

    // Update is called once per frame
    void Update () {

//      print( sdf.dimensions.x );
      if( sdf._buffer != null ){
//        print("hell0");
        mat.SetMatrix( "_SDFTransform" , sdf.gameObject.transform.worldToLocalMatrix );
        mat.SetVector( "_Dimensions", sdf.Dimensions);
        mat.SetVector( "_Extents", sdf.Extents);
        mat.SetVector( "_Center", sdf.Center);
        mat.SetBuffer("_volumeBuffer" , sdf._buffer);
        mat.SetTexture("_MainTex", sdf._texture);
      }else{
        print("hey");
      }
    }

}
