
struct PixelInput {
	float4 position : SV_POSITION;
	float4 color	: COLOR;
	float2 texCoord : TEXCOORD;
};

float CircleProportion = 1.0f;
float4 Color = float4(1, 1, 1, 1);
sampler ColorMapSampler : register(s0);

float4 mainPS(PixelInput input) : SV_TARGET
{
	float4 sample = (float4)0;

	float x = (input.texCoord.x * 2 - 1);
	float y = (input.texCoord.y * 2 - 1);
	float d = x * x + y * y;
	if (d < CircleProportion)
		return float4(0, 0, 0, 0);
	else
		return Color;
}

float4 GrayScale(PixelInput input) : SV_TARGET
{
	float4 color = tex2D(ColorMapSampler, input.texCoord);
	float4 avg_color = (color.r + color.g + color.b) / 3;
	avg_color.a = 1.0f;

	return avg_color;
}

float4 Sepia(PixelInput input) : SV_TARGET
{
	float4 color = tex2D(ColorMapSampler, input.texCoord);
	float4 outputColor = color;
	outputColor.r = (color.r * 0.393) + (color.g * 0.769) + (color.b * 0.189);
	outputColor.g = (color.r * 0.349) + (color.g * 0.686) + (color.b * 0.168);
	outputColor.b = (color.r * 0.272) + (color.g * 0.534) + (color.b * 0.131);

	return outputColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 mainPS();
	}

	pass Pass2
	{
		PixelShader = compile ps_4_0 GrayScale();
	}

	pass Pass3
	{
		PixelShader = compile ps_4_0 Sepia();
	}
}