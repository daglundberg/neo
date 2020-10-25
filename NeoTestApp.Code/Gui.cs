using Neo.Components;
using Neo;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using Neo.Controls;

namespace NeoTestApp.Code
{
	class Gui
	{
		Neo.Neo _neo;
		Button _btnOk, _btnCancel;
		Switch _switch;
		Label _label;
		public Gui(GraphicsDeviceManager graphics, ContentManager content)
		{
			Style style = new Style(content);
			_neo = new Neo.Neo(graphics, style, Game1.CurrentPlatform);

			_btnOk = new Button("Just a button");
			_btnCancel = new Button("Inside a container");
			_btnOk.Flow = Flow.TopRight;
			_btnCancel.Flow = Flow.TopRight;
			_switch = new Switch();
			_switch.Flow = Flow.BottomLeft;
			_switch.Clicked += swOk_Clicked;
			_label = new Label("Heeey there!");
			_label.Flow = Flow.Top;

			Container container = new Container(new PixelUnit(0, 0), new PixelUnit(500, 900));
			container.Flow = Flow.Center;

			container.AddChild(_btnCancel);

			_btnOk.Clicked += btnOk_Clicked;
			_btnCancel.Clicked += btnCancel_Clicked;

			_neo.AddControl(_btnOk);
			_neo.AddControl(container);
			_neo.AddControl(_switch);
			_neo.AddControl(_label);
			_neo.Calculate();
		}

		private void swOk_Clicked(object sender, EventArgs e)
		{
			if (_switch.Checked)
			{
				_neo.ScaleMultiplier = 1.5f;
				_neo.Calculate();
			}
			else
			{
				_neo.ScaleMultiplier = 1;
				_neo.Calculate();
			}
			
		}

		private void btnCancel_Clicked(object sender, EventArgs e)
		{
			((Button)sender).Text = "Thanks!";
			_btnOk.Text = "Click me!";
		}

		private void btnOk_Clicked(object sender, EventArgs e)
		{
			((Button)sender).Text = "Thanks!";
			_btnCancel.Text = "Click me!";
		}

		public void Calculate()
		{
			_neo.Calculate();
		}

	//	public bool WantsMouse(Point point)
	//	{
	//		return _neo.WantsMouse(point);
	//	}

		public void Click(Point point)
		{
			_neo.Click(point);
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
