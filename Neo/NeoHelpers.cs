using Microsoft.Xna.Framework;
using System;

namespace Neo
{
	public partial class Neo
	{
        /// <summary>
        /// Takes two sizes and returns the coordinate for centering the second size in the first one.
        /// <param name="a">Size of container.</param>
        /// <param name="b">Size of component to be centered in the container.</param>
        /// </summary>
        public static Size Center(Size a, Size b)
        {
            return new Size(
                a.Width / 2 - b.Width / 2,
                a.Height / 2 - b.Height / 2);
        }

        /// <summary>
        /// Takes two sizes and an offset. Returns the coordinate for centering the second size in the first one.
        /// <param name="a">Size of container.</param>
        /// <param name="b">Size of component to be centered in the container.</param>
        /// </summary>
        public static Size Center(Size a, Size b, int offsetHeight)
        {
            return new Size(
                a.Width / 2 - b.Width / 2,
                (a.Height / 2 - b.Height / 2) + offsetHeight);
        }

        /// <summary>
        /// Takes two sizes and an offset. Returns the coordinate for centering the second size in the first one.
        /// <param name="a">Size of container.</param>
        /// <param name="b">Size of component to be centered in the container.</param>
        /// </summary>
        public static Vector2 Center(Size a, Size b, Size offset)
        {
            return offset.ToVector2() + new Size(
                a.Width / 2 - b.Width / 2,
                a.Height / 2 - b.Height / 2).ToVector2();
        }

        public static Vector2 FromBottomRight(Vector2 screenSize, int x, int y)
        {
            return new Vector2(screenSize.X - x, screenSize.Y - y);
        }
    }

    [Flags]
    public enum Anchors
    {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,
    }

    public struct Margins
    {
        public Margins(int margins)
        {
            Left = margins;
            Top = margins;
            Right = margins;
            Bottom = margins;
        }

        public Margins(int left, int top, int right, int bottom)
		{
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
		}

        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }

/*    public struct Block
    {
        public Vector2 Position;
        public Vector4 Color;
        public Vector2 Size;
        public float Radius;
    }*/
}
