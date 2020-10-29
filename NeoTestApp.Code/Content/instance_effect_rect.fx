float2 Scale;

struct InstancingVSinput
{
	float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

struct InstancingVSoutput
{
	float4 Position : POSITION;
    float2 UV : TEXCOORD;
    float4 Color : COLOR;
    float2 Size : TEXCOORD2;
    float Radius : TEXCOORD3;
};

InstancingVSoutput InstancingVS(InstancingVSinput input, float4 instancePos : POSITION1, float4 Color : COLOR1, float2 Size : TEXCOORD2, float Radius : TEXCOORD3)
{
	InstancingVSoutput output;
    Size = Size * Scale;

    output.Position = input.Position;

    output.Position.xy = output.Position.xy * Size.xy + instancePos.xy;
    output.UV = input.UV;
    output.Color = Color;
    output.Size = Size;
    output.Radius = Radius;
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
    float2 Size = 100;
    //Convert our UV position (that go from 0 - 1) to pixel positions relative to the rectangle
    float2 pixelPos = float2(input.UV.x * Size.x, input.UV.y * Size.y);

    // Calculate distance to edge.   
    float distance = roundedBoxSDF(pixelPos - (Size / 2.0f), Size, input.Radius);
    
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