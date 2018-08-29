using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class TransferTriBuffer :  IntBuffer {

  public override void BeforeBuffer(){
    GetBuffer();
  }

  public override void AfterBuffer(){
    MakeMesh();
    SetData();
  }

  // This function you will override to assign which triangles point to which verts
  public virtual void MakeMesh(){}

  public virtual void GetBuffer(){}
  public override void SetStructSize(){ structSize = 1; }

}
}
