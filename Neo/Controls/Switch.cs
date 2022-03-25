namespace Neo.Controls;

public class Switch : Control
{
	private float anim = 1;

	public bool Checked;

	public Switch(Neo neo) : base(neo)
	{
		WantsMouse = true;
		Clicked += Switch_Clicked;
	}

	private void Switch_Clicked(object sender, EventArgs e)
	{
		Checked = !Checked;
		anim = 0;
	}

	internal override void SetBounds(Rectangle bounds)
	{
		Bounds = bounds;
	}

	internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
	{
		if (IsClipped != true)
		{
			if (anim < 1f)
				anim += (float) gameTime.ElapsedGameTime.TotalSeconds * 15;

			guiBatch.Draw(new Block
			{
				Position = Bounds.Location.ToVector2(),
				Size = Bounds.Size.ToVector2(),
				Color = Color.Lerp(Color.Gray, new Color(0.9f, 0.3f, 0.0f, 1f),
					Checked
						? anim
						: 1 - anim), // Checked ? Color.Gray.ToVector4() : Color.MediumSpringGreen.ToVector4(),
				Radius = Bounds.Height / 2
			});


			guiBatch.Draw(new Block
			{
				Position = Bounds.Location.ToVector2() +
				           new Vector2(
					           Checked
						           ? Bounds.Height * 1.125f * anim
						           : Bounds.Height * .125f + (Bounds.Height * 1.125f - anim * (Bounds.Height * 1.125f)),
					           Bounds.Height * .125f),
				Size = new Vector2(Bounds.Height * .75f),
				Color = Color.White,
				Radius = Bounds.Height * .375f
			});
		}
	}
}