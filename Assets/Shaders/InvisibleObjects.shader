Shader "MyShaders/InvisibleObjects"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_GhostTexture("Ghost texture for when near player but not seen", Color) = (1,1,1,1)
		_Position("Position", Vector) = (.0, .0, .0)
		_Distance("distance for shadow", Float) = 3
    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
			ZWrite Off
			ZTest Always
			Stencil {
				Ref 3
				Comp Less
				Pass Replace
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
		Pass
		{

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
					float3 worldPos : TEXCOORD1;
				};

				sampler2D _GhostTexture;
				float4 _GhostTexture_ST;
				uniform float3 _Position;
				float _Distance;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _GhostTexture);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_GhostTexture, i.uv);
					col.a = _Distance - distance(i.worldPos, _Position);
					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
			}
			ENDCG
		}
    }
}
