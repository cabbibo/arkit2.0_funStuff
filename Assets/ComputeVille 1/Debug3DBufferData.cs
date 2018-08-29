using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComputeVille;

public class Debug3DBufferData : MonoBehaviour {

    public FloatBuffer buffer;

	// Use this for initialization
	void Start () {

        if( buffer == null ){ GetComponent<FloatBuffer>();}

	}

	// Update is called once per frame
	void Update () {

	}

    public void LogValues(){
        //buffer.GetData();

        buffer.SetData();
        print("first");
        float[] values = buffer.GetValues();
        for( int i = 0; i < values.Length; i+=8 ){
            print( values[i] );
        }

        buffer.GetData();
        print("second");
        values = buffer.GetValues();
        for( int i = 0; i < values.Length; i+=8 ){
            print( values[i] );
        }

    }
}
