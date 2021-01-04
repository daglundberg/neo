using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neo.Controls
{
	public class Switch : Control
	{
		public Switch()
		{
			WantsMouse = true;
			Clicked += Switch_Clicked;
		}

		public bool Checked = false;

		internal override void Initialize(Neo neo)
		{

		}

		private void Switch_Clicked(object sender, EventArgs e)
		{
			Size = new Size(100, 50);
			Checked = !Checked;
			anim = 0;
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
		}

		float anim = 1;
		internal override void Draw(GameTime gameTime, GuiBatch guiBatch)
		{
			if (IsClipped != true)
			{
				if (anim < 1f)
					anim += (float)gameTime.ElapsedGameTime.TotalSeconds*15;

				guiBatch.DrawBlock(new Block { 
					Position = Bounds.Location.ToVector2(),
					Size = Bounds.Size.ToVector2(),
					Color = Vector4.Lerp(Color.Gray.ToVector4(), new Vector4(0.9f, 0.3f, 0.0f, 1f), Checked ? anim : 1 - anim), // Checked ? Color.Gray.ToVector4() : Color.MediumSpringGreen.ToVector4(),
					Radius = Bounds.Height/2 });
				

				guiBatch.DrawBlock(new Block {
					Position = Bounds.Location.ToVector2() + new Vector2( Checked ? (Bounds.Height * 1.125f) * anim: (Bounds.Height * .125f) + ((Bounds.Height * 1.125f) - (anim * (Bounds.Height * 1.125f))), (Bounds.Height * .125f)),
					Size = new Vector2(Bounds.Height * .75f),
					Color = Color.White.ToVector4(),
					Radius = Bounds.Height * .375f });
			}
		}
	}
}
