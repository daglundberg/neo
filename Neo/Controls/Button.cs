namespace Neo.Controls;

public class Button : Control
{
	private float anim = 1;

	//Control
	public string Text;

	public Button(Neo neo) : base(neo, false)
	{
		Text = "btn";
		WantsMouse = true;
		Clicked += Button_Clicked;
	}

	public Color Color { get; set; } = new(0.9f, 0.3f, 0.0f, 1f);

	private void Button_Clicked(object sender, EventArgs e)
	{
		anim = 0;
	}

	internal override void SetBounds(Rectangle bounds)
	{
		Bounds = bounds;
	}

	internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
	{
		if (anim < 1f)
			anim += (float) gameTime.ElapsedGameTime.TotalSeconds * 8;

		if (IsClipped != true)
		{
			guiBatch.Draw(new Block
			{
				Position = Bounds.Location.ToVector2() + new Vector2(0, 10 - 10 * anim),
				Size = Bounds.Size.ToVector2(),
				Color = Color.Lerp(Color.Gray, new Color(0.9f, 0.3f, 0.0f, 1f), anim),
				Radius = 4 + (1 - anim) * 10
			});

			if (_neo != null)
				guiBatch.DrawString(Text, Bounds.Location.ToVector2() + new Vector2(10, 5 + (10 - 10 * anim)), 22f,
					Color.White);
		}
	}
}