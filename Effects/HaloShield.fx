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
	
	float luminosity = (color.r + color.g + color.b) / 2;
	color.rgb = luminosity * uColor;
	
	float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
	
	color.rgb *= 1 - frac(0.3 - coords.x + uTime * 1.4) / 6; // Horizontal forward
	color.rgb *= 1 - frac(frameY + uTime * 0.75) / 8; // Vertical upwards
	color.rgb *= 1 - frac(coords.x + frameY + uTime) / 6; // Diagonal #1
	color.rgb *= 1 - frac((0.6 - coords.x) * frameY + uTime * 0.7) / 8; // Diagonal #2
	color.rgb *= 1 - frac(coords.x * (1 - frameY) + uTime * 0.5) / 6; // Diagonal #3
	color.rgb *= 1 - frac(distance(float2(coords.x, frameY), 0.5) * 3 - uTime) / 4; // Elliptical
	
	return color * sampleColor;
}
    
technique Technique1
{
	pass Main
	{
		PixelShader = compile ps_2_0 Main();
	}
}