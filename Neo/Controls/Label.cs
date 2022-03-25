namespace Neo.Controls;

public class Label : Control
{
	public Color Color;

	public float FontSize = 30f;

	//Control
	public string Text;

	public Label(Neo neo, string text) : base(neo)
	{
		Color = Color.White;
		Text = text;
	}

	internal override void SetBounds(Rectangle bounds)
	{
		Bounds = bounds;
	}

	internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
	{
		if (_neo != null)
			guiBatch.DrawString(Text, Bounds.Location.ToVector2() + new Vector2(10, 10), FontSize, Color);
	}
}