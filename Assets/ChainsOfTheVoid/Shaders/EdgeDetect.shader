Shader "Custom/EdgeDetect"
{
	Properties
	{
		_MainTex("Dont use", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_BaseColor("Base Color", Color) = (1,1,1,1)
		_OutlineThinkness("Outline Thinkness", Range(-0.1, 0.5)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
			};

			//Defaults
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			//Add variables here
			float4 _OutlineColor;
			float4 _BaseColor;
			half _OutlineThinkness;
			
			//Vertex shader
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			//unused
			float4 sobel(v2f i)
			{
				float2 sDim = float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
				float4 yColor =
					tex2D(_MainTex, i.uv + float2(1, -1) / sDim) +
					tex2D(_MainTex, i.uv + float2(0, -1) / sDim) * 2 +
					tex2D(_MainTex, i.uv + float2(-1, -1) / sDim) -
					tex2D(_MainTex, i.uv + float2(1, 1) / sDim) -
					tex2D(_MainTex, i.uv + float2(0, 1) / sDim) * 2 -
					tex2D(_MainTex, i.uv + float2(-1, 1) / sDim);

				float4 xColor =
					tex2D(_MainTex, i.uv + float2(-1, -1) / sDim) +
					tex2D(_MainTex, i.uv + float2(-1, 0) / sDim) * 2 +
					tex2D(_MainTex, i.uv + float2(-1, 1) / sDim) -
					tex2D(_MainTex, i.uv + float2(1, -1) / sDim) -
					tex2D(_MainTex, i.uv + float2(1, 0) / sDim) * 2 -
					tex2D(_MainTex, i.uv + float2(1, 1) / sDim);

				return sqrt(yColor * yColor + xColor * xColor);
			}

			//Frag Shader (Runs once no loop)
			fixed4 frag (v2f i) : SV_Target
			{
				float4 retval = _BaseColor;
				if(i.uv.x > 0.9f - _OutlineThinkness)  retval = _OutlineColor;
				if (i.uv.x < 0.1f + _OutlineThinkness)  retval = _OutlineColor;
				if (i.uv.y > 0.9f - _OutlineThinkness)  retval = _OutlineColor;
				if (i.uv.y < 0.1f + _OutlineThinkness)  retval = _OutlineColor;
				return retval;
			}
			ENDCG
		}
	}
}
