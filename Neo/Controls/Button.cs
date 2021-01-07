using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo.Controls
{
	public class Button : Control
	{
		//Control
		public string Text;
		public Color Color { get; set; } = new Color(0.9f, 0.3f, 0.0f, 1f);

		public Button() : base (false)
		{
			Text = " ";
			WantsMouse = true;
			Clicked += Button_Clicked;
		}


		public Button(Neo neo) : base(false)
		{
			_neo = neo;
			WantsMouse = true;
			Clicked += Button_Clicked;
		}

		float anim = 1;
		private void Button_Clicked(object sender, EventArgs e)
		{
			anim = 0;
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;			
		}

		internal override void Draw(GameTime gameTime, GuiBatch guiBatch)
		{
			if (anim < 1f)
				anim += (float)gameTime.ElapsedGameTime.TotalSeconds*8;

			if (IsClipped != true)
			{
				guiBatch.DrawBlock(new Block
				{
					Position = Bounds.Location.ToVector2() + new Vector2(0, 10 - 10 * anim),
					Size = Bounds.Size.ToVector2(),
					Color = Vector4.Lerp(Color.Gray.ToVector4(), new Vector4(0.9f, 0.3f, 0.0f, 1f), anim),
					Radius = 4 + ((1 - anim) * 10)
				});

				if (_neo != null)
					guiBatch.DrawString(_neo.DefaultFont, Text, Bounds.Location.ToVector2() + new Vector2(10, 5+(10 - 10 * anim)), 22f, Color.White);
			}

		}
	}
}
