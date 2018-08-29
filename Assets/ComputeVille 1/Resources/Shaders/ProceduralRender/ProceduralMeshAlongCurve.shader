Shader "ProceduralRender/MeshAlongCurve" {
	Properties {
		}


  SubShader{
//        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
    Cull Off
    Pass{

      //Blend SrcAlpha OneMinusSrcAlpha // Alpha blending

      CGPROGRAM
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "../Chunks/noise.cginc"
      #include "../Chunks/hsv.cginc"

		  uniform int _CurveCount;
		  uniform int _VertCount;
      uniform float3 _Color;
      uniform float _ModelLength;
      uniform float3 _Start;
      uniform float3 _End;

struct Vert{
  float3 pos;
  float3 nor;
  float3 tan;
  float2 uv;
  float debug;
};


struct Curve{
  float3 pos;
  float3 oPos;
  float3 nor;
  float3 tangent;
  float3 bitangent;
  float idDown;
  float idUp;
  float2 uv;
  float3 debug;
};



      StructuredBuffer<Vert> _vertBuffer;
      StructuredBuffer<Curve> _curveBuffer;
      StructuredBuffer<int> _triBuffer;


      //uniform float4x4 worldMat;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos      : SV_POSITION;
          float3 nor      : TEXCOORD0;
          float3 worldPos : TEXCOORD1;
          float3 eye      : TEXCOORD2;
          float3 debug    : TEXCOORD3;
          float2 uv       : TEXCOORD4;
          float  noiseVal : TEXCOORD5;
      };



float3 cubicCurve( float t , float3  c0 , float3 c1 , float3 c2 , float3 c3 ){
  
  float s  = 1. - t; 

  float3 v1 = c0 * ( s * s * s );
  float3 v2 = 3. * c1 * ( s * s ) * t;
  float3 v3 = 3. * c2 * s * ( t * t );
  float3 v4 = c3 * ( t * t * t );

  float3 value = v1 + v2 + v3 + v4;

  return value;

}


float3 cubicFromValue( in float val , out float3 dir , out float3 nor ){

  //float3 upPos;
  //float3 doPos;




  Curve c0;
  Curve c1;
  Curve c2;
  Curve cMinus;

  float3 n0 = float3(0,0,0);
  float3 n1 = float3(0,0,0);
  float3 v0 = float3( 0. , 0. , 0. );
  float3 v1 = float3( 0. , 0. , 0. );



  float base = val * (float(_CurveCount)-1.);
  float baseUp   = floor( base );
  float baseDown = ceil( base );
  float amount = base - baseUp;


  if( baseUp == 0. ){

    c0 = _curveBuffer[ int( baseUp )         ];
    c1 = _curveBuffer[ int( baseDown )       ];
    c2 = _curveBuffer[ int( baseDown + 1. )  ];


    v1 = .5 * ( c2.pos - c0.pos );
    n1 = .5 * ( c2.tangent - c0.tangent );

  }else if( baseDown == float(_CurveCount-1) ){

    c0 = _curveBuffer[ int( baseUp )       ];
    c1 = _curveBuffer[ int( baseDown )     ];
    c2 = _curveBuffer[ int( baseUp - 1. )  ];

    v0 = .5 * ( c1.pos - c2.pos );
    n0 = .5 * ( c1.tangent - c2.tangent );

  }else{

    c0 = _curveBuffer[ int( baseUp )    ];
    c1 = _curveBuffer[ int( baseDown )  ];


    float3 pMinus;

    cMinus = _curveBuffer[ int( baseUp - 1. )    ];
    c2 =     _curveBuffer[ int( baseDown + 1. )  ];

    v1 = .5 * ( c2.pos - c0.pos );
    v0 = .5 * ( c1.pos - cMinus.pos );

    n1 = .5 * ( c2.tangent - c0.tangent );
    n0 = .5 * ( c1.tangent - cMinus.tangent );

  }


  float3 p0 = c0.pos;
  float3 p1 = c0.pos + v0/3.;
  float3 p2 = c1.pos - v1/3.;
  float3 p3 = c1.pos;

  float3 t0 = c0.tangent;
  float3 t1 = c0.tangent + n0/3.;
  float3 t2 = c1.tangent - n1/3.;
  float3 t3 = c1.tangent;

  float3 pos = cubicCurve( amount , p0 , p1 , p2 , p3 );
  float3 normal = normalize(cubicCurve( amount , t0 , t1 , t2 , t3 ));

  //nor = lerp(t0,t3,amount);//normal;//_curveBuffer[ int( baseDown )       ].debug;//normal; //cubicCurve( amount  + .11 , c0 , c1 , c2 , c3 );
  nor = normal;//_curveBuffer[ int( baseDown )       ].debug;//normal; //cubicCurve( amount  + .11 , c0 , c1 , c2 , c3 );
  dir = normalize(pos -cubicCurve( amount  + .01 , p0 , p1 , p2 , p3 ));

  return pos;


}


      //Our vertex function simply fetches a point from the buffer corresponding to the vertex index
      //which we transform with the view-projection matrix before passing to the pixel program.
      varyings vert (uint id : SV_VertexID){

        varyings o;

        int triID = _triBuffer[id];

        if( triID < _VertCount ){

// http://www.unchainedgeometry.com/jbloom/pdf/ref-frames.pdf
     /* float3 V = bezierDeriv(PosLocalStart.xyz, CtrlPosLocal0, CtrlPosLocal1, PosLocalEnd.xyz, tubeLerpCoeff);
      float3 Q = bezierDoubleDeriv(PosLocalStart.xyz, CtrlPosLocal0, CtrlPosLocal1, PosLocalEnd.xyz, tubeLerpCoeff);
      float3 T = normalize(V);
      float3 N = normalize(cross(cross(V, Q), V));
      float3 B = cross(N, T);

      float3 localNormal = v.normal.x * N + v.normal.z * B;*/


        	Vert v = _vertBuffer[triID];

          float val = v.pos.z / _ModelLength;

          if( val > 0 && val < 1 ){
        	float valAlong = clamp( val ,0,1);

        	float3 nor; float3 dir;

        	float3 extraPos = cubicFromValue(valAlong,dir,nor);
        	
           // nor = float3(0,0,1);
        
        	float x = v.pos.x;
        	float y = v.pos.y;

          float3 zDir = normalize(dir);
          float3 xDir = normalize(nor);
          float3 yDir = normalize(cross(dir,nor));

    float xN = 1;//noise( extraPos * 1000);
    float yN = 1;//noise( extraPos * 1000);

        	float3 fPos = extraPos  + x*xN  * xDir + y *yN* yDir;


       		o.worldPos = fPos;///* .001/(.1+length(v.debug));//*(1/(.1+max(length(v.debug),0)));//mul( worldMat , float4( v.pos , 1.) ).xyz;
	        o.debug = xDir;//float3(float(id)/1000000,1,0);
	        o.eye = _WorldSpaceCameraPos - o.worldPos;
          o.nor =normalize( v.nor);//fPos;
          o.uv = v.uv;

	        o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));
          }else{


              if( val < 0 ){

                float3 dir =  normalize(_curveBuffer[1].pos - _curveBuffer[0].pos);
                float3 oDir = -(_Start - _End);


                float3 xDir = normalize(cross( dir , oDir ));//float3(1,0,0);
                float3 yDir = -normalize(cross(oDir , xDir ));
                float3 zDir = normalize(oDir);// -float3(0,0,1);

                float x = v.pos.x;
                float y = v.pos.y;
                float z = v.pos.z;

                float3 extraPos = _curveBuffer[0].pos;


                float3 fPos = extraPos + z * zDir  + x * xDir + y * yDir;

                o.worldPos = fPos;///* .001/(.1+length(v.debug));//*(1/(.1+max(length(v.debug),0)));//mul( worldMat , float4( v.pos , 1.) ).xyz;
                o.debug = xDir;//float3(float(id)/1000000,1,0);
                o.eye = _WorldSpaceCameraPos - o.worldPos;
                o.nor =normalize( v.nor);//fPos;
                o.uv = v.uv;

                o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));


              }

              if( val > 1 ){
                
                float3 dir =  normalize(_curveBuffer[_CurveCount-1].pos - _curveBuffer[_CurveCount-2].pos);
                float3 oDir = _Start - _End;


                float3 xDir = normalize(cross( dir , oDir ));//float3(1,0,0);
                float3 yDir = normalize(cross(oDir , xDir));
                float3 zDir = -normalize(oDir);// -float3(0,0,1);

                float x = v.pos.x;
                float y = v.pos.y;
                float z = v.pos.z-_ModelLength;

                float3 extraPos = _curveBuffer[_CurveCount-1].pos;


                float3 fPos = extraPos + z * zDir  + x * xDir + y * yDir;

                o.worldPos = fPos;///* .001/(.1+length(v.debug));//*(1/(.1+max(length(v.debug),0)));//mul( worldMat , float4( v.pos , 1.) ).xyz;
                o.debug = xDir;//float3(float(id)/1000000,1,0);
                o.eye = _WorldSpaceCameraPos - o.worldPos;
                o.nor =normalize( v.nor);//fPos;
                o.uv = v.uv;

                o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));


              }

          }

       	}

        return o;

      }




      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
          float3 col = normalize(v.debug*100);// * .5 + .5;
      		col = normalize(v.nor) * .5 + .5;


          col =  col;//float3(v.uv.x,0,0);//sin(v.uv.x*100) + sin(v.uv.y*100);
          return float4( col, 1 );

      }

      ENDCG

    }
  }

  Fallback Off


}
