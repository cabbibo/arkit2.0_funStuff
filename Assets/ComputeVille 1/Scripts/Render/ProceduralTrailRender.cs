
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(TrailBuffer))]
public class ProceduralTrailRender : MonoBehaviour {

  public Material material;
  protected TrailBuffer buffer;

  void OnEnable(){
    GetBuffer();
  }

  void OnRenderObject(){
    if( CheckNull() ){
      Render();
    }
  }
  public virtual void Render(){}
  public virtual void GetBuffer(){
    buffer = GetComponent<TrailBuffer>();
  }
  public virtual bool CheckNull(){
    return ( buffer._buffer != null );
  }

}
}
