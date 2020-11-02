using MonoGame.Framework.Utilities;
using System;

namespace NeoTestApp.Desktop
{
	public static class Program
	{
			[STAThread]
			static void Main()
			{
				using (var game = new NeoTestApp.Code.Game1(MonoGamePlatform.DesktopGL))
					game.Run();
			}
	}
}
