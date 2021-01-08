texture tex;
float4x4 MatrixTransform;

sampler2D colorMapSampler = sampler_state
{
    Texture = <tex>;
    AddressU = clamp;
    AddressV = clamp;
};


struct InstancingVSinput
{
	float4 Position : POSITION;
	float2 UV : TEXCOORD;
    float2 InstancePos : POSITION1;
    float4 Color : COLOR;
    float2 Size : TEXCOORD2;
    float Radius : TEXCOORD3;
};


struct InstancingVSoutput
{
	float4 Position : POSITION;
	float2 UV : TEXCOORD;
	float4 Color : COLOR;
	float2 Size : TEXCOORD2;
	float Radius : TEXCOORD3;
};


struct BasicVSinput
{
    float4 Position : POSITION;
    float2 UV : TEXCOORD;
};

struct BasicVSoutput
{
    float4 Position : POSITION;
    float2 UV : TEXCOORD;
};



InstancingVSoutput InstancingVS(InstancingVSinput input)
{
    InstancingVSoutput output;
    input.Position.xy = input.Position.xy * input.Size + input.InstancePos;
    output.Position = mul(input.Position, MatrixTransform);
	
    output.UV = input.UV;
    output.Color = input.Color;

    output.Size = input.Size;
    output.Radius = input.Radius;
   
    return output;
}


//Signed Distance Field
//Returns how far a pixel is from the edge of the shape
float roundedBoxSDF(float2 centerPosition, float2 size, float radius)
{
	return length(max(abs(centerPosition) - (size / 2) + radius, 0)) - radius;
}

//-----------------------------------------------------------------------------
// Pixel shader.
//-----------------------------------------------------------------------------
float4 RoundedBlockPS(InstancingVSoutput input) : COLOR0
{
	//Convert our UV position (that go from 0 - 1) to pixel positions relative to the rectangle
    float2 pixelPos = float2(input.UV.x * input.Size.x, input.UV.y * input.Size.y);

	// Calculate distance to edge.   
    float distance = roundedBoxSDF(pixelPos - (input.Size / 2.0f), input.Size, input.Radius) +1;
	
   // clip(1 - distance);
	
	// Smooth the result (free antialiasing).
	// How soft the edges should be (in pixels). Higher values could be used to simulate a drop shadow.

	return input.Color * smoothstep(1, 0, distance);
   // return input.Color;
}

BasicVSoutput BasicVS(BasicVSinput input)
{
    BasicVSoutput output;

    output.Position = mul(input.Position, MatrixTransform);
    output.UV = input.UV;
    return output;
}

float4 SpritePixelShader(InstancingVSoutput input) : SV_Target0
{
    float4 outColor = tex2D(colorMapSampler, input.UV);
    return outColor;
}

float2 SafeNormalize(in float2 v)
{
    float len = length(v);
    len = (len > 0.0) ? 1.0 / len : 0.0;
    return v * len;
}

float Median(float a, float b, float c)
{
    return max(min(a, b), min(max(a, b), c));
}

float4 msdfPS(BasicVSoutput input) : COLOR
{
	// Convert normalized texture coordinates to absolute texture coordinates
    float2 uv = input.UV * 50;

	// Calculate derivatives
    float2 Jdx = ddx(uv);
    float2 Jdy = ddy(uv);

	// Sample texture
    float3 samp = tex2D(colorMapSampler, input.UV).rgb;

	// Calculate the signed distance (in texels)
    float sigDist = Median(samp.r, samp.g, samp.b) - 0.5f;

	// For proper anti-aliasing we need to calculate the signed distance in pixels.
	// We do this using the derivatives.	
    float2 gradDist = SafeNormalize(float2(ddx(sigDist), ddy(sigDist)));
    float2 grad = float2(gradDist.x * Jdx.x + gradDist.y * Jdy.x, gradDist.x * Jdx.y + gradDist.y * Jdy.y);

	// Apply anti-aliasing
    const float thickness = 0.9f;
    const float normalization = thickness * 0.5f * sqrt(2.0f);

    float afWidth = min(normalization * length(grad), 0.5f);
    float opacity = smoothstep(0.0f - afWidth, 0.0f + afWidth, sigDist);

	// Apply pre-multiplied alpha with gamma correction

    float4 color;
    color.a = pow(abs(1 * opacity), 1.0f / 2.2f);
    color.rgb = float3(1, 1, 1) * color.a;

    return color;
}

technique Instancing
{
	pass P0
	{
        VertexShader = compile vs_3_0 InstancingVS();
        PixelShader = compile ps_3_0 RoundedBlockPS();
    }
};

technique InstancingTexture
{
    pass P0
    {
        VertexShader = compile vs_3_0 InstancingVS();
        PixelShader = compile ps_3_0 SpritePixelShader();
    }
};

technique BasicTexture
{
    pass P0
    {
        VertexShader = compile vs_3_0 BasicVS();
        PixelShader = compile ps_3_0 msdfPS();
    }
};