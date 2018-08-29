  Shader "PostTransfer/ModelRope1" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Amount ("Extrusion Amount", Range(-1,1)) = 0.5
       _BumpMap ("Bumpmap", 2D) = "bump" {}
    }
    SubShader {
        Cull Off
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
         #pragma target 3.0
#include "UnityCG.cginc"
#include "../Chunks/hsv.cginc"
     #pragma vertex vert
      #pragma surface surf Standard addshadow
     
 struct appdata{
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
 
            uint id : SV_VertexID;
         };
 
  
       #ifdef SHADER_API_METAL
           struct Transfer {
        float3 vertex;
        float3 normal;
        float3 tangent;
        float2 uv;
        float debug;
      };
      StructuredBuffer<Transfer> _transferBuffer;
#endif
       struct Input {
          float2 texcoord1;
          float3 tangent;
          float debug;
      };
       float _Amount;
       void vert (inout appdata v, out Input data ) {
      
 UNITY_INITIALIZE_OUTPUT( Input , data );
       #ifdef SHADER_API_METAL
        float3 fPos = _transferBuffer[v.id].vertex;
        float3 fNor = _transferBuffer[v.id].normal;
        float3 fTan = _transferBuffer[v.id].tangent;
        float2 fUV = _transferBuffer[v.id].uv;
      
            v.vertex = float4(fPos,1);// float4(v.vertex.xyz,1.0f);
            v.normal = fNor; //float4(normalize(points[id].normal), 1.0f);
            v.tangent = float4(fTan,1);//float4( normalize(cross(fNor,float3(0,1,0))),1);
             //v.UV = fUV;
           // v.texcoord1 = fUV;
            data.texcoord1 = fUV;//float2(1,1);
            data.tangent = v.tangent;
            data.debug =  _transferBuffer[v.id].debug;
            #endif
  
         }
 
      sampler2D _MainTex;
      sampler2D _BumpMap;
      void surf (Input IN, inout SurfaceOutputStandard o) {
         float3 mainCol = tex2D (_MainTex, IN.texcoord1.xy).rgb;
        float3 nor = UnpackNormal (tex2D (_BumpMap, IN.texcoord1.xy * float2( 1 , 1)));
          //half rim = 1.0 - saturate(dot (normalize(v.vertex._WorldSpaceCameraPosition), o.Normal));
          //o.Emission =UnpackNormal (tleex2D (_BumpMap, IN.texcoord1.xy * float2( 1 , 1))) * .5 + .5;// _RimColor.rgb * pow (rim, _RimPower);
          o.Albedo =  2*mainCol;//(nor * .5 + .5) * mainCol * hsv( length(mainCol) * .3 + _Time.x * 5 + IN.debug,1,1);// 1000*(UnpackNormal (tex2D (_BumpMap, IN.texcoord1.xy * float2( 1 , 1)))-float3(0,0,1));// * .5 + .5; //tex2D (_MainTex, IN.texcoord1.xy).rgb;
         o.Smoothness = .3;//0.8f;
         o.Metallic = .8;//.9f;
          o.Normal = nor;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }
