using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osscilate : MonoBehaviour {
    public float size;
    public float speed;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

    transform.position += size * Vector3.left * Mathf.Sin( speed * Time.time * 1) * .01f;
    transform.position += size * Vector3.up * Mathf.Sin( speed * Time.time * 1.3f +2) * .013f;
    transform.position += size * Vector3.forward * Mathf.Sin( speed * Time.time * 1.5f + 1) * .015f;
    //transform.LookAt( target );

	}
}
