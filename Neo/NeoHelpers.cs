using Microsoft.Xna.Framework;

namespace Neo
{
	public partial class Neo
	{
        /// <summary>
        /// Takes two sizes and returns the coordinate for centering the second size in the first one.
        /// <param name="a">Size of container.</param>
        /// <param name="b">Size of component to be centered in the container.</param>
        /// </summary>
        public static Point Center(Point a, Point b)
        {
            return new Point(
                a.X / 2 - b.X / 2,
                a.Y / 2 - b.Y / 2);
        }

        /// <summary>
        /// Takes two sizes and an offset. Returns the coordinate for centering the second size in the first one.
        /// <param name="a">Size of container.</param>
        /// <param name="b">Size of component to be centered in the container.</param>
        /// </summary>
        public static Point Center(Point a, Point b, int offsetHeight)
        {
            return new Point(
                a.X / 2 - b.X / 2,
                (a.Y / 2 - b.Y / 2) + offsetHeight);
        }

        /// <summary>
        /// Takes two sizes and an offset. Returns the coordinate for centering the second size in the first one.
        /// <param name="a">Size of container.</param>
        /// <param name="b">Size of component to be centered in the container.</param>
        /// </summary>
        public static Vector2 Center(Point a, Point b, Point offset)
        {
            return offset.ToVector2() + new Point(
                a.X / 2 - b.X / 2,
                a.Y / 2 - b.Y / 2).ToVector2();
        }

        public static Vector2 FromBottomRight(Vector2 screenSize, int x, int y)
        {
            return new Vector2(screenSize.X - x, screenSize.Y - y);
        }
    }
}
