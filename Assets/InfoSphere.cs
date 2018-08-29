using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSphere : MonoBehaviour {

  public string title;
  public string info;
  public AudioClip clip;
  public Color color;

  void Start(){
    GetComponent<MeshRenderer>().material.SetColor("_Color", color);
  }

}
