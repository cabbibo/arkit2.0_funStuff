Shader "Custom/connectionLine" {
  Properties {
  

		_Evil( "Evil" , Float ) = 1.0
  }

  SubShader {


    Pass {

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "Chunks/noise.cginc"
      #include "Chunks/hsv.cginc"


      struct VertexIn{
         float4 position  : POSITION; 
         float3 normal    : NORMAL; 
         float4 texcoord  : TEXCOORD0; 
         float4 tangent   : TANGENT;
      };


      struct VertexOut {
          float4 pos      : POSITION; 
          float3 normal   : NORMAL; 
          float4 uv       : TEXCOORD0; 
          float3 mPos       : TEXCOORD1; 
      };


      uniform float _Evil;


      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = UnityObjectToWorldNormal(v.normal);//v.normal;

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );

        o.mPos = mul( unity_ObjectToWorld , v.position );
     
    
        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {


        // Our color starts off at zero,   
        float3 col = hsv( v.uv.x + _Time.y , .6 ,1-_Evil);

        float val = sin( v.uv.x * 50);
        val += noise( v.mPos * 100 ) * 2;
        if( val < 1.5 ){
        	discard;
        }



        col *= val - 1;

        //col += v.uv;

        //col = v.screenPos.x/v.screenPos.w;

        fixed4 color;
        color = fixed4( col , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
