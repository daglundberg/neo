float4x4 World;
float4x4 View;
float4x4 Projection;
float Alpha = 1;
float Time;

texture ColorMap;
texture FlowMap;
sampler2D colorMapSampler = sampler_state
{
	Texture = <ColorMap>;
    AddressU = clamp;
    AddressV = clamp;
};

sampler flowMapSampler = sampler_state
{
    Texture = <FlowMap>;
};

struct VertexInput
{
	float4 Position : POSITION;
	float4 TexCoord : TEXCOORD0;
};

struct VertexOutput
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
};

//-----------------------------------------------------------------------------
// Vertex shaders.
//-----------------------------------------------------------------------------

VertexOutput VS_BasicTexture(VertexInput input)
{
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);

	VertexOutput output;
	output.Position = mul(viewPosition, Projection);

	output.TexCoord = input.TexCoord;
	return output;
}

//-----------------------------------------------------------------------------
// Pixel shaders.
//-----------------------------------------------------------------------------

float4 PS_Normal(VertexOutput input) : COLOR
{
    float alphaTestDirection = 1.0f;
    float alphaTestThreshold = 0.95f;
    float4 outColor = tex2D(colorMapSampler, input.TexCoord);

    outColor.rgba *= Alpha;

    return outColor;
}

float4 PS_Water(VertexOutput input) : COLOR
{
    float alphaTestDirection = 1.0f;
    float alphaTestThreshold = 0.95f;

    float2 flow = tex2D(flowMapSampler, input.TexCoord).xy;
    flow = (flow - 0.5) * 2.0;
    float nice_time_phase1 = frac(Time*0.14);
    float nice_time_phase2 = frac(nice_time_phase1 + 0.5);
    float nice_mix = abs((nice_time_phase1 - 0.5) * 2.0);
   //  float2 nice_uv = input.TexCoord + (flow * nice_time);
    
    float4 tex1 = tex2D(colorMapSampler, input.TexCoord + (flow * nice_time_phase1));
    float4 tex2 = tex2D(colorMapSampler, input.TexCoord + (flow * nice_time_phase2));
    float4 outColor = lerp(tex1, tex2, nice_mix);
   // float4 outColor = tex2D(colorMapSampler, nice_uv);
	//clip((outColor.a - alphaTestThreshold) * alphaTestDirection);
	
    outColor.rgba *= Alpha;
    
    return outColor;
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

technique Water
{
    pass
    {
        VertexShader = compile vs_3_0 VS_BasicTexture();
        PixelShader = compile ps_3_0 PS_Water();
    }
}