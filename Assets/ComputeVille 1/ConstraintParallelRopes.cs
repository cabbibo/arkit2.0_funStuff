using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ConstraintParallelRopes : Physics {

  public RopeVertBuffer rope1;
  public RopeVertBuffer rope2;
  public float SpringDistance;


  public override void GetBuffer(){
    buffer = rope1;
  }

  public override void SetShaderValues(){
    shader.SetBuffer(kernel , "rope1" , rope1._buffer );
    shader.SetBuffer(kernel , "rope2" , rope2._buffer );
    shader.SetFloat(  "_SpringDistance" , SpringDistance );
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}
