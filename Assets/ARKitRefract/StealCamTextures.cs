using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class StealCamTextures : MonoBehaviour {


  public UnityARVideo video;

  private Material mat;
	// Use this for initialization
	void Start () {
		mat = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {


    mat.SetTexture( "_vidTexY" , video.m_ClearMaterial.GetTexture( "_textureY" ) );
    mat.SetTexture( "_vidTexCBCR" , video.m_ClearMaterial.GetTexture( "_textureCbCr" ));

		
	}
}
