using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(Buffer))]
public class ProceduralRender : MonoBehaviour {

  public Material material;
  protected Buffer buffer;

  void OnEnable(){
    GetBuffer();
    material = new Material(material);
  }

  void OnRenderObject(){
    if( CheckNull() ){
      Render();
    }
  }
  public virtual void Render(){}
  public virtual void GetBuffer(){
    buffer = GetComponent<Buffer>();
  }
  public virtual bool CheckNull(){
    return ( buffer._buffer != null );
  }

}
}
