texture tex;
float4x4 MatrixTransform;

sampler2D colorMapSampler = sampler_state
{
    Texture = <tex>;
    AddressU = clamp;
    AddressV = clamp;
};

struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
    float2 texCoord		: TEXCOORD0;
};

VSOutput SpriteVertexShader(float4 position : POSITION0, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	VSOutput output;
    output.position = mul(position, MatrixTransform);
	output.color = color;
	output.texCoord = texCoord;
	return output;
}


float4 SpritePixelShader(VSOutput input) : SV_Target0
{    
    float4 outColor = tex2D(colorMapSampler, input.texCoord);
    return outColor;
}

technique Normal
{
    pass
    {
        VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 SpritePixelShader();
    }
}