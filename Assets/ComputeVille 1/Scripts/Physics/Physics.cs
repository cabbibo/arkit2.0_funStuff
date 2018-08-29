using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
[RequireComponent(typeof(Buffer))]
public class Physics : MonoBehaviour {


  public delegate void BeforeDispatch(ComputeShader shader, int kernel);
  public event BeforeDispatch OnBeforeDispatch;
  public delegate void AfterDispatch(ComputeShader shader, int kernel);
  public event AfterDispatch OnAfterDispatch;


  public ComputeShader shader;
  protected Buffer buffer;
  public string kernelName = "CSMain";
  public bool update;

  protected uint numThreads;
  protected int numGroups;
  protected int kernel;

  void OnEnable(){
    GetBuffer();
    CastBuffer();
    GetInfo();
    AfterLoaded();
  }

  void GetInfo(){
    FindKernel();
    GetNumThreads();
    GetNumGroups();
  }


  void LateUpdate(){
    if( CheckNull() && update == true ){
      _UpdatePhysics();
    }
  }

  private void _UpdatePhysics(){
    GetNumGroups();
    if( OnBeforeDispatch != null ){ OnBeforeDispatch(shader,kernel); }
    Dispatch();
    if( OnAfterDispatch != null ){ OnAfterDispatch(shader,kernel); }
    UpdatePhysics();
  }
  public virtual void UpdatePhysics(){

  }
  public virtual void CastBuffer(){}
  public virtual void AfterLoaded(){}

  public virtual void GetBuffer(){
    buffer = GetComponent<Buffer>();
  }
  public virtual bool CheckNull(){
    return ( buffer._buffer != null );
  }

  public virtual void FindKernel(){
    kernel = shader.FindKernel( kernelName );
  }

  public virtual void GetNumThreads(){
    uint y; uint z;
    shader.GetKernelThreadGroupSizes(kernel, out numThreads , out y, out z);
  }

  public virtual void GetNumGroups(){
    numGroups = (buffer.count+((int)numThreads-1))/(int)numThreads;
  }


  protected void AssignTransform(Transform t){

    Matrix4x4 m = t.localToWorldMatrix;

    float[] matrixFloats = new float[]
    {
    m[0,0], m[1, 0], m[2, 0], m[3, 0],
    m[0,1], m[1, 1], m[2, 1], m[3, 1],
    m[0,2], m[1, 2], m[2, 2], m[3, 2],
    m[0,3], m[1, 3], m[2, 3], m[3, 3]
    };

    shader.SetFloats("_Transform", matrixFloats);

  }


  protected void AssignInverseTransform(Transform t){

    Matrix4x4 m = t.worldToLocalMatrix;

    float[] matrixFloats = new float[]
    {
    m[0,0], m[1, 0], m[2, 0], m[3, 0],
    m[0,1], m[1, 1], m[2, 1], m[3, 1],
    m[0,2], m[1, 2], m[2, 2], m[3, 2],
    m[0,3], m[1, 3], m[2, 3], m[3, 3]
    };

    shader.SetFloats("_InverseTransform", matrixFloats);

  }

  public virtual void SetShaderValues(){}




  public virtual void Dispatch(){

//    print("hmmmm3");
    AssignTransform( transform );
    shader.SetFloat("_Time", Time.time);
    shader.SetFloat("_Delta", Time.deltaTime);
    shader.SetInt("_Count", buffer.count );
    shader.SetBuffer(kernel, "vertBuffer" , buffer._buffer );
    SetShaderValues();
    shader.Dispatch(kernel,numGroups,1,1);
  }


}
}
