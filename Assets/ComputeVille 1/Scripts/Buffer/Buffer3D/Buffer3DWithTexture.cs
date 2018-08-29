using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
public class Buffer3DWithTexture : Buffer3D {

  public Texture3D _texture;

  public Texture3D MakeTexture( float[] values ){

    Color[] bmp = GenerateBitmap(values);
    Texture3D _texture = new Texture3D((int)Dimensions.x, (int)Dimensions.y, (int)Dimensions.z, TextureFormat.RGBAHalf, true);

    _texture.name = "Distance Field Texture";
    _texture.filterMode = FilterMode.Trilinear;
    _texture.wrapMode = TextureWrapMode.Clamp;
    _texture.SetPixels(bmp);
    _texture.Apply();

    return _texture;

  }

  public Color[] GenerateBitmap(float[] values){

    Color[] col = new Color[count];
    for( int i = 0; i < count; i++ ){
      col[i] = new Color( values[ i * structSize + 0 ],
                          values[ i * structSize + 1 ],
                          values[ i * structSize + 2 ],
                          values[ i * structSize + 3 ]);
    }


    return col;

  }

  public void RemakeTexture(){
    GetData();
    _texture = MakeTexture(values);
  }


  public override void OnLoaded(){
    RemakeTexture();
  }




}
}
