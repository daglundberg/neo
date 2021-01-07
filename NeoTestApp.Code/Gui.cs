using Neo;
using Neo.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace NeoTestApp.Code
{
	class Gui : DrawableGameComponent
	{
		Neo.Neo _neo;
		Row r;
		public Gui(Game game) : base(game)
		{
			Style style = new Style(Game.Content);
			_neo = new Neo.Neo(game, style, Game1.CurrentPlatform);

			r = new Row();

			r.Anchors = Anchors.Top | Anchors.Left | Anchors.Bottom;
			r.Size = new Size(80);
			r.Margins = new Margins(25, 50, 0, 100);
			r.LayoutRule = Row.LayoutRules.TopToBottom;
			r.AddChildren(
						new Control[]
						{
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4,16,4,4)
							},
							new Switch()
							{
								Size = new Size(60,30),
								Margins = new Margins(4),
								Checked = true
							},
							new Button()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},
							new Switch()
							{
								Size = new Size(60,30),
								Margins = new Margins(4),
								Checked = false
							},
							new Switch()
							{
								Size = new Size(60,30),
								Margins = new Margins(4),
								Checked = true
							},
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4,4,16,4)
							},
						});
			
			_neo.AddChildren(
				new Control[]
				{
					new Grid()
					{
						Anchors = Anchors.Top | Anchors.Bottom | Anchors.Right,
						Size = new Size(600, 400),
						Margins = new Margins(135, 50, 135, 135)
					},

					r,

					new Button()
					{
						Anchors = Anchors.Right | Anchors.Bottom,
						Size = new Size(110, 40),
						Margins = new Margins(30)
					},

					new Button()
					{
						Anchors = Anchors.Left | Anchors.Bottom,
						Size = new Size(110, 40),
						Margins = new Margins(30),
						Color = Color.Blue
					},
				});
		}

		public override void Draw(GameTime gameTime)
		{
			_neo.Draw(gameTime);
		}

		//float x = 0;
		public override void Update(GameTime gameTime)
		{
			CheckMouse();
			CheckTouch();
		//	r.Margins = new Margins((int)x);
		//	x += 0.2f;
			base.Update(gameTime);
		}

		public void Init()
		{
			_neo.Init();
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
