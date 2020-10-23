using Neo.Components;
using Neo;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography.X509Certificates;
using Neo.Controls;

namespace ScreenTest
{
	class Gui
	{
		Neo.Neo _neo;
		Button _btnOk, _btnCancel;
		Switch swOk;
		public Gui(ContentManager content, Point screenSize)
		{
			Style style = new Style(content);
			_neo = new Neo.Neo(style, screenSize);

			_btnOk = new Button("Click me!");
			_btnCancel = new Button("Cancel.");
			_btnOk.Flow = Flow.TopRight;
			_btnCancel.Flow = Flow.TopRight;
			swOk = new Switch();
			swOk.Flow = Flow.Center;

			Container container = new Container(new PixelUnit(0, 0), new PixelUnit(500, 900));
			container.Flow = Flow.Center;

			container.AddChild(_btnCancel);

			_btnOk.Clicked += btnOk_Clicked;
			_btnCancel.Clicked += btnOk_Clicked;

			_neo.AddControl(swOk);
		//	_neo.AddControl(_btnOk);
		//	_neo.AddControl(container);
			_neo.Calculate(1f);
		}

		private void btnOk_Clicked(object sender, EventArgs e)
		{
			((Button)sender).Text = "Clicked!";
		}

		public void Update(GameTime gameTime)
		{
			_neo.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			_neo.Draw(spriteBatch);
		}
	}
}
