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
		Button _btnOk;
		Switch _switch;
		Label _label;
		public Gui(GraphicsDeviceManager graphics, ContentManager content)
		{
			Style style = new Style(content);
			_neo = new Neo.Neo(graphics, style, Game1.CurrentPlatform);

			_btnOk = new Button("Hey hey") { Flow = Flow.TopRight };
			_switch = new Switch() { Flow = Flow.Left, PositionOffset = new PixelUnit(0, 0) };
			_switch.Clicked += swOk_Clicked;


			_label = new Label("Heeey there!") { Color = Color.Aquamarine, Flow = Flow.Top };

			_btnOk.Clicked += btnCancel_Clicked;

			_neo.AddControls(new Control[]{
				_btnOk,
				_switch,
				_label });
		}

		private void swOk_Clicked(object sender, EventArgs e)
		{ 
			if (_switch.Checked)
				_neo.ScaleMultiplier = 1.5f;
			else
				_neo.ScaleMultiplier = 1;
		}

		private void btnCancel_Clicked(object sender, EventArgs e)
		{
			((Button)sender).Text = "Thanks!";
			_btnOk.Text = "Click me!";
			MessageBox _messageBox = new MessageBox("Hey there dude!", RunThisCode);
			_neo.AddControl(_messageBox);
		}

		private void RunThisCode(MessageBox.Result result)
		{
			MessageBox messageBox = new MessageBox("you clicked " + result.ToString());
			_neo.AddControl(messageBox);
		}

		public void Calculate()
		{
			_neo.Calculate(true);
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
