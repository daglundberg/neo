namespace Neo;

public struct BlockVertex
{
	public Vector2 Position;
	public Vector2 TextureCoordinate;
	public Vector2 Size;
	public Vector4 Color;
	public float Radius;
}

public struct Block
{
	public Vector2 Position;
	public Vector2 Size;
	public Color Color;
	public float Radius;
}

public struct CustomBlock
{
	public Vector2 Position;
	public Vector2 Size;
	public Color Color;
	public float RadiusTL;
	public float RadiusTR;
	public float RadiusBL;
	public float RadiusBR;
}

internal class NeoBatchItem
{
	public enum ItemType
	{
		Texture,
		Block,
		Glyph
	}

	public Texture2D Texture;
	public ItemType Type;
	public BlockVertex vertexBL;
	public BlockVertex vertexBR;

	public BlockVertex vertexTL;
	public BlockVertex vertexTR;

	public NeoBatchItem()
	{
		vertexTL = new BlockVertex();
		vertexTR = new BlockVertex();
		vertexBL = new BlockVertex();
		vertexBR = new BlockVertex();
	}

	public void Set(float x, float y, float dx, float dy, float w, float h, float sin, float cos, Color color,
		Vector2 texCoordTL, Vector2 texCoordBR)
	{
		Type = ItemType.Texture;
		vertexTL.Position.X = x + dx * cos - dy * sin;
		vertexTL.Position.Y = y + dx * sin + dy * cos;
		vertexTL.Color = color.ToVector4();
		vertexTL.TextureCoordinate.X = texCoordTL.X;
		vertexTL.TextureCoordinate.Y = texCoordTL.Y;

		vertexTR.Position.X = x + (dx + w) * cos - dy * sin;
		vertexTR.Position.Y = y + (dx + w) * sin + dy * cos;
		vertexTR.Color = color.ToVector4();
		vertexTR.TextureCoordinate.X = texCoordBR.X;
		vertexTR.TextureCoordinate.Y = texCoordTL.Y;

		vertexBL.Position.X = x + dx * cos - (dy + h) * sin;
		vertexBL.Position.Y = y + dx * sin + (dy + h) * cos;
		vertexBL.Color = color.ToVector4();
		vertexBL.TextureCoordinate.X = texCoordTL.X;
		vertexBL.TextureCoordinate.Y = texCoordBR.Y;

		vertexBR.Position.X = x + (dx + w) * cos - (dy + h) * sin;
		vertexBR.Position.Y = y + (dx + w) * sin + (dy + h) * cos;
		vertexBR.Color = color.ToVector4();
		vertexBR.TextureCoordinate.X = texCoordBR.X;
		vertexBR.TextureCoordinate.Y = texCoordBR.Y;
	}

	public void Set(float x, float y, float w, float h, Color color, Vector2 texCoordTL, Vector2 texCoordBR)
	{
		Type = ItemType.Texture;
		vertexTL.Position.X = x;
		vertexTL.Position.Y = y;
		vertexTL.Color = color.ToVector4();
		vertexTL.TextureCoordinate.X = texCoordTL.X;
		vertexTL.TextureCoordinate.Y = texCoordTL.Y;

		vertexTR.Position.X = x + w;
		vertexTR.Position.Y = y;
		vertexTR.Color = color.ToVector4();
		vertexTR.TextureCoordinate.X = texCoordBR.X;
		vertexTR.TextureCoordinate.Y = texCoordTL.Y;

		vertexBL.Position.X = x;
		vertexBL.Position.Y = y + h;
		vertexBL.Color = color.ToVector4();
		vertexBL.TextureCoordinate.X = texCoordTL.X;
		vertexBL.TextureCoordinate.Y = texCoordBR.Y;

		vertexBR.Position.X = x + w;
		vertexBR.Position.Y = y + h;
		vertexBR.Color = color.ToVector4();
		vertexBR.TextureCoordinate.X = texCoordBR.X;
		vertexBR.TextureCoordinate.Y = texCoordBR.Y;
	}

