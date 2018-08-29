using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
[RequireComponent(typeof(VerletVertBuffer))]
public class VerletPhysics : Physics {

  public delegate void BeforeConstraintDispatch(ComputeShader shader, int kernel);
  public event BeforeConstraintDispatch OnBeforeConstraintDispatch;
  public delegate void AfterConstraintDispatch(ComputeShader shader, int kernel);
  public event AfterConstraintDispatch OnAfterConstraintDispatch;

  public string kernelConstraintName = "Constraint";

  protected uint numThreadsConstraint;
  protected int numGroupsConstraint;
  protected int kernelConstraint;

  public override void GetBuffer(){ buffer = GetComponent<VerletVertBuffer>(); }

  public override void UpdatePhysics(){
//    print("hmmm");
    if( OnBeforeConstraintDispatch != null ){ OnBeforeConstraintDispatch(shader,kernel); }
    ConstraintDispatch(0);
    ConstraintDispatch(1);
    if( OnAfterConstraintDispatch != null ){ OnAfterConstraintDispatch(shader,kernel); }
  }
 

  public override void FindKernel(){
    kernel = shader.FindKernel( kernelName );
    kernelConstraint = shader.FindKernel( kernelConstraintName );
  }

  public override void GetNumThreads(){
    uint y; uint z;
    shader.GetKernelThreadGroupSizes(kernel, out numThreads , out y, out z);
    shader.GetKernelThreadGroupSizes(kernel, out numThreadsConstraint , out y, out z);
  }

  public override void GetNumGroups(){
    numGroups = (buffer.count+((int)numThreads-1))/(int)numThreads;
    numGroupsConstraint = (buffer.count+((int)numThreadsConstraint-1))/(int)numThreads;
  }

  public virtual void ConstraintDispatch(int whichPass){
    shader.SetInt("_PassID", whichPass);
    shader.SetBuffer(kernelConstraint, "vertBuffer" , buffer._buffer );
    shader.Dispatch(kernelConstraint,numGroups,1,1);
  }

}
}