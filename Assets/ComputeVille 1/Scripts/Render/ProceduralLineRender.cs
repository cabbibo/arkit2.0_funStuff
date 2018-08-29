using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(Buffer))]
public class ProceduralLineRender : ProceduralRender {

  public override void Render(){
    material.SetPass(0);
    material.SetBuffer("_vertBuffer", buffer._buffer);
    material.SetInt("_Count",buffer.count);
    SetShaderValues();
    Graphics.DrawProcedural(MeshTopology.Lines, TotalDrawCount() );
  }

  public virtual void SetShaderValues(){}

  public virtual int TotalDrawCount(){
    return (buffer.count-1) * 2;
  }


}
}
