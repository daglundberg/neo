float4x4 MatrixTransform;

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
    float distance = roundedBoxSDF(pixelPos - (input.Size / 2.0f), input.Size, input.Radius);
	
   // clip(1 - distance);
	
	// Smooth the result (free antialiasing).
	// How soft the edges should be (in pixels). Higher values could be used to simulate a drop shadow.

	return input.Color * smoothstep(1, 0, distance);
   // return input.Color;
}

technique Instancing
{
	pass P0
	{
        VertexShader = compile vs_3_0 InstancingVS();
        PixelShader = compile ps_3_0 RoundedBlockPS();
    }
};