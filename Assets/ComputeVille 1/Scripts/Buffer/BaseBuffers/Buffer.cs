using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ComputeVille{
public class Buffer : MonoBehaviour {

  public int count;
  public ComputeBuffer _buffer;
  public int structSize;

  void OnEnable(){
    BeforeBuffer();
    SetStructSize();
    SetCount();
    CreateBuffer();
    AfterBuffer();
  }


  void OnDisable(){
    ReleaseBuffer();
  }

  public virtual void CreateBuffer(){}

  public void ReleaseBuffer(){
   if(_buffer != null){ _buffer.Release(); }
  }


  public virtual void BeforeBuffer(){}
  public virtual void AfterBuffer(){}

  public virtual void SetStructSize(){}
  public virtual void SetCount(){}



}
}
