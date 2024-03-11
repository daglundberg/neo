using Microsoft.Xna.Framework.Input;

namespace Neo.Controls;

public class TextBox : Control
{
	public Color Color;

	public float FontSize = 30f;

	private double animationFrame;
	
	//Control
	public string Text;

	public TextBox(Neo neo, string text) : base(neo)
	{
		Color = Color.White;
		Text = text;
	}

	internal override void SetBounds(Rectangle bounds)
	{
		Bounds = bounds;
	}

	public override void KeyPress(Keys[] key)
	{
		if (key.Length > 0)
		{
			var c = (char) key[0];
			if (c >= 65 && c <= 90 || c == 32)
				Text += c;

			else if (key[0] == Keys.Delete || key[0] == Keys.Back)
				if (Text.Length > 0)
					Text = Text.Substring(0, Text.Length - 1);
		}
	}

	internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
	{
		if (_neo != null)
		{
			animationFrame += gameTime.ElapsedGameTime.TotalSeconds;
			guiBatch.Draw(new Block(){Color = Color.Blue, Position = Bounds.Location.ToVector2(), Size = Bounds.Size.ToVector2()});
			var adv = guiBatch.DrawString(Text, Bounds.Location.ToVector2() + new Vector2(10, 10), FontSize, Color);

			if (animationFrame > 1)
				animationFrame = 0;
			
			if (animationFrame > 0.5f)
				guiBatch.Draw(new Block(){Color = Color.Red, Position = Bounds.Location.ToVector2() + new Vector2(adv + 10, 10), Size = new Vector2(3, 35)});
		}
	}
}