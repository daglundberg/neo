using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Neo.Components;
using System.Collections.Generic;

namespace Neo
{
    public partial class Neo
	{
		private List<Control> _controls;
		Point _screenSize;
		public Style Style { get; private set; }

		public Neo(Style style, Point screenSize)
		{
			_controls = new List<Control>();
			Style = style;
			_screenSize = screenSize;
		}

		public void AddControl(Control control)
		{
			_controls.Add(control);
		}

		public void Calculate(float scale)
		{
			foreach (Control control in _controls)
				ComputeControl(control, new Rectangle(0,0, _screenSize.X, _screenSize.Y), scale);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Control control in _controls)
				DrawControl(control, spriteBatch);
		}

		private void ComputeControl(Control control, Rectangle parentBounds, float scale)
		{
			control.Initialize(this);
			control.SetPositionAndScale(parentBounds, scale);

			foreach (Control child in control)
				ComputeControl(child, control.Bounds, scale);
		}

		private void DrawControl(Control control, SpriteBatch spriteBatch)
		{
			control.Draw(spriteBatch);

			foreach (Control child in control)
				DrawControl(child, spriteBatch);
		}
	}

	public class Style
	{
		ContentManager _content;
		public Style(ContentManager content)
		{
			_content = content;
			ButtonBg = content.Load<Texture2D>("buttonbg");
			Font = content.Load<SpriteFont>("fonts/patuaone-big");
		}

		public Texture2D ButtonBg;
		public SpriteFont Font;
	}

	public enum Flow
	{
		Center,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		BottomLeft,
		Left
	}
}