	public void SetBlock(float x, float y, float w, float h, Color color, Vector2 texCoordTL, Vector2 texCoordBR,
		float radius)
	{
		Type = ItemType.Block;
		vertexTL.Position.X = x;
		vertexTL.Position.Y = y;
		vertexTL.Color = color.ToVector4();
		vertexTL.TextureCoordinate.X = texCoordTL.X;
		vertexTL.TextureCoordinate.Y = texCoordTL.Y;
		vertexTL.Size.X = w;
		vertexTL.Size.Y = h;
		vertexTL.Radius = radius;

		vertexTR.Position.X = x + w;
		vertexTR.Position.Y = y;
		;
		vertexTR.Color = color.ToVector4();
		vertexTR.TextureCoordinate.X = texCoordBR.X;
		vertexTR.TextureCoordinate.Y = texCoordTL.Y;
		vertexTR.Size.X = w;
		vertexTR.Size.Y = h;
		vertexTR.Radius = radius;

		vertexBL.Position.X = x;
		vertexBL.Position.Y = y + h;
		vertexBL.Color = color.ToVector4();
		vertexBL.TextureCoordinate.X = texCoordTL.X;
		vertexBL.TextureCoordinate.Y = texCoordBR.Y;
		vertexBL.Size.X = w;
		vertexBL.Size.Y = h;
		vertexBL.Radius = radius;

		vertexBR.Position.X = x + w;
		vertexBR.Position.Y = y + h;
		vertexBR.Color = color.ToVector4();
		vertexBR.TextureCoordinate.X = texCoordBR.X;
		vertexBR.TextureCoordinate.Y = texCoordBR.Y;
		vertexBR.Size.X = w;
		vertexBR.Size.Y = h;
		vertexBR.Radius = radius;
	}
	
	public void SetCustomBlock(float x, float y, float w, float h, Vector2 texCoordTL, Vector2 texCoordBR,
		float radiusTL, float radiusTR, float radiusBL, float radiusBR, Color colorTR, Color colorTL, Color colorBL, Color colorBR)
	{
		Type = ItemType.Block;
		vertexTL.Position.X = x;
		vertexTL.Position.Y = y;
		vertexTL.Color = colorTR.ToVector4();
		vertexTL.TextureCoordinate.X = texCoordTL.X;
		vertexTL.TextureCoordinate.Y = texCoordTL.Y;
		vertexTL.Size.X = w;
		vertexTL.Size.Y = h;
		vertexTL.Radius = radiusTL;

		vertexTR.Position.X = x + w;
		vertexTR.Position.Y = y;
		;
		vertexTR.Color = colorTL.ToVector4();
		vertexTR.TextureCoordinate.X = texCoordBR.X;
		vertexTR.TextureCoordinate.Y = texCoordTL.Y;
		vertexTR.Size.X = w;
		vertexTR.Size.Y = h;
		vertexTR.Radius = radiusTR;

		vertexBL.Position.X = x;
		vertexBL.Position.Y = y + h;
		vertexBL.Color = colorBL.ToVector4();
		vertexBL.TextureCoordinate.X = texCoordTL.X;
		vertexBL.TextureCoordinate.Y = texCoordBR.Y;
		vertexBL.Size.X = w;
		vertexBL.Size.Y = h;
		vertexBL.Radius = radiusBL;

		vertexBR.Position.X = x + w;
		vertexBR.Position.Y = y + h;
		vertexBR.Color = colorBR.ToVector4();
		vertexBR.TextureCoordinate.X = texCoordBR.X;
		vertexBR.TextureCoordinate.Y = texCoordBR.Y;
		vertexBR.Size.X = w;
		vertexBR.Size.Y = h;
		vertexBR.Radius = radiusBR;
	}
}