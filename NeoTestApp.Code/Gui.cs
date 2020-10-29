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
			_switch = new Switch() { Flow = Flow.Left, PositionOffset = new ScreenUnit(0, 0) };
			_switch.Clicked += swOk_Clicked;

			_label = new Label("Heeey there!") { Color = Color.Aquamarine, Flow = Flow.Top };

			_btnOk.Clicked += btnCancel_Clicked;

			/*						_neo.AddControls(new Control[]{
										new Container(){ Size = new ScreenUnit(200, 200), Flow = Flow.Bottom },
										_btnOk,
										_switch,
										_label });*/

			MessageBox elenasMessageBox = new MessageBox("Do you want to take \nElena out on a date?", ElenasAction, MessageBox.Result.Yes, MessageBox.Result.Yes, MessageBox.Result.Yes);
			_neo.AddControl(elenasMessageBox);
/*			MessageBox _messageBox = new MessageBox("Hey there dude!", RunThisCode, MessageBox.Result.Yes, MessageBox.Result.Cancel, MessageBox.Result.No);
			_neo.AddControl(_messageBox);*/
		}

		private void ElenasAction(MessageBox.Result result)
		{

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
			MessageBox _messageBox = new MessageBox("Hey there dude!", RunThisCode, MessageBox.Result.Yes, MessageBox.Result.Cancel, MessageBox.Result.No);
			_neo.AddControl(_messageBox);
		}

		private void RunThisCode(MessageBox.Result result)
		{
			MessageBox messageBox = new MessageBox("You clicked " + result.ToString());
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
