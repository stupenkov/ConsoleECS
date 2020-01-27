using System;
using System.Drawing;
using ECS.Numerics;

namespace ECS.Input
{
	public static class Cursor
	{
		private static Vector2 cursorPos;
		private static Vector2 begin;
		private static Vector2 end;
		private static bool _enable = true;

		static Cursor()
		{
			TranslateToOrigin();
			ResetRectangel();
		}

		public static bool Enable
		{
			get => _enable;
			set
			{
				_enable = value;
				Console.CursorVisible = value;
			}
		}

		public static Vector2 GlobalPosition { get => cursorPos; }

		public static Vector2 LocalPosition { get => cursorPos - begin; }

		public static Vector2 Begin
		{
			get => begin;
			set
			{
				if (begin != value)
				{
					begin = value;
					TranslateToOrigin();
				}
			}
		}

		public static Vector2 End
		{
			get => end;
			set
			{
				if (end != value)
				{
					end = value;
					TranslateToOrigin();
				}
			}
		}

		public static Rectangle Rectangle
		{
			get => new Rectangle(Begin.X, Begin.Y, End.X - Begin.X, End.Y - Begin.Y);
			set
			{
				Begin = new Vector2(value.Left, value.Top);
				End = new Vector2(value.Right, value.Bottom);
			}
		}

		public static void ResetRectangel()
		{
			begin = Vector2.Zero;
			end = new Vector2(Console.BufferWidth - 1, Console.BufferHeight - 1);
		}

		public static void ResetPosition()
		{			
			cursorPos = Vector2.Zero;
		}

		public static void MoveLeft()
		{
			if (!Enable)
			{
				return;
			}

			if (cursorPos.X > Begin.X)
			{
				cursorPos.X--;
			}

			SetCursorPosition();
		}

		public static void MoveRight()
		{
			if (!Enable)
			{
				return;
			}

			if (cursorPos.X < End.X)
			{
				cursorPos.X++;
			}

			SetCursorPosition();
		}

		public static void MoveUp()
		{
			if (!Enable)
			{
				return;
			}

			if (cursorPos.Y > Begin.Y)
			{
				cursorPos.Y--;
			}

			SetCursorPosition();
		}

		public static void MoveDown()
		{
			if (!Enable)
			{
				return;
			}

			if (cursorPos.Y < End.Y)
			{
				cursorPos.Y++;
			}

			SetCursorPosition();
		}

		public static void SetPosition(Vector2 position)
		{
			cursorPos = position;
			SetCursorPosition();
		}

		private static void TranslateToOrigin()
		{
			if (cursorPos.X < begin.X || cursorPos.X > end.X || cursorPos.Y < begin.Y || cursorPos.Y > end.Y)
			{
				cursorPos = Begin;
				SetCursorPosition();
			}
		}

		private static void SetCursorPosition()
		{
			Console.SetCursorPosition(cursorPos.X, cursorPos.Y);
		}
	}
}
