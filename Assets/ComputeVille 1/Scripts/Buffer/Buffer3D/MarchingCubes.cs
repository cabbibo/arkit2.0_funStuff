using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{


[RequireComponent(typeof(BufferSDF))]
[RequireComponent(typeof(MarchingCubesVertBuffer))]
public class MarchingCubes : Physics3D {

    public BufferSDF sdf;
    public MarchingCubesVertBuffer verts;

    public override bool CheckNull(){
        return ( sdf._buffer != null && verts._buffer != null );
    }

    public override void GetBuffer(){
        if( sdf == null ){ sdf = GetComponent<BufferSDF>(); }
        if( verts == null ){ verts = GetComponent<MarchingCubesVertBuffer>(); }
    }

}
}
