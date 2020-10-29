float4x4 World;
float4x4 View;
float4x4 Projection;
float Alpha = 1;


struct VertexInput
{
	float4 Position : POSITION;
	float4 Color : TEXCOORD;
};

struct VertexOutput
{
	float4 Position : POSITION;
    float2 Color : TEXCOORD;
};

//-----------------------------------------------------------------------------
// Vertex shaders.
//-----------------------------------------------------------------------------

VertexOutput VS_BasicTexture(VertexInput input)
{
	VertexOutput output;
    output.Position = input.Position;
	output.Color = input.Color;
    return output;
}


//-----------------------------------------------------------------------------
// Pixel shaders.
//-----------------------------------------------------------------------------

float4 PS_Normal(VertexOutput input) : COLOR
{
    //float xDist = abs(input.Color.x - 0.5);
    //float yDist = abs(input.Color.y - 0.5);
	
    //float xD = xDist * xDist * 3.2;
    //float yD = yDist * yDist * 3.2;
    
    //float aha = floor((1 - xD) * (1 - yD) * 5);
    
    //float dx = input.Color.x;
    //float dy = input.Color.y;
    
    //float dist = dx * dx + dy * dy;
    //float distFromCenter = 0.25 - dist;
    
   // float distFromCenter = Hej5(input.Color.xy, 0.2);
   //// float distFromCenter = 100;
   //// float distFromCenter = Hej4(input.Color.xy, 1) - 0.01;
   // float4 outColor = float4(distFromCenter, distFromCenter, distFromCenter, 0);
   //// clip(distFromCenter);

    

   // return outColor;
    
    //float2 fragCoord = input.Color.xy;
    //// The pixel space scale of the rectangle.
    //float2 size = float2(0.1f, 0.1f);
    
    //// the pixel space location of the rectangle.
    //float2 location = float2(0.5, 0.5);

    //// How soft the edges should be (in pixels). Higher values could be used to simulate a drop shadow.
    //float edgeSoftness = 0.5f;
    
    //// The radius of the corners (in pixels).
    //float radius = 0.5; //(sin(iTime) + 1.0f) * 30.0f;
    
    //// Calculate distance to edge.   
    //float distance = roundedBoxSDF(fragCoord.xy - location - (size / 2.0f), size / 2.0f, radius);
    
    //// Smooth the result (free antialiasing).
    //float smoothedAlpha = 1.0f - smoothstep(0.0f, edgeSoftness * 2.0f, distance);
    
    //// Return the resultant shape.
    //float4 quadColor = lerp(float4(1.0f, 1.0f, 1.0f, 1.0f), float4(0.0f, 0.2f, 1.0f, smoothedAlpha), smoothedAlpha);
    
    //// Apply a drop shadow effect.
    //return quadColor;
    
    return float4(0.1, 0.1, 0.1, 0.1);
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