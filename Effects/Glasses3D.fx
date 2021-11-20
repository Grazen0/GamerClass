sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 Main(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	
	if (uProgress <= 0)
		return color;
    
	float4 rightColor = tex2D(uImage0, float2(coords.x + uProgress / uScreenResolution.x, coords.y));
    
	float luminosity = (color.r + color.g + color.b) / 3;
	float rightLuminosity = (rightColor.r + rightColor.g + rightColor.b) / 3;
    
	color.rgb = luminosity;
	
	bool paintRed = luminosity < uOpacity;
	bool paintCyan = rightLuminosity < uOpacity;
	
	if (paintRed && !paintCyan)
	{
		color.rgb *= float3(1 - luminosity, 0, 0) * 3;
	}
	else if (paintCyan && !paintRed)
	{
		color.rgb *= float3(0, 1, 1);
	}
    
	return color;
}

technique Technique1
{
	pass Main
	{
		PixelShader = compile ps_2_0 Main();
	}
}
