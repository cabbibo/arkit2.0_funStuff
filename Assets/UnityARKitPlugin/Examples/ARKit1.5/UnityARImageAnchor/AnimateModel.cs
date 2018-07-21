using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateModel : MonoBehaviour {


  public LockToImage start;
  public LockToImage end;
  public GameObject Model;
  public AnimationClip clip;
  public Animator animator;
  public float speedMultiplier;
  public float loopOffset;

  public TextMesh text;
  // Use this for initialization
  void Start () {

    clip = animator.runtimeAnimatorController.animationClips[0];

  }
  
  // Update is called once per frame
  void Update () {

    float dif = (start.transform.position - end.transform.position).magnitude;
    if( start.added == true  && end.added == true ){
      text.text = "Frame: " + (Mathf.Floor(( dif * speedMultiplier ) * clip.frameRate) % ( clip.frameRate  * clip.length)).ToString("0");
      Model.transform.LookAt( Model.transform.position - start.transform.position + end.transform.position );
      animator.Play("mixamo_com",0,dif*speedMultiplier % clip.length);
      Model.transform.position =start.transform.position + Mathf.Floor( dif * speedMultiplier / (float)clip.length) * ( - start.transform.position + end.transform.position ).normalized * loopOffset;

    }else{
    }
    
  }
}