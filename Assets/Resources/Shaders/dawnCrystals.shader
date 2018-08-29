Shader "Custom/dawnCrystals" {
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/crystalsUnderWater" {

  Properties {
  


    _Direction( "_Direction", Vector ) = (0,1,0,0)
    _Center( "_Center", Vector ) = (0,-1,0,0)
    _Falloff("_Falloff" , Float ) = 1


    _MainTex ("_MainTex", 2D) = "white" {}

    _RefractDist( "RefractRayDistance",float) = .001
    _RefractAmount( "RefractAmount",float) = .8

    _CubeMap( "Cube Map" , Cube ) = "white" {}

  }

  



  SubShader {

    // inside SubShader
  //  Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }

    // inside Pass
   // ZWrite Off
    //Blend SrcAlpha OneMinusSrcAlpha


       Pass
       {
           Name "ShadowCaster"
           Tags { "LightMode" = "ShadowCaster" }
         
           Fog { Mode Off }
           ZWrite On ZTest Less Cull Off
           Offset 1, 1
           
           CGPROGRAM

             #pragma vertex vert
      #pragma fragment frag exclude_path:prepass

      #include "UnityCG.cginc"
      #include "Chunks/noise.cginc"



     /* float3 doVertPos( position , normal , uv ){

      }*/


      uniform int _NumberSteps;
      uniform float _TotalDepth;
      uniform float _PatternSize;
      uniform float _HueSize;

      uniform sampler2D _Audio;
      uniform sampler2D _MainTex;

      struct VertexIn{
         float4 position  : POSITION; 
         float3 normal    : NORMAL; 
         float4 texcoord  : TEXCOORD0; 
         float4 tangent   : TANGENT;
      };


      struct VertexOut {
          float4 pos      : POSITION; 
      };



      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        //Gets the positino of the vert in world space
        float4 mPos = mul( unity_ObjectToWorld , float4( v.position.xyz , 1) );
        

        float n = noise( mPos * 30 + float3( 0 , _Time.y * 1.2 , 0 ) );
        n += noise( mPos * 10 + float3( 0 , _Time.y * 1.5 , 0 ) );
        n /= 2;

        // Gets ray going from Camera to vert
        float3 dir = normalize((_WorldSpaceCameraPos - mPos));

        float m = dot( dir , v.normal );
        float3 audio = tex2Dlod( _Audio , float4( n , 0 , 0, 0)).xyz;

        float3 fPos = v.position +  v.normal * length( audio*audio) * 10.3 * n*n*n ; 

        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  fPos );
     
        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {
        fixed4 color = fixed4( 1,1,1,1);
        return color;
      }

      ENDCG
        
    }
    Pass {

      Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Geometry"}
            blend SrcAlpha OneMinusSrcAlpha
           
            //LOD 200
            CGPROGRAM
            //#pragma surface surf Lambert 

      //CGPROGRAM



      #pragma vertex vert
      #pragma fragment frag exclude_path:prepass

      #include "UnityCG.cginc"
      #include "Chunks/noise.cginc"


      uniform int _NumberSteps;
      uniform float _TotalDepth;
      uniform float _PatternSize;
      uniform float _HueSize;
      uniform sampler2D _Audio;




      struct VertexIn{
         float4 position  : POSITION; 
         float3 normal    : NORMAL; 
         float4 texcoord  : TEXCOORD0; 
         float4 tangent   : TANGENT;
      };


      struct VertexOut {
          float4 pos    	: POSITION; 
          float3 normal 	: NORMAL; 
          float4 uv     	: TEXCOORD0; 
          float3 ro     	: TEXCOORD1;
          float3 rd     	: TEXCOORD2;
          float planeVal  : TEXCOORD3;
          float3 mPos  : TEXCOORD4;
          float noiseVal  : TEXCOORD5;
      };




      float getFogVal( float3 pos ){

      	return abs( sin( pos.x * _PatternSize ) + sin(pos.y * _PatternSize ) + sin( pos.z * _PatternSize ));
      }

      float sdPlane( float3 p, float3 n )
      {
        // n must be normalized
        return dot(p,n.xyz);
      }

      uniform float3 _Center;
      uniform float3 _Direction;
      uniform float _Falloff;

      uniform float _RefractDist;
      uniform float _RefractAmount;

      uniform sampler2D _Audio;
      uniform sampler2D _MainTex;

      uniform samplerCUBE _CubeMap;

      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = v.normal;

        o.uv = v.texcoord;
      


               //Gets the positino of the vert in world space
        float4 mPos = mul( unity_ObjectToWorld , float4( v.position.xyz , 1) );
        

        float n = noise( mPos * 30 + float3( 0 , _Time.y * 1.2 , 0 ) );
        n += noise( mPos * 10 + float3( 0 , _Time.y * 1.5 , 0 ) );
        n /= 2;

        // Gets ray going from Camera to vert
        float3 dir = normalize((_WorldSpaceCameraPos - mPos));

        float m = dot( dir , o.normal );
        float3 audio = tex2Dlod( _Audio , float4( n , 0 , 0, 0)).xyz;

        float3 fPos = v.position +  v.normal * length( audio*audio) * .3 * n*n*n ; 

        o.noiseVal = n;

        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  fPos );


        mPos = mul( unity_ObjectToWorld , float4( fPos.xyz , 1) );
        o.mPos = mPos;
       


        float3 centeredP = mPos- _Center;
        float planeVal = dot( centeredP , _Direction );

        o.planeVal = planeVal;

        // The ray origin will be right where the position is of the surface
        o.ro = fPos;




        float3 camPos = mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and 
        // draw a ray from the camera to the position shooting a ray through that point
        o.rd = normalize( fPos.xyz - camPos );

        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {
        float3 col;
		   

         // Gets ray going from Camera to vert
        float3 dir = normalize((_WorldSpaceCameraPos - v.mPos));

        float3 refl = reflect( dir , v.normal );
        col = texCUBE( _CubeMap , normalize(refl) ).xyz;

        //Getting match of normal and cam dir for 'rim lighting'
        float match = dot( dir , v.normal );

        float3 aCol = tex2D( _Audio , float2( v.noiseVal , 0 )).xyz;
        //col += (1-match) * (1-match);

       // col += aCol * (1-match);

        float3 mainCol = tex2D( _MainTex , v.uv.xy ).xyz;

        //col = lerp( mainCol , mainCol * aCol  , min( 1 , 4 * v.noiseVal * v.noiseVal * length( aCol ) ));


        float fall =  clamp( -v.planeVal * _Falloff  , 0 , 1 );
        float opacity = 1 -fall;


        if( opacity < 1 ){
        	col *= float3( .3 , 1 , .7);
        }else{
        	col *= float3( 1 , .8 , .3);
        }

        //col = float3( 0, fall *100 , 0 );



        fixed4 color = fixed4( col , opacity );


        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
