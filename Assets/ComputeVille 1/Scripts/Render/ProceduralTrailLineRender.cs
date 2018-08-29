using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class ProceduralTrailLineRender : ProceduralTrailRender {

  public override void Render(){
    material.SetPass(0);
    material.SetBuffer("_vertBuffer", buffer._buffer);
    material.SetInt("_Count",buffer.particles.count);
    material.SetInt("_VertsPerParticle",buffer.vertsPerParticle);

    Graphics.DrawProcedural(MeshTopology.Triangles, buffer.particles.count * (buffer.vertsPerParticle-1) * 3 * 2 );
  }

}
}
