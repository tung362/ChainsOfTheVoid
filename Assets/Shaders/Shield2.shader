Shader "Custom/Shield2"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_InnerRange("Inner Range", float) = 0.0
		_OuterIntensity("Outer Intensity", float) = 0.0
		_Multiply("Multiply", float) = 2.0
		_AdditiveColor("Additive Color", Color) = (1,1,1,1)
		_SurfaceColor("Surface Color", Color) = (1,1,1,1)
		_Transparency("Transparency", Range(-1,1)) = 0.5
		_SwitchMode("SwitchMode", float) = 0

		/*_ColorA("Color A", Color) = (1, 1, 1, 1)
		_ColorB("Color B", Color) = (0, 0, 0, 1)
		_Slide("Slide", Range(0, 1)) = 0.5

		_Augoo("Augoo", Range(-10, 10)) = 0
		_EEEE1("_EEEE1", Range(-30, 30)) = 0
		_EEEE2("_EEEE2", Range(-30, 30)) = 0
		_EEEE3("_EEEE3", Range(-30, 30)) = 0*/
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		ZWrite Off
		Blend One One
		LOD 200

		CGPROGRAM
		#pragma surface surf BlinnPhong alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Texture;
		float _InnerRange;
		float _OuterIntensity;
		float _Multiply;
		fixed4 _AdditiveColor;
		fixed4 _SurfaceColor;
		half _Transparency;
		float _SwitchMode;
		
		/*fixed4 _ColorA, _ColorB;
		float _Slide;*/

		struct Input
		{
			float2 uv_Texture;
			float3 viewDir;
			//float3 lightDir;
			//float3 worldPos;
			//float3 worldNormal;
			//float4 screenPos;
		};
		
		float Distance(float2 a, float2 b)
		{
			float2 distance = a - b;
			float magnitude = sqrt(distance.x*distance.x + distance.y*distance.y);
			return magnitude;
		}
		
		//void surf (Input IN, inout SurfaceOutputStandard o) 
		void surf(Input IN, inout SurfaceOutput o)
		{
			////Gradient
			//float t = length(IN.uv_Texture - float2(0.5, 0.5)) * 1.41421356237;
			//fixed4 RadiusGradient = lerp(_ColorA, _ColorB, t + (_Slide - 0.5) * 2);
			
			fixed4 texColor = tex2D(_Texture, IN.uv_Texture);
			fixed4 scalar = saturate(1.0 - (1.0 - Distance(float2(0.5, 0.5), IN.uv_Texture)));
			scalar = pow(scalar, _InnerRange) * _OuterIntensity;
			o.Albedo = _SurfaceColor;
			o.Albedo += scalar * _AdditiveColor * texColor;
			o.Albedo *= _Multiply;
			o.Alpha = texColor.a + _Transparency;
			if(_SwitchMode == 1.0) o.Alpha = (texColor.a * scalar.a) * _Transparency;
		}
	ENDCG
	}
	FallBack "Diffuse"
}