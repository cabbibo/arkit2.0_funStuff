using UnityEngine;
using System.Collections;


namespace ComputeVille{
public class DataBuffer : FloatBuffer {

  struct TriIds{
    public Vector3 triIDs;
    public float  distance;
  };


  public override void SetStructSize(){
    structSize = 3+1;
  }

}
}
