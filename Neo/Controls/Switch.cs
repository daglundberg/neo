using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neo.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neo.Controls
{
	public class Switch : Control
	{
		public Switch()
		{
			this.Clicked += Switch_Clicked;
			WantsMouse = true;
		}

		public bool Checked = false;
		Neo _neo;
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_neo.Style.SwitchBg, _position, new Rectangle(0, Checked ? 0:50, 100, 50), Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0); ;
		}

		public override void Initialize(Neo neo, GraphicsDeviceManager graphics)
		{
			_neo = neo;
		}

		private void Switch_Clicked(object sender, EventArgs e)
		{
			Size = new ScreenUnit(100, 50);
			Checked = !Checked;
		}
	}
}
