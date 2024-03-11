﻿using Neo;
using Neo.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using static Neo.Controls.MessageBox;

namespace NeoTestApp.Code
{
	class Gui : DrawableGameComponent
	{
		Neo.Neo _neo;
		Button btnClickMe;
		TextBox txtBox;
		Row row;

		public Gui(Game game) : base(game)
		{

			_neo = new Neo.Neo(game, Game1.CurrentPlatform);

			btnClickMe = new Button(_neo);
			btnClickMe.Text = "Open";
			btnClickMe.Size = new Size(110, 38);
			btnClickMe.Margins = new Margins(50);
			btnClickMe.Anchors = Anchors.Left;
			btnClickMe.Clicked += BtnClickMe_Clicked;

			txtBox = new TextBox(_neo, "Hejsan du")
			{
				Size = new Size(120, 40)
			};

			row = new Row(_neo);
			row.Anchors = Anchors.Left | Anchors.Right | Anchors.Top;
			row.Size = new Size(50);
			row.Margins = new Margins(20);

			_neo.AddChildren(
				new Control[]
				{
					txtBox,
					btnClickMe,
					row
				});

			_neo.Create();
			_neo.FocusedControl = txtBox;
		}
		
		private void BtnClickMe_Clicked(object sender, System.EventArgs e)
		{
			row.Margins = new Margins(50, 100, 50, 50);
			MessageBox msg = new MessageBox(_neo, $"Title", "This is a message with\nmultiple lines.. \n\nDo you like it?", new Result[] { Result.Yes, Result.No });
			msg.Size = new Size(350, 240);
			msg.Closed += Msg_Closed;
			_neo.AddChild(msg);
			_neo.ForceRefresh();			
		}

		private void Msg_Closed(object sender, System.EventArgs e)
		{
			btnClickMe.Text = ((Result)sender).ToString();
		}

		public override void Draw(GameTime gameTime)
		{
			_neo.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			CheckMouse();
			CheckTouch();
			CheckKeyboard();
			base.Update(gameTime);
		}
		
		private void CheckKeyboard()
		{
			var pressedKeys = Keyboard.GetState().GetPressedKeys();
			_neo.FocusedControl?.KeyPress(pressedKeys);
		}

		MouseState _oldMouseState, _newMouseState;
		private void CheckMouse()
		{
			_oldMouseState = _newMouseState;
			_newMouseState = Mouse.GetState();

			//Check if a mouse click occured
			if (_oldMouseState.LeftButton == ButtonState.Pressed && _newMouseState.LeftButton == ButtonState.Released)
			{
				//Handle click
				if (_neo.ListensForMouseOrTouchAt(_newMouseState.Position))
				{
					_neo.Click(_newMouseState.Position);
					return;
				}
			}
			else if (_oldMouseState.LeftButton == ButtonState.Released && _newMouseState.LeftButton == ButtonState.Pressed)
			{
				_neo.MouseDown(_newMouseState.Position);
				return;
			}

			if (_oldMouseState.ScrollWheelValue > _newMouseState.ScrollWheelValue)
			{
				_neo.Scale -= 0.07f;
				_neo.ForceRefresh();
			}

			if (_oldMouseState.ScrollWheelValue < _newMouseState.ScrollWheelValue)
			{
				_neo.Scale += 0.07f;
				_neo.ForceRefresh();
			}
		}

		private void CheckTouch()
		{
			TouchCollection tc = TouchPanel.GetState();
			foreach (TouchLocation tl in tc)
			{
				if (tl.State == TouchLocationState.Pressed)
				{
					Point pos = tl.Position.ToPoint();

					//Handle click
					if (_neo.ListensForMouseOrTouchAt(pos))
					{
						_neo.Click(pos);
						return;
					}
				}
			}
		}
	}
}