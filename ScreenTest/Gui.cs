using Neo.Components;
using Neo;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ScreenTest
{
	class Gui
	{
		Neo.Neo _neo;
		public Gui(ContentManager content, Point screenSize)
		{
			Style style = new Style(content);
			_neo = new Neo.Neo(style, screenSize);

			Button btnOk = new Button("Click me!");
			Button btnCancel = new Button("Cancel.");
			btnOk.Flow = Flow.TopRight;
			btnCancel.Flow = Flow.TopRight;

			Container container = new Container(new PixelUnit(0, 0), new PixelUnit(500, 900));
			container.Flow = Flow.Center;

			//container.AddChild();
			container.AddChild(btnCancel);

			_neo.AddControl(btnOk);
			_neo.AddControl(container);
			_neo.Calculate(1f);
		}

		public void Update()
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			_neo.Draw(spriteBatch);
		}
	}
}
