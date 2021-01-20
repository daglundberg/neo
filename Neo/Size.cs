using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Neo
{
	/// <summary>
	/// Describes a 2D-size.
	/// </summary>
	[DataContract]
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	public struct Size : IEquatable<Size>
	{
		private static readonly Size zeroPoint = new Size();

		/// <summary>
		/// The width of this <see cref="Size"/>.
		/// </summary>
		[DataMember]
		public int Width;

		/// <summary>
		/// The height of this <see cref="Size"/>.
		/// </summary>
		[DataMember]
		public int Height;

		/// <summary>
		/// Returns a <see cref="Size"/> with coordinates 0, 0.
		/// </summary>
		public static Size Zero
		{
			get { return zeroPoint; }
		}

		/// <summary>
		/// Constructs a point with Width and Height from two values.
		/// </summary>
		/// <param name="width">The width in 2d-space.</param>
		/// <param name="height">The height in 2d-space.</param>
		public Size(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		/// <summary>
		/// Constructs a point with Width and Height set to the same value.
		/// </summary>
		/// <param name="value">The width and height in 2d-space.</param>
		public Size(int value)
		{
			this.Width = value;
			this.Height = value;
		}

		/// <summary>
		/// Adds two points.
		/// </summary>
		/// <param name="value1">Source <see cref="Size"/> on the left of the add sign.</param>
		/// <param name="value2">Source <see cref="Size"/> on the right of the add sign.</param>
		/// <returns>Sum of the points.</returns>
		public static Size operator +(Size value1, Size value2)
		{
			return new Size(value1.Width + value2.Width, value1.Height + value2.Height);
		}

		/// <summary>
		/// Subtracts a <see cref="Size"/> from a <see cref="Size"/>.
		/// </summary>
		/// <param name="value1">Source <see cref="Size"/> on the left of the sub sign.</param>
		/// <param name="value2">Source <see cref="Size"/> on the right of the sub sign.</param>
		/// <returns>Result of the subtraction.</returns>
		public static Size operator -(Size value1, Size value2)
		{
			return new Size(value1.Width - value2.Width, value1.Height - value2.Height);
		}

		/// <summary>
		/// Multiplies the components of two points by each other.
		/// </summary>
		/// <param name="value1">Source <see cref="Size"/> on the left of the mul sign.</param>
		/// <param name="value2">Source <see cref="Size"/> on the right of the mul sign.</param>
		/// <returns>Result of the multiplication.</returns>
		public static Size operator *(Size value1, Size value2)
		{
			return new Size(value1.Width * value2.Width, value1.Height * value2.Height);
		}

		/// <summary>
		/// Divides the components of a <see cref="Size"/> by the components of another <see cref="Size"/>.
		/// </summary>
		/// <param name="source">Source <see cref="Size"/> on the left of the div sign.</param>
		/// <param name="divisor">Divisor <see cref="Size"/> on the right of the div sign.</param>
		/// <returns>The result of dividing the points.</returns>
		public static Size operator /(Size source, Size divisor)
		{
			return new Size(source.Width / divisor.Width, source.Height / divisor.Height);
		}

		/// <summary>
		/// Compares whether two <see cref="Size"/> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="Size"/> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="Size"/> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(Size a, Size b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Compares whether two <see cref="Size"/> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="Size"/> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="Size"/> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
		public static bool operator !=(Size a, Size b)
		{
			return !a.Equals(b);
		}

		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="Object"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj)
		{
			return (obj is Size) && Equals((Size)obj);
		}

		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="Size"/>.
		/// </summary>
		/// <param name="other">The <see cref="Size"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(Size other)
		{
			return ((Width == other.Width) && (Height == other.Height));
		}

		/// <summary>
		/// Gets the hash code of this <see cref="Size"/>.
		/// </summary>
		/// <returns>Hash code of this <see cref="Size"/>.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + Width.GetHashCode();
				hash = hash * 23 + Height.GetHashCode();
				return hash;
			}

		}

		/// <summary>
		/// Returns a <see cref="String"/> representation of this <see cref="Size"/> in the format:
		/// {Width:[<see cref="Width"/>] Height:[<see cref="Height"/>]}
		/// </summary>
		/// <returns><see cref="String"/> representation of this <see cref="Size"/>.</returns>
		public override string ToString()
		{
			return "{Width:" + Width + " Height:" + Height + "}";
		}

		/// <summary>
		/// Gets a <see cref="Vector2"/> representation for this object.
		/// </summary>
		/// <returns>A <see cref="Vector2"/> representation for this object.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 ToVector2()
		{
			return new Vector2(Width, Height);
		}

		/// <summary>
		/// Gets a <see cref="Point"/> representation for this object.
		/// </summary>
		/// <returns>A <see cref="Point"/> representation for this object.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Point ToPoint()
		{
			return new Point(Width, Height);
		}

		/// <summary>
		/// Deconstruction method for <see cref="Size"/>.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Deconstruct(out int width, out int height)
		{
			width = Width;
			height = Height;
		}
	}
}

namespace Neo.Extensions
{ 
	public static class Point
	{
		/// <summary>
		/// Gets a <see cref="Size"/> representation for this object.
		/// </summary>
		/// <returns>A <see cref="Size"/> representation for this object.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size ToSize(this Microsoft.Xna.Framework.Point point)
		{
			return new Size(point.X, point.Y);
		}
	}

	public static class Vector2
	{
		/// <summary>
		/// Gets a <see cref="Size"/> representation for this object.
		/// </summary>
		/// <returns>A <see cref="Size"/> representation for this object.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Size ToSize(this Microsoft.Xna.Framework.Vector2 vector2)
		{
			return new Size((int)vector2.X, (int)vector2.Y);
		}
	}
}

