// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Custom/basicRefractFake" {

  Properties {

    _RefractDist( "RefractRayDistance",float) = .001
    _RefractAmount( "RefractAmount",float) = .8

  }

  SubShader {


    Cull Back

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
          float3 mPos     : TEXCOORD1;
      };

      uniform sampler2D _vidTexY;
      uniform sampler2D _vidTexCBCR;
      uniform float _RefractDist;
      uniform float _RefractAmount;


      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = UnityObjectToWorldNormal(v.normal);

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
        //Gets the positino of the vert in world space
        float4 mPos = mul( unity_ObjectToWorld , float4( v.position.xyz , 1) );
        o.mPos = mPos;

     

  
        


        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {

        // Gets ray going from Camera to vert
        float3 dir = normalize((_WorldSpaceCameraPos - v.mPos));


        // Here is where refraction happens!
        float3 refracted = normalize(refract( dir , v.normal  , _RefractAmount ));
        
        //We take the refracted ray, and step it forward a tiny bit 
        //This is the point that we will use to sample from video texture
        float3 newPos = v.mPos + refracted * _RefractDist;


        float4 mp = mul( UNITY_MATRIX_VP , float4( newPos, 1. ) );

        // Getting our screen position
        float4 sp = ComputeScreenPos( mp );
     

        // Our color starts off at zero,   
        float3 col = float3( 0.0 , 0.0 , 0.0 );

        // Getting The color from the ARCamera textures!
        float2 texcoord = float2(1,1)-  sp.yx/sp.w;//ComputeScreenPos( v.screenPos ); //v.uv;

        //clamping for when you get to the edge of the screen. 
        // This is really where you can see how the process is quite fake...
        float y = tex2D(_vidTexY, clamp( texcoord , float2(0.001,0.001) , float2(.9999,.9999))).r;
        float4 ycbcr = float4(y, tex2D(_vidTexCBCR, texcoord).rg, 1.0);

        const float4x4 ycbcrToRGBTransform = float4x4(
            float4(1.0, +0.0000, +1.4020, -0.7010),
            float4(1.0, -0.3441, -0.7141, +0.5291),
            float4(1.0, +1.7720, +0.0000, -0.8860),
            float4(0.0, +0.0000, +0.0000, +1.0000)
          );

        col= mul(ycbcrToRGBTransform, ycbcr).xyz;

        //Getting match of normal and cam dir for 'rim lighting'
        float match = dot( dir , v.normal );
        col += (1-match) * (1-match);

        fixed4 color;
        color = fixed4( col , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
