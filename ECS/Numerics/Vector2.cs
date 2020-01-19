using System;

namespace ECS.Numerics
{
	[Serializable]
	public struct Vector2
	{
		public int X;
		public int Y;

		public Vector2(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Vector2 One => new Vector2 { X = 1, Y = 1 };

		public static Vector2 Zero => new Vector2 { X = 0, Y = 0 };

		public static Vector2 operator +(Vector2 value1, Vector2 value2)
		{
			return new Vector2(value1.X + value2.X, value1.Y + value2.Y);
		}

		public static Vector2 operator -(Vector2 value1, Vector2 value2)
		{
			return new Vector2(value1.X - value2.X, value1.Y - value2.Y);
		}

		public static Vector2 operator -(Vector2 vector, int value)
		{
			return new Vector2(vector.X - value, vector.Y - value);
		}

		public static Vector2 operator +(Vector2 vector, int value)
		{
			return new Vector2(vector.X + value, vector.Y + value);
		}

		public static bool operator ==(Vector2 value1, Vector2 value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Vector2 value1, Vector2 value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return $"X: {X}, Y: {Y}";
		}
	}
}
