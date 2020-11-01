using Microsoft.Xna.Framework;
using Neo.Extensions;

namespace Neo.Controls
{
	public class Label : Control
	{
		//Control
		public string Text;
		public Color Color;

		public Label(string text)
		{
			Color = Color.White;
			Text = text;
		}

		//Neo-init
		Neo _neo;
		internal override void Initialize(Neo neo)
		{
			_neo = neo;
			Size = _neo.Style.Font.MeasureString(Text).ToSize() + new Size(20, 10);
		}

		internal override void SetBounds(Rectangle bounds)
		{
			throw new System.NotImplementedException();
		}
	}
}
