Shader "Custom/tailRenderer" {
		Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Main Color", Color) = (1,1,1,1)
  }
  SubShader{

  	


    Cull Off
   // zWrite off
    Pass{


     // Blend One One // Additive
      CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"




      struct Vert{
      	float used;
      	float justChanged;
        float3 pos;
        float3 vel;
        float3 target;
        float3 oPos[8];
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

        int which = id / 6;
        int triID = id % 6;

        int bID = which / 7;
        int tailID = which % 7;

       	Vert v = _vertBuffer[bID];

       	float3 p1 = v.oPos[tailID];
       	float3 p2 = v.oPos[tailID+1];



       	float3 fPos = float3(0,0,0);//mul(float4(0,0,0,1),b.bindPose ).xyz;

       	fPos = v.pos + float3(float(tailID) ,0,0);//mul( b.transform, float4(fPos,1) ).xyz;
       	

       	float3 up = UNITY_MATRIX_IT_MV[0].xyz;
				float3 r = UNITY_MATRIX_IT_MV[1].xyz;

        float3 dif = p1 - p2;

       	float3 dir = normalize( dif );
       	float3 fwd  = normalize( cross(dir, r));
       	float3 ri = normalize( cross( fwd, dir ));

        float id1 = 1-float( tailID )/8;
        float id2 = 1-float( tailID )/8;

        float2 uv1 = float2( id1 , 0 );
        float2 uv2 = float2( id1 , 1 );
        float2 uv3 = float2( id2 , 0 );
        float2 uv4 = float2( id2 , 1 );

       	float3 f1 = p1 + ri  * .003 * id1;
       	float3 f2 = p1 - ri  * .003 * id1;

       	float3 f3 = p2 + ri  * .003 * id2;
       	float3 f4 = p2 - ri  * .003 * id2;






        float2 fUV;


      
				if( triID == 0 ||  triID ==  3 ){ fPos = f4; fUV = uv4; }
       	if( triID == 1  ){ fPos = f3;fUV = uv3;}
       	if( triID == 2 ||  triID ==  4 ){ fPos = f1;fUV = uv1;}
       	if( triID == 5  ){ fPos = f2;fUV = uv2;}


        o.uv = fUV;

				o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));
				o.worldPos = fPos;
				o.eye = _WorldSpaceCameraPos - o.worldPos;
	
				o.nor = float3(0,0,0);//v.nor;//float3(0,0,0);
		
        o.debug = dir * .5 + .5;

        return o;


      }
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      	float3 col = float3( 0,1,0);//v.nor * .5+ .5;// v.debug;

        return float4(v.debug  * v.uv.x + float3(1,1,1)* (1-v.uv.x) , 1. );


      }

      ENDCG

    }
  }

}