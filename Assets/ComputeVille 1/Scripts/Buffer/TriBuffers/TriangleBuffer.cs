using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ComputeVille{
public class TriangleBuffer : IntBuffer {

  public override void BeforeBuffer(){
    structSize = 1;
    GetTriangles();
  }

  public override void AfterBuffer(){
    SetData();
  }

  // making own triangle array
  public override void MakeValueArray(){}
  public virtual void GetTriangles(){}


}
}
