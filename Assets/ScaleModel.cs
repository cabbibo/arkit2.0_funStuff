using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModel : MonoBehaviour {


  public LockToImage start;
  public LockToImage end;
  public GameObject Model;
  public Vector3 startScale;
  public float scaleMultiplier;

  public TextMesh text;
	// Use this for initialization
	void Start () {

    startScale = Model.transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {

    float dif = (start.transform.position - end.transform.position).magnitude;
    if( start.added == true  && end.added == true ){
      Model.transform.position = start.transform.position;
      Model.transform.localScale = dif * scaleMultiplier * startScale;
      Model.transform.LookAt( Model.transform.position + (end.transform.position - start.transform.position) );
      text.text = "Scale: " + Mathf.Floor( Model.transform.localScale.x * 1000 ) / 1000;
    }else{
    }
		
	}
}
