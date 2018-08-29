using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsscilateLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    transform.Rotate( Vector3.up + Vector3.left , .2f*Mathf.Sin(Time.time));
		
	}
}
