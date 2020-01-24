using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	public static class TransformExtansion
	{
		public static Rectangle GetRectangle(this TransformComponent transform)
		{
			return new Rectangle(transform.Position.X, transform.Position.Y, transform.Size.X, transform.Size.Y);
		}

		public static int GetBottom(this TransformComponent transform)
		{
			int value = transform.Position.X + transform.Size.X - 1;
			return value < 0 ? 0 : value;
		}

		public static int GetRight(this TransformComponent transform)
		{
			int value = transform.Position.Y + transform.Size.Y - 1;
			return value < 0 ? 0 : value;
		}
	}
}
