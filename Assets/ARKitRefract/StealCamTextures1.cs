using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class StealCamTextures1 : MonoBehaviour {


  public UnityARVideo video;

  private Material mat;
	// Use this for initialization
	void OnEnable() {
		mat = GetComponent<MeshRenderer>().material;
    if( video == null ){ video = Camera.main.GetComponent<UnityARVideo>();}
	}
	
	// Update is called once per frame
	void Update () {

    mat = GetComponent<Renderer>().material;
    mat.SetTexture( "_vidTexY" , video.m_ClearMaterial.GetTexture( "_textureY" ) );
    mat.SetTexture( "_vidTexCBCR" , video.m_ClearMaterial.GetTexture( "_textureCbCr" ));

		
	}
}
