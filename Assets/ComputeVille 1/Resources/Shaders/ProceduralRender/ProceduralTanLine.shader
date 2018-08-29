Shader "ProceduralRender/ProceduralTan" {
	Properties {
		}


  SubShader{
//        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
    Cull Off
    Pass{

      //Blend SrcAlpha OneMinusSrcAlpha // Alpha blending

      CGPROGRAM
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "../Chunks/noise.cginc"
      #include "../Chunks/hsv.cginc"

		  uniform int _Count;
      uniform float3 _Color;

			  struct Vert{
			   	float3 pos;
			   	float3 oPos;
			   	float3 nor;

          float3 tangent;
          float3 bitangent;
			   	float idUp;
			   	float idDown;
			   	float2 uv;
			   	float3 debug;
			  };


      StructuredBuffer<Vert> _vertBuffer;


      //uniform float4x4 worldMat;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos      : SV_POSITION;
          float3 nor      : TEXCOORD0;
          float3 worldPos : TEXCOORD1;
          float3 eye      : TEXCOORD2;
          float3 debug    : TEXCOORD3;
          float2 uv       : TEXCOORD4;
          float  noiseVal : TEXCOORD5;
      };


      //Our vertex function simply fetches a point from the buffer corresponding to the vertex index
      //which we transform with the view-projection matrix before passing to the pixel program.
      varyings vert (uint id : SV_VertexID){

        varyings o;

        int pID = id /2;
        int offset = id % 2;	
        int fID = pID;// + offset;

        if( fID < _Count ){

        	Vert v = _vertBuffer[fID];

       		o.worldPos = v.pos;

       		if( offset == 1 ){
       			o.worldPos = v.pos + v.tangent * .03;
       		}

       		///* .001/(.1+length(v.debug));//*(1/(.1+max(length(v.debug),0)));//mul( worldMat , float4( v.pos , 1.) ).xyz;
	        o.debug =float3(float(id)/1000000,1,0);
	        
	        o.eye = _WorldSpaceCameraPos - o.worldPos;
          o.nor =normalize( v.nor);//fPos;
          o.uv = v.uv;

	        o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

       	}

        return o;

      }




      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      		float3 col = normalize(v.nor) * .5 + .5;
          col =  float3(1,0,0);//sin(v.uv.x*100) + sin(v.uv.y*100);
          return float4( col, 1 );

      }

      ENDCG

    }
  }

  Fallback Off


}
