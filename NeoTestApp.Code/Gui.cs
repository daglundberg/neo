using Neo;
using Neo.Controls;
using Microsoft.Xna.Framework;
using System;

namespace NeoTestApp.Code
{
	class Gui : DrawableGameComponent
	{

		Neo.Neo _neo;
		public Gui(Game game) : base(game)
		{
			Style style = new Style(Game.Content);
			_neo = new Neo.Neo(game, style, Game1.CurrentPlatform);

			_neo.AddChildren(
				new Control[]
				{
					new Grid()
					{
						Anchors = Anchors.Right | Anchors.Top | Anchors.Left | Anchors.Bottom,
						Size = new Size(0),
						Margins = new Margins(200,100,200,300)
					},

					new Grid()
					{
						Anchors = Anchors.Top | Anchors.Left,
						Size = new Size(100),
						Margins = new Margins(20)
					},

					new Row()
					{
						Anchors = Anchors.Right | Anchors.Left | Anchors.Bottom,
						Size = new Size(500),
						Margins = new Margins(20),
						LayoutRule = Row.LayoutRules.TopToBottom
					}.AddChildren(
						new Control[]
						{
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(10),
								Margins = new Margins(2)

							},
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(100),
								Margins = new Margins(2)
							},
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(100),
								Margins = new Margins(2)
							},
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(100),
								Margins = new Margins(2)
							},
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(100),
								Margins = new Margins(2)
							},
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(100),
								Margins = new Margins(2)
							},
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(100),
								Margins = new Margins(2)
							},
							new Grid()
							{
								Anchors = Anchors.Left | Anchors.Right,
								Size = new Size(100),
								Margins = new Margins(2)
							},
							new Grid()
							{
								Anchors = Anchors.Top | Anchors.Bottom,
								Size = new Size(100),
								Margins = new Margins(2)
							},
						}),
				}); ;
		}

		public override void Draw(GameTime gameTime)
		{
			_neo.Draw(gameTime);
		}

		public void Init()
		{
			_neo.Init();
		}
	}
}
