// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Custom/fakeRefract" {

  Properties {
  

  }

  SubShader {


    Cull Back

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
          float3 ro       : TEXCOORD1;
          float3 rd       : TEXCOORD2;
          float4 screenPos : TEXCOORD4;

          float3 fakedScreenPos : TEXCOORD3;
      };

      uniform sampler2D _vidTexY;
      uniform sampler2D _vidTexCBCR;
      uniform sampler2D _Audio;

      uniform float _hsvStart;


      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = UnityObjectToWorldNormal(v.normal);//v.normal;

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
        float4 mPos = mul( unity_ObjectToWorld , float4( v.position.xyz , 1) );

        // The ray origin will be right where the position is of the surface
        o.ro = v.position;

        

        o.screenPos = ComputeScreenPos( UnityObjectToClipPos( float4( v.position.xyz , 1)) );


        float3 camPos = mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and 
        // draw a ray from the camera to the position shooting a ray through that point
        o.rd = normalize( v.position.xyz - camPos );


        o.ro = mPos;


        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {


        float3 dir = normalize((_WorldSpaceCameraPos - v.ro));


        float match = dot( dir , v.normal );

        float n = noise( v.ro * 50 + 1* (sin(length( v.ro)) + 2) * float3( sin( _Time.y  * .1), sin(_Time.y*.2), sin( _Time.y*.4)) );

        float3 refracted = normalize(refract( dir , v.normal  , .8 + n * .2  ));
        float3 newPos = v.ro + refracted * .002 * (n+6);
        float4 mp = mul( UNITY_MATRIX_VP , float4( newPos, 1. ) );

        float4 sp = ComputeScreenPos( mp );
     

        // Our color starts off at zero,   
        float3 col = float3( 0.0 , 0.0 , 0.0 );

       // float match = dot( v.ro , v.rd );

  // sample the texture
        //float2 texcoord = float2(1,1)- (.3 * match + .9 ) * v.screenPos.yx/v.screenPos.w;//ComputeScreenPos( v.screenPos ); //v.uv;
        float2 texcoord = float2(1,1)-  sp.yx/sp.w;//ComputeScreenPos( v.screenPos ); //v.uv;
        float y = tex2D(_vidTexY, clamp( texcoord , float2(0.001,0.001) , float2(.9999,.9999))).r;
        float4 ycbcr = float4(y, tex2D(_vidTexCBCR, texcoord).rg, 1.0);

        const float4x4 ycbcrToRGBTransform = float4x4(
            float4(1.0, +0.0000, +1.4020, -0.7010),
            float4(1.0, -0.3441, -0.7141, +0.5291),
            float4(1.0, +1.7720, +0.0000, -0.8860),
            float4(0.0, +0.0000, +0.0000, +1.0000)
          );

        col= mul(ycbcrToRGBTransform, ycbcr).xyz;

        //col += (1-match) * (1-match);

        col += (1-match) * (1-match) ;

        col += tex2D( _Audio , float2( n , 0.)).xyz;

        col *= hsv( n * .3  + _hsvStart , .4 ,1 );


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