Shader "Custom/debugShaderTest" {
		Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Main Color", Color) = (1,1,1,1)
  }
  SubShader{

  	


    Cull Back
    //zWrite off
    Pass{


      //Blend One One // Additive
      CGPROGRAM
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"





      struct Vert{
      	float used;
        float3 pos;
        float3 debug;
      };

      StructuredBuffer<Vert> _vertBuffer;// : register(u1);

      uniform sampler2D _MainTex;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos 			: SV_POSITION;
          float3 nor 			: TEXCOORD0;
          float2 uv  			: TEXCOORD1;
          float3 eye      : TEXCOORD5;
          float3 worldPos : TEXCOORD6;
          float3 debug    : TEXCOORD7;

      };


      varyings vert ( uint id : SV_VertexID ){

        varyings o;

        int bID = id / 6;

        int tri = id % 6;

       	Vert v = _vertBuffer[bID];

       	float3 fPos = float3(0,0,0);//mul(float4(0,0,0,1),b.bindPose ).xyz;

       	fPos = v.pos;//mul( b.transform, float4(fPos,1) ).xyz;
       	
        float2 fUV = float2(0,0);
				float3 up = UNITY_MATRIX_IT_MV[0].xyz;
				float3 ri = UNITY_MATRIX_IT_MV[1].xyz;

        float size =  v.used * .001;

				if( tri == 0 || tri == 3 ){ fPos -= ri * size; fUV = float2( 0 ,0); }
				if( tri == 2 || tri == 4 ){ fPos += ri * size; fUV = float2( 1 ,1);}
				if( tri == 5 ){ fPos += up * size; fUV = float2( 0 ,1);}
				if( tri == 1 ){ fPos -= up * size; fUV = float2( 1 ,0); }

				o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));
				o.worldPos = fPos;
				o.eye = _WorldSpaceCameraPos - o.worldPos;
	
				o.nor = float3(0,0,0);//v.nor;//float3(0,0,0);
				o.uv = fUV;
        o.debug = normalize( o.eye ) * .5 + .5;// float3(float(bID)/10000,0,0);

        return o;


      }
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      	float3 col = v.debug * tex2D(_MainTex, v.uv ).xyz;//v.nor * .5+ .5;// v.debug;

        return float4( .1 * v.debug , 1. );


      }

      ENDCG

    }
  }

  Fallback Off
  
}