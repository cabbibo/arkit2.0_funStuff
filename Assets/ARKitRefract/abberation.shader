// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Custom/abberation" {

  Properties {

    _RefractDist( "RefractRayDistance",float) = .001
    _RefractAmount( "RefractAmount",float) = .9
    _RefractChange( "RefractChange",float) = .3
    _RefractColAmount( "RefractColAmount",float) = .05
    _RefractColChange( "RefractColChange",float) = .2
    _RefractDistChange( "RefractColChange",float) = .01

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
      uniform float _RefractColChange;
      uniform float _RefractColAmount;
      uniform float _RefractChange;
      uniform float _RefractDistChange;

      //uniform 

      uniform float2 _Touch;


      
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


     

      float3 getVidCol( float3 newPos ){


      	float4 mp = mul( UNITY_MATRIX_VP , float4( newPos, 1. ) );

        // Getting our screen position
        float4 sp = ComputeScreenPos( mp );

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

        float3 col =  mul(ycbcrToRGBTransform, ycbcr).xyz;

        return col;

      }

       float3 refractedCol( float3 pos , float3 dir , float3 normal , float refractAmount ){

      	float3 refracted = normalize(refract( dir , normal  , refractAmount ));

      	float3 newPos = pos + refracted * (_RefractDist+_Touch.y * _RefractDistChange);

      	float3 col = getVidCol( newPos );

        return col;
     

      }


      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {

        // Gets ray going from Camera to vert
        float3 dir = normalize((_WorldSpaceCameraPos - v.mPos));


      

      	float3 colMain = getVidCol( v.mPos );////refractedCol( v.mPos , dir , v.normal , 1);


      	float baseAmount = _RefractAmount - _Touch.x * _RefractChange;
      	float colAmount = _RefractColAmount - _Touch.y * _RefractColChange;


      	float3 colR = refractedCol( v.mPos , dir , v.normal , baseAmount - 0* colAmount );
      	float3 colG = refractedCol( v.mPos , dir , v.normal , baseAmount - 1* colAmount );
      	float3 colB = refractedCol( v.mPos , dir , v.normal , baseAmount - 2* colAmount );


      	float3 col = float3(colR.x,colG.y,colB.z);

        //Getting match of normal and cam dir for 'rim lighting'
        float match = dot( dir , v.normal );
        //col += (1-match) * (1-match);


				float val = min( length(v.uv - float2(.5,.5)) * 2 , 1);
       // col = lerp( col , colMain , val  );

      

        //col = (1-val) * (1-val);

        //col.x = _Touch.x;

        fixed4 color;
        color = fixed4( col , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
