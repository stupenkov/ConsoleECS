using System;
using ECS;
using ECS.Drawing;
using ECS.Input;
using ConsolePaint.Components;
using ECS.ConsoleUI;

namespace ConsolePaint.Systems
{
	public class PaintSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent), typeof(CanvasComponent))
				.Foreach((Entity entity, BrushComponent brush, SpriteComponent sprite) =>
				{
					if (Input.Key == ConsoleKey.Spacebar)
					{
						Pixel currentPixel = sprite.Bitmap.GetPixel(Cursor.LocalPosition);
						if (brush.IsBackground)
						{
							currentPixel.BackgroundColor = brush.Pixel.BackgroundColor;
						}

						if (brush.IsForeground)
						{
							currentPixel.Color = brush.Pixel.Color;
						}

						if (brush.IsSymbol)
						{
							currentPixel.Symbol = brush.Pixel.Symbol;
						}
						else
						{
							currentPixel.Symbol = ' ';
						}

						sprite.Bitmap.SetPixel(Cursor.LocalPosition, currentPixel);
					}
				});
		}
	}
}
