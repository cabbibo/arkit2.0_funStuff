using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHeightFromOther : MonoBehaviour {

  public Transform other;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.position = other.position;
		
	}
}
