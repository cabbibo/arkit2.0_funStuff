using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCliffOffset : MonoBehaviour {

  public Transform cliff;// position
  public Transform basePos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.position = new Vector3( cliff.position.x , basePos.position.y , cliff.position.z );
		
	}
}
