
Shader "Custom/rodLine" {

  Properties {
  

  }

  SubShader {


    Pass {

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"


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
      };


      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = UnityObjectToWorldNormal(v.normal);//v.normal;

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
    
        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {


        // Our color starts off at zero,   
        float3 col = float3( 0.0 , 0.0 , 0.0 );

        if( abs(v.uv.y - .5) < .3 ){
        	discard;
        }

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