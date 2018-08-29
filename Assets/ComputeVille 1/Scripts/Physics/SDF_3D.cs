using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(BufferSDF))]
public class SDF_3D : Physics3D {


public override void GetBuffer(){
if( buffer == null ){ buffer = GetComponent<BufferSDF>(); }
}

public override void Dispatch(){
    print("hm");
//shader.SetVector("_Dimensions", buffer.dimensions );
shader.SetInt("_VolDim", (int)buffer3D.Dimensions.x );
shader.SetVector("_Dimensions", buffer3D.Dimensions);
shader.SetVector("_Extents", buffer3D.Extents );
shader.SetBuffer(kernel, "volumeBuffer" , buffer._buffer );
shader.Dispatch(kernel,numGroups,1,1);
}

}}
