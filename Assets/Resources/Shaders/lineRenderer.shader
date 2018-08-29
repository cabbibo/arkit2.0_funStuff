Shader "Custom/lineRenderer" {

	Properties
  {
  }
  SubShader{

    //Cull Back
    //zWrite off
    Pass{


      Blend One One // Additive
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

      uniform int numVerts;
      uniform float connectionRadius;

      uniform float3 _Target;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos 			: SV_POSITION;
          float3 nor 			: TEXCOORD0;
          float3 mPos     : TEXCOORD1;


      };


      varyings vert ( uint id : SV_VertexID ){

        varyings o;

        int bID = id / 2;
        int alt = id % 2;

        int id1 = bID % numVerts;
        int id2 = bID / numVerts;


       	Vert v1 = _vertBuffer[id1];
       	Vert v2 = _vertBuffer[id2];

       	float3 fPos = float3(0,0,0);


       	if( v1.used == 1 && v2.used == 1 ){
       		if( length( v1.pos - v2.pos ) < connectionRadius ){
       			fPos = v1.pos;
       			if( alt == 1 ){ fPos = v2.pos; }

       			o.nor = normalize( v1.pos - v2.pos ) * .5 ;
       		}
       	}


				o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));

        o.mPos= mul( unity_ObjectToWorld , fPos ).xyz;

				
        return o;


      }
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      	float3 col = float3( 1,1,1) / (100 * length( v.mPos - _Target));//v.nor * .5+ .5;// v.debug;

        return float4( col ,1 );


      }

      ENDCG

    }
  }

  Fallback Off
  
}