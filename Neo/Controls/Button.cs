namespace Neo.Controls;

public class Button : Control
{
	private float anim = 1;
	private float anim2 = 0;

	//Control
	public string Text;

	public Button(Neo neo) : base(neo, false)
	{
		Text = "btn";
		WantsMouse = true;
		Clicked += Button_Clicked;
		MouseDowned += Button_MouseDowned;
	}

	public Color Color { get; set; } = new(0.9f, 0.3f, 0.0f, 1f);

	private void Button_Clicked(object sender, EventArgs e)
	{
		anim = 0;
		anim2 = 0;
	}

	private void Button_MouseDowned(object sender, EventArgs e)
	{
		anim2 = 1f;
	}

	internal override void SetBounds(Rectangle bounds)
	{
		Bounds = bounds;
	}

	internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
	{
		if (anim < 1f)
		{
			anim += (float) gameTime.ElapsedGameTime.TotalSeconds * 7;
			if (anim > 1f)
				anim = 1f;
		}

		if (IsClipped != true)
		{
			guiBatch.Draw(new Block
			{
				Position = Bounds.Location.ToVector2() + new Vector2(anim2/2, anim2/2+ 10 - 10 * anim),
				Size = Bounds.Size.ToVector2() - new Vector2(anim2, anim2),
				Color = Color.Lerp(Color.White, new Color(0.9f, 0.3f, 0.0f, 1f), (anim+0.4f)-(anim2*0.5f)),
				Radius = 4 + (1 - anim) * 10 + anim2
			});

			if (_neo != null)
				guiBatch.DrawString(Text, Bounds.Location.ToVector2() + new Vector2(10, 5 + (10 - 10 * anim)), 22f,
					Color.White);
			
		}
	}
}