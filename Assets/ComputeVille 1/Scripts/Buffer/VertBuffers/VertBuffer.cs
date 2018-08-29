using UnityEngine;
using System.Collections;


namespace ComputeVille{
public class VertBuffer : FloatBuffer {

  public override void BeforeBuffer(){
    GetVerts();
  }

  public override void AfterBuffer(){
    SetOriginalValues();
    SetData();
  }


  public virtual void SetOriginalValues(){}
  public virtual void GetVerts(){}
}
}
