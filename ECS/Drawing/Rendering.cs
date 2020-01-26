using System;
using ECS.Input;
using ECS.Numerics;

namespace ECS.Drawing
{
	public class Rendering : IRendering
	{
		private Bitmap firstBuffer;
		private Bitmap secondBuffer;

		public Rendering(int width, int height)
		{
			InitializeView(width, height);
		}

		public int WidthBuffer { get; private set; }

		public int HeightBuffer { get; private set; }

		public void AddBitmap(int x, int y, Bitmap bitmap)
		{
			secondBuffer.AddBitmap(x, y, bitmap);
		}

		public void AddBitmap(Vector2 position, Bitmap bitmap)
		{
			AddBitmap(position.X, position.Y, bitmap);
		}

		public void Render()
		{
			Console.CursorVisible = false;
			Vector2 curpos = new Vector2(Console.CursorLeft, Console.CursorTop);
			firstBuffer.Foreach((int x, int y, ref Pixel firstPixel) =>
			{
				Pixel secondPixel = secondBuffer.GetPixel(x, y);
				if (!Equals(firstPixel, secondPixel))
				{
					Console.SetCursorPosition(x, y);
					Console.ForegroundColor = secondPixel.Color;
					Console.BackgroundColor = secondPixel.BackgroundColor;
					Console.Write(secondPixel.Symbol);
					firstPixel = secondPixel;
					secondPixel.Clear();
				}
			});
			Console.SetCursorPosition(curpos.X, curpos.Y);
			if (Cursor.Enable)
			{
				Console.CursorVisible = true;
			}
		}

		public void SetView(int width, int height)
		{
			InitializeView(width, height);
		}

		public void InitializeView(int width, int height)
		{
			WidthBuffer = width;
			HeightBuffer = height;
			firstBuffer = new Bitmap(WidthBuffer, HeightBuffer);
			secondBuffer = new Bitmap(WidthBuffer, HeightBuffer);

			Console.SetWindowSize(1, 1);
			Console.SetBufferSize(WidthBuffer, HeightBuffer);
			Console.SetWindowSize(WidthBuffer, HeightBuffer);
			Console.ResetColor();
			Console.Clear();
		}

		public void InitializeView(Vector2 size)
		{
			InitializeView(size.X, size.Y);
		}
	}
}
