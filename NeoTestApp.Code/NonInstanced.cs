using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoTestApp.Code
{
	class NonInstanced
	{
		Rectangle[] rects;
		int count = 100;
		public NonInstanced()
		{

			rects = new Rectangle[count];

			int id = 0;
			for (int x = 0; x < 10; x++)
			{
				for (int y = 0; y < 10; y++)
				{
					rects[id] = new Rectangle(new Vector2(x*20f, y*20), Color.AliceBlue, new Vector2(19,19));
					id++;
				}
			}
		}

		public void Draw(GameTime gameTime)
		{
			for (int i = 0; i < count; i++)
			{
				rects[i].Draw(gameTime);
			}
		}
	}
}
