using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ComputeVille{
public class SaveableFloatBuffer : FloatBuffer {

  public string SaveName;
  public bool loadedFromFile;
  public bool alwaysNew;

  public void Save(){
    string name = GetName();
    BinaryFormatter bf = new BinaryFormatter();
    FileStream stream = new FileStream(name,FileMode.Create);
    float[] valuesToSave = GetValuesToSave();
    bf.Serialize(stream,valuesToSave);
    stream.Close();
  }


  public void Load(){

    if( alwaysNew == false ){
      string name = GetName();
      if( File.Exists(name)){
         print("LOADING");
         BinaryFormatter bf = new BinaryFormatter();
         FileStream stream = new FileStream(name,FileMode.Open);

          float[] data = bf.Deserialize(stream) as float[];
          loadedFromFile = true;
          OnLoad(data,loadedFromFile);

          stream.Close();

      }else{
        NewLoad();
      }
    }else{
      NewLoad();
    }

  }

  public void NewLoad(){
    loadedFromFile = false;
    float[] d = new float[1];
    OnLoad(d,loadedFromFile);
  }

  public void OnLoad( float[] data , bool fromFile ){

    if( fromFile == true ){
      values =  GetValuesFromSave(data);
      SetData();
      OnLoaded();
    }else{
      CreateNewValues();
    }
  }


  public virtual void CreateNewValues(){}
  public virtual void OnLoaded(){}

  public virtual float[] GetValuesToSave(){
    GetData();
    return values;
  }

  public virtual string GetName(){
    return Application.dataPath + "/"+SaveName + ".buffer";
  }

  public virtual float[] GetValuesFromSave(float[] data){
    return data;
  }

  public override void SetCount(){}


}
}
