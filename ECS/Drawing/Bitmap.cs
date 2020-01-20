using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ECS.Numerics;

namespace ECS.Drawing
{
	public delegate void ReturnPositionAndPixels(int x, int y, ref Pixel pixel);

	public delegate void ReturnPixel(ref Pixel pixel);

	[Serializable]
	public class Bitmap
	{
		private Pixel[,] map;

		public Vector2 Size { get; }
		public int Width { get; set; }
		public int Height { get; set; }

		public Bitmap(int width, int height)
		{
			Width = width;
			Height = height;
			Size = new Vector2(Width, Height);
			map = new Pixel[Width, Height];
		}

		public Bitmap(Vector2 size) : this(size.X, size.Y)
		{
		}

		public static Bitmap CreateFromText(string text, ColorMask mask)
		{
			return CreateFromText(text, mask.ColorText, mask.Background);
		}

		public static Bitmap CreateFromText(string text, ConsoleColor color = ConsoleColor.Green, ConsoleColor background = ConsoleColor.Black)
		{
			Bitmap bitmap = new Bitmap(text.Length, 1);
			bitmap.Foreach((int x, int y, ref Pixel p) =>
			{
				p.BackgroundColor = background;
				p.Color = color;
				p.Symbol = text[x];
			});

			return bitmap;
		}

		public static Bitmap Load(string path)
		{
			using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryFormatter bf = new BinaryFormatter();
			return (Bitmap)bf.Deserialize(fs);
		}

		public Bitmap GetCopy()
		{
			return (Bitmap)MemberwiseClone();
		}

		public void Clear()
		{
			Foreach((ref Pixel p) =>
			{
				p = new Pixel { BackgroundColor = 0, Color = 0, Symbol = ' ' };
			});
		}

		public void FillColor(ConsoleColor color)
		{
			Foreach((ref Pixel p) =>
			{
				p.BackgroundColor = color;
			});
		}

		public void AddBitmap(int xPos, int yPos, Bitmap bitmap)
		{
			bitmap.Foreach((int x, int y, ref Pixel pixel) =>
			{
				SetPixel(x + xPos, y + yPos, pixel);
			});
		}

		public void SetPixel(int x, int y, ReturnPixel action)
		{
			action.Invoke(ref map[x, y]);
		}

		public void SetPixel(int x, int y, Pixel pixel)
		{
			map[x, y] = pixel;
		}

		public void SetPixel(Vector2 position, Pixel pixel)
		{
			SetPixel(position.X, position.Y, pixel);
		}

		public Pixel GetPixel(int x, int y)
		{
			return map[x, y];
		}

		public Pixel GetPixel(Vector2 position)
		{
			return GetPixel(position.X, position.Y);
		}

		public void Save(string path)
		{
			using FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, this);
		}

		public void Foreach(ReturnPositionAndPixels action)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					action.Invoke(x, y, ref map[x, y]);
				}
			}
		}

		public void Foreach(ReturnPixel action)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					action.Invoke(ref map[x, y]);
				}
			}
		}

		public override string ToString()
		{
			return $"Size(width: {Width}, height: {Height})";
		}
	}
}
