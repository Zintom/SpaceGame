#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

float radius;
float diameter;
float4 circleColor;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 DrawCircle(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
	
	float2 pos = input.TextureCoordinates.xy;
	
	// Convert to texture coordinates rather than normalized coords.
	float posU = pos.x * diameter;
	float posV = pos.y * diameter;
	float2 pixelPos = float2(posU, posV);
	float2 circleCenter = float2(radius, radius);
	
	// Pythagoras theorum to calculate distance. Distance = Sqrt(x^2 + y^2)
	float currentPixelDistanceToCenter = sqrt((circleCenter.x - pixelPos.x) * (circleCenter.x - pixelPos.x) + (circleCenter.y - pixelPos.y) * (circleCenter.y - pixelPos.y));
	
	// In the radius of the circle.
	if (currentPixelDistanceToCenter < radius)
	{
        color.rgba = circleColor;
    }
	
	return color;
}

technique SpriteDrawing
{
	pass PDrawCircle
	{
		PixelShader = compile PS_SHADERMODEL DrawCircle();
	}
}