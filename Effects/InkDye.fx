sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 Main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	
	float2 noiseCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
	noiseCoords.y -= frac(uTime * 0.1);
	
	if (noiseCoords.y < 0)
		noiseCoords.y += 1;
	
	float4 noise = tex2D(uImage1, noiseCoords);
	
	if (uOpacity > 0)
	{
		float luminosity = saturate(lerp(0.5, (color.r + color.g + color.b) / 3, 1.2));
		color.rgb = luminosity * uColor;
	}
	
	if (noise.r <= 0.2)
		color.rgb *= noise.r;
	
	return color * sampleColor;
}

technique Technique1
{
	pass Main
	{
		PixelShader = compile ps_2_0 Main();
	}
}
