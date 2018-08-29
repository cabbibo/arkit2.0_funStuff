// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

 Shader "Custom/insideBox" {
  Properties {
  

    _NumberSteps( "Number Steps", Int ) = 100
    _MaxTraceDistance( "Max Trace Distance" , Float ) = 6.0
    _IntersectionPrecision( "Intersection Precision" , Float ) = 0.0001
    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}


  }


  
  SubShader {
    //Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
 Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }

// inside Pass
ZWrite Off
Blend SrcAlpha OneMinusSrcAlpha
    LOD 200

    Pass {
      //Blend SrcAlpha OneMinusSrcAlpha // Alpha blending


      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      // Use shader model 3.0 target, to get nicer looking lighting
      #pragma target 3.0

      #include "UnityCG.cginc"
      #include "Chunks/noise.cginc"
      
 


      


      uniform int _NumberSteps;
      uniform float  _IntersectionPrecision;
      uniform float _MaxTraceDistance;

     
      uniform float3 scale;

      uniform samplerCUBE _CubeMap;
      


      struct VertexIn
      {
         float4 position  : POSITION; 
         float3 normal    : NORMAL; 
         float4 texcoord  : TEXCOORD0; 
         float4 tangent   : TANGENT;
      };

      struct VertexOut {
          float4 pos    : POSITION; 
          float3 normal : NORMAL; 
          float4 uv     : TEXCOORD0; 
          float3 ro     : TEXCOORD2;
          float3 origin : TEXCOORD3;

          //float3 rd     : TEXCOORD3;
          float3 camPos : TEXCOORD4;
      };
        

      float3 origin;

      #include "Chunks/sdfFunctions.cginc"

      
      float2 map( in float3 pos ){
        
        float2 res;
        float2 sphere;
        float2 plane;

        //res = float2( -sdBox( pos , float3( 1. , 1. , 1. ) * .53 ) , 0. );
        //res = smoothU( res , float2( sdSphere( pos , .04) , 1. ) , 0.1);

        sphere = float2( sdSphere( (pos ) + float3(0 , .25 , 0)  - origin , .1 ) , 1. );
        plane = float2( sdPlane( pos - origin + float3( 0 , .25 , 0 ) , float4( 0,1,0,0) ),5.);


        float n  = noise( pos * 10 + float3( _Time.y , 0 , 0 ) ) * 1.3;// + .4 * noise( pos * 30 + float3( 0, _Time.y , 0 ) ) + .2 * noise( pos * 60+ float3( 0,0,_Time.y ) );

        res = sphere;
        //res.x  += n * .08;

        sphere = float2( sdSphere( (pos )+ float3(sin(_Time.y * .4 + .2 ) * .1 , 0.2 + sin(_Time.y * 2) * .1, sin(_Time.y * .3) * .1)  - origin , .05 ) , 1. );
        res = smoothU( res , sphere , .1 );

        sphere = float2( sdSphere( (pos )+ float3(sin(_Time.y) * .1 , 0.2 + sin(_Time.y * 2 + 1 ) * .1, sin(_Time.y * 1.3) * .1)  - origin , .05 ) , 1. );
        res = smoothU( res , sphere , .1 );

        sphere = float2( sdSphere( (pos )+ float3(sin(_Time.y * .8) * .1 , 0.4 + sin(_Time.y * 2 + 3 ) * .1, sin(_Time.y * 1.3) * .1)  - origin , .05 ) , 1. );
        res = smoothU( res , sphere , .1 );


        res = smoothU( res , plane , .2 );

        return res;//float2( length( pos ) - .3, 0.1 ); 
     
      }

      float3 calcNormal( in float3 pos ){

        float3 eps = float3( 0.001, 0.0, 0.0 );
        float3 nor = float3(
            map(pos+eps.xyy).x - map(pos-eps.xyy).x,
            map(pos+eps.yxy).x - map(pos-eps.yxy).x,
            map(pos+eps.yyx).x - map(pos-eps.yyx).x );
        return normalize(nor);

      }
              
         

      float2 calcIntersection( in float3 ro , in float3 rd ){     
            
               
        float h =  _IntersectionPrecision * 2;
        float t = 0.0;
        float res = -1.0;
        float id = -1.0;
        
        for( int i=0; i< _NumberSteps; i++ ){
            
            if( h < _IntersectionPrecision || t > _MaxTraceDistance ) break;
    
            float3 pos = ro + rd*t;
            float2 m = map( pos );
            
            h = m.x;
            t += h;
            id = m.y;
            
        }
    
    
        if( t <  _MaxTraceDistance ){ res = t; }
        if( t >  _MaxTraceDistance ){ id = -1.0; }
        
        return float2( res , id );
          
      
      }
            
    

      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = v.normal;
        
        o.uv = v.texcoord;
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
        float3 mPos = mul( unity_ObjectToWorld , v.position );

        o.ro = mPos;
        o.camPos = _WorldSpaceCameraPos; 
        o.origin = mul( unity_ObjectToWorld , float4(0,0,0,1) );


        return o;

      }


     // Fragment Shader
      fixed4 frag(VertexOut i) : COLOR {

        float3 ro = i.ro;
        float3 rd = normalize(ro - i.camPos);

        origin= i.origin;

        float3 col = float3( 0.0 , 0.0 , 0.0 );
        float2 res = calcIntersection( ro , rd );
        
        col= float3( 0. , 0. , 0. );


        float trans;

        if( res.y > -0.5 ){

          float3 pos = ro + rd * res.x;
          float3 norm = calcNormal( pos );
          col = norm * .5 + .5;

          float3 fRefl = reflect( -rd , norm );
          float3 cubeCol = texCUBE(_CubeMap,-fRefl ).rgb;

          col *= cubeCol;
          trans = 1;
          if( res.y > 4.5 ){
            //col = float3(0,0,0 );

            trans = 1 - (res.y - 4.5) * 2;
            if( res.y > 4.95 ){
              discard;
            }
          }
          //col = float3( 1. , 0. , 0. );
          
        }else{
          trans = 0;
          discard;
        }
     
        //col = float3( 1. , 1. , 1. );

            fixed4 color;
            color = fixed4( col , trans );
            return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
