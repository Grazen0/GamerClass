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
	
	if (uOpacity <= 0)
		return color;
	
	// Scratches
	float rows = 7;
	float columns = 3;
	int frame = round(uTime * 20) % (rows * columns);
	
	if (frame == 1 || frame == 6 || frame == 14 || frame == 18)
	{
		coords.y += 0.0016;
		color = tex2D(uImage0, coords);
	}
	
	float2 noiseCoords = coords;
	
	noiseCoords.y /= rows;
	noiseCoords.y += ((1 / rows) * (frame % rows));
	
	noiseCoords.x /= columns;
	noiseCoords.x += ((1 / columns) * floor(frame / rows));
	
	float4 noise = tex2D(uImage1, noiseCoords);
	color.rgb *= 1 - noise.r;
	
	float luminosity = saturate(lerp(0.5, (color.r + color.g + color.b) / 3, 1.2)); // Saturate luminosity a bit
	luminosity *= clamp(pow(abs((1 - distance(0.5, coords))), uIntensity) * uProgress, 0, 1); // Darken towards borders
	luminosity *= 1 + round(frac(uTime * 4)) * 0.08; // Flash thingy
	
	color.rgb = luminosity * uColor;
	
	return color;
}

technique Technique1
{
	pass Main
	{
		PixelShader = compile ps_2_0 Main();
	}
}
