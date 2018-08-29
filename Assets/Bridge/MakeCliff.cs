using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCliff : MonoBehaviour {

	public float height;
	public float width;

	public bool scaleOnRotation;

	public GameObject OtherCliff;
	public GameObject Cliff;
  public GameObject CliffTop;
  public float fWidth;
  public float fHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if( scaleOnRotation == true ){
			float scaler = .5f*((transform.parent.rotation.eulerAngles.y%360)/360)+.5f;
			print( scaler );
			fHeight = scaler * height;
			fWidth = scaler * width;
		}else{
			fHeight = height;
			fWidth = width;
		}


		Vector3 dif = OtherCliff.transform.position - transform.position;
		CliffTop.transform.position = transform.position + Vector3.up * fHeight+ dif.normalized * fWidth * .5f;// transform.position.x , transform.position.y + fHeight , tra)
		Cliff.transform.position = transform.position + Vector3.up * fHeight * .5f;// - dif.normalized * fWidth * .5f;
		Cliff.transform.localScale = new Vector3( fWidth , fHeight , fWidth );

		transform.LookAt( OtherCliff.transform.position );
		//transform.position += Cliff.transform.localPosition;

	}
}
