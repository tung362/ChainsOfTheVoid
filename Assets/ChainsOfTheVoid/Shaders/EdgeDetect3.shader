Shader "Custom/EdgeDetect3"
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

			float lookup(float2 p, float dx, float dy, float theD)
			{
				float2 uv = (p.xy + float2(dx * theD, dy * theD)) / _WorldSpaceCameraPos.xy; //iResolution
				float4 c = tex2D(_MainTex, uv.xy);

				// return as luma
				return 0.2126*c.r + 0.7152*c.g + 0.0722*c.b;
			}

			//Frag Shader (Runs once no loop)
			fixed4 frag (v2f i) : SV_Target
			{
				float d = sin(_Time * 5.0)*0.5 + 1.5;
				
			float2 p = i.uv;

			// simple sobel edge detection
			float gx = 0.0;
			gx += -1.0 * lookup(p, -1.0, -1.0, d);
			gx += -2.0 * lookup(p, -1.0, 0.0, d);
			gx += -1.0 * lookup(p, -1.0, 1.0, d);
			gx += 1.0 * lookup(p, 1.0, -1.0, d);
			gx += 2.0 * lookup(p, 1.0, 0.0, d);
			gx += 1.0 * lookup(p, 1.0, 1.0, d);

			float gy = 0.0;
			gy += -1.0 * lookup(p, -1.0, -1.0, d);
			gy += -2.0 * lookup(p, 0.0, -1.0, d);
			gy += -1.0 * lookup(p, 1.0, -1.0, d);
			gy += 1.0 * lookup(p, -1.0, 1.0, d);
			gy += 2.0 * lookup(p, 0.0, 1.0, d);
			gy += 1.0 * lookup(p, 1.0, 1.0, d);

			// hack: use g^2 to conceal noise in the video
			float g = gx*gx + gy*gy;
			float g2 = g * (sin(_Time) / 2.0 + 0.5);

			float4 col = tex2D(_MainTex, p / _WorldSpaceCameraPos.xy);
			col += float4(0.0, g, g2, 1.0);

			return col;
				
				
				/*float4 retval = _BaseColor;
				return retval;*/
			}
			ENDCG
		}
	}
}
