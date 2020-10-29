float2 Scale;

struct InstancingVSinput
{
	float4 Position : POSITION0;
    float2 UV : TEXCOORD1;
};

struct InstancingVSoutput
{
	float4 Position : POSITION;
    float4 Color : COLOR;
    float2 Size : TEXCOORD;
    float2 UV : TEXCOORD1;
};

InstancingVSoutput InstancingVS(InstancingVSinput input, float4 instancePos : POSITION1, float4 Color : COLOR1, float2 Size : TEXCOORD)
{
	InstancingVSoutput output;

    Size = Size * Scale;

    output.Position = input.Position;
    
    output.Position.xy = output.Position.xy * Size.xy + instancePos.xy;
    output.UV = input.UV;
    output.Color = Color;
    output.Size = Size;
	return output;
}

float4 InstancingPS(InstancingVSoutput input) : COLOR0
{
	return input.Color;
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
    float2 Size = 100;
    //Convert our UV position (that go from 0 - 1) to pixel positions relative to the rectangle
    float2 pixelPos = float2(input.UV.x * Size.x, input.UV.y * Size.y);

    // Calculate distance to edge.   
    float distance = roundedBoxSDF(pixelPos - (Size / 2.0f), Size, 10);
    
    clip(0.01 - distance);
    
    // Smooth the result (free antialiasing).
    // How soft the edges should be (in pixels). Higher values could be used to simulate a drop shadow.
    // float edgeSoftness = 0.001;
    // float smoothedAlpha = 1.0f - smoothstep(0.0f, edgeSoftness * 2.0f, distance);
    // Return the resultant shape.
    return input.Color;
}

technique Instancing
{
	pass P0
	{
        VertexShader = compile vs_3_0 InstancingVS();
        PixelShader = compile ps_3_0 InstancingPS();
    }
};