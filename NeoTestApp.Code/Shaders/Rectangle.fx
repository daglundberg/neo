float4 Color2;


struct VertexInput
{
	float4 Position : POSITION;
	float4 TextCoord : TEXCOORD;
};

struct VertexOutput
{
	float4 Position : POSITION;
    float2 TextCoord : TEXCOORD;
};

//-----------------------------------------------------------------------------
// Vertex shaders.
//-----------------------------------------------------------------------------

VertexOutput VS_BasicTexture(VertexInput input)
{
	VertexOutput output;
    output.Position = input.Position;
    output.TextCoord = input.TextCoord;
    return output;
}


//-----------------------------------------------------------------------------
// Pixel shaders.
//-----------------------------------------------------------------------------

float4 PS_Normal(VertexOutput input) : COLOR
{
    return Color2;
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique Normal
{
	pass
	{
        VertexShader = compile vs_3_0 VS_BasicTexture();
        PixelShader = compile ps_3_0 PS_Normal();
    }
}