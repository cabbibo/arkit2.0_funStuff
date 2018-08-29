﻿using UnityEngine;
using System.Collections;


namespace ComputeVille{
// should be on the object that is moving around the scen
public class BoneBuffer : FloatBuffer {

  public SkinnedMeshRenderer mesh;
  public Transform[] bones;
  public Matrix4x4[] bindPoses;

  struct Bone{
    public Matrix4x4 transform;
    public Matrix4x4 bindPose;
  };

  public Matrix4x4 tmpMat;

  private bool updated = false;

  public override void BeforeBuffer(){
    GetMesh();
    count = bones.Length;
    SetStructSize();
  }

  public override void SetStructSize(){ structSize = 16 * 2; }

  public virtual void SetOriginalValues(){}


  public virtual void GetMesh(){

    if( mesh == null){
      mesh = gameObject.GetComponent<SkinnedMeshRenderer>();
    }


    bones = mesh.bones;
    bindPoses = mesh.sharedMesh.bindposes;

  }

  // Update is called once per frame
  void FixedUpdate () {
    UpdateBoneBuffer();
  }






  void UpdateBoneBuffer(){
    for( int i = 0; i < count; i++){

      tmpMat = mesh.bones[i].localToWorldMatrix; //bones[i].localToWorldMatrix;

      values[ i * 32 + 0] = tmpMat[0,0];
      values[ i * 32 + 1] = tmpMat[1,0];
      values[ i * 32 + 2] = tmpMat[2,0];
      values[ i * 32 + 3] = tmpMat[3,0];
      values[ i * 32 + 4] = tmpMat[0,1];
      values[ i * 32 + 5] = tmpMat[1,1];
      values[ i * 32 + 6] = tmpMat[2,1];
      values[ i * 32 + 7] = tmpMat[3,1];
      values[ i * 32 + 8] = tmpMat[0,2];
      values[ i * 32 + 9] = tmpMat[1,2];
      values[ i * 32 +10] = tmpMat[2,2];
      values[ i * 32 +11] = tmpMat[3,2];
      values[ i * 32 +12] = tmpMat[0,3];
      values[ i * 32 +13] = tmpMat[1,3];
      values[ i * 32 +14] = tmpMat[2,3];
      values[ i * 32 +15] = tmpMat[3,3];


//      Quaternion rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);



   //   Matrix4x4 m = Matrix4x4.TRS(translation, rotation, 1/scale);


      var rotation = Matrix4x4.Rotate(mesh.bones[i].rotation);
      tmpMat = bindPoses[i];//  qadfgh;'' * rotation.inverse * rotation.inverse;

      values[ i * 32 +16] = tmpMat[0,0];
      values[ i * 32 +17] = tmpMat[1,0];
      values[ i * 32 +18] = tmpMat[2,0];
      values[ i * 32 +19] = tmpMat[3,0];
      values[ i * 32 +20] = tmpMat[0,1];
      values[ i * 32 +21] = tmpMat[1,1];
      values[ i * 32 +22] = tmpMat[2,1];
      values[ i * 32 +23] = tmpMat[3,1];
      values[ i * 32 +24] = tmpMat[0,2];
      values[ i * 32 +25] = tmpMat[1,2];
      values[ i * 32 +26] = tmpMat[2,2];
      values[ i * 32 +27] = tmpMat[3,2];
      values[ i * 32 +28] = tmpMat[0,3];
      values[ i * 32 +29] = tmpMat[1,3];
      values[ i * 32 +30] = tmpMat[2,3];
      values[ i * 32 +31] = tmpMat[3,3];


    }

    SetData();

  }


}
}
