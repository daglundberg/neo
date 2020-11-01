using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo.Controls
{
	public class Button : Control
	{
		//Control
		public string Text;

		public Button()
		{
			Text = " ";
		}

		public Button(string text) : this()
		{
			Text = text;
		}


		internal override void Initialize(Neo neo)
		{

		}

		internal override void SetBounds(Rectangle bounds)
		{
			throw new System.NotImplementedException();
		}
	}


}
