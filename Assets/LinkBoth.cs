using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBoth : MonoBehaviour {

  public LockToImage start;
  public LockToImage end;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    if( start.added == true && end.added == false ){
      end.transform.position = start.transform.position + Vector3.forward * .1f;
    }


    if( end.added == true &&start.added == false ){
     start.transform.position = end.transform.position + Vector3.forward * .1f;
    }

    if( end.added == false && start.added == false ){
     start.transform.position = Camera.main.transform.position; 
     end.transform.position = Camera.main.transform.position + Vector3.forward * .1f;
    }


		
	}
}
