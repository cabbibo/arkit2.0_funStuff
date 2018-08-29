
Shader "PostTransfer/procedural" {
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

		  uniform int _VertCount;

struct Vert{
  float3 pos;
  float3 nor;
  float3 tan;
  float2 uv;
  float debug;
};





      StructuredBuffer<Vert> _vertBuffer;
      StructuredBuffer<int> _triBuffer;


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

        int triID = _triBuffer[id];

        if( triID < _VertCount ){

        	Vert v = _vertBuffer[triID];

 

       		o.worldPos = v.pos;///* .001/(.1+length(v.debug));//*(1/(.1+max(length(v.debug),0)));//mul( worldMat , float4( v.pos , 1.) ).xyz;
	        o.debug = v.tan;//float3(float(id)/1000000,1,0);
	        o.eye = _WorldSpaceCameraPos - o.worldPos;
          o.nor = normalize( v.nor);//fPos;
          o.uv = v.uv;

	        o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));
	      }

        return o;

      }




      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      		float3 col = normalize(v.nor) * .8 + .5;
          return float4( col, 1 );

      }

      ENDCG

    }
  }

  Fallback Off


}
