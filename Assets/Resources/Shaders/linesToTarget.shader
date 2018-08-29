Shader "Custom/linesToTarget" {
	Properties
  {
  }
  SubShader{

    //Cull Back
    zWrite off
    Pass{


      Blend One One // Additive
      CGPROGRAM
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "Chunks/hsv.cginc"

      struct Vert{
      	float used;
        float3 pos;
        float3 debug;
      };

      StructuredBuffer<Vert> _vertBuffer;// : register(u1);

      uniform float3 _Target;

      uniform int numVerts;
      uniform float connectionRadius;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          
          float4 pos 			: SV_POSITION;
          float3 nor 			: TEXCOORD0;
          float start     : TEXCOORD1;
          float3 mPos     : TEXCOORD2;


      };


      varyings vert ( uint id : SV_VertexID ){

        varyings o;

        int bID = id / 2;
        int alt = id % 2;

       	Vert v1 = _vertBuffer[bID];

       	float3 fPos = float3(0,0,0);


        o.start = 0;
       	if( v1.used == 1 ){
    			fPos = v1.pos;
       		if( alt == 1 ){ 
            o.start = 1;
       			fPos = _Target; 
       			o.nor = normalize( v1.pos - _Target ) * .5 + .5;
       		}
       	}
			   o.mPos = fPos;
				o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));

				
        return o;


      }

      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      	float3 col = float3( 1,1,1) * v.start * hsv( v.start + _Time.y , 1 , 1) / ( .4 +length( _Target - v.mPos) * 20);//v.nor * .5+ .5;// v.debug;

        return float4( col , v.start );


      }

      ENDCG

    }
  }

  Fallback Off
  
}