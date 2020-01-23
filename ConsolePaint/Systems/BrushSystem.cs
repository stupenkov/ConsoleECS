using System;
using ECS;
using ECS.Input;
using ConsolePaint.Components;
using ECS.ConsoleUI;

namespace ConsolePaint.Systems
{
	public class BrushSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach(
				(Entity entity, NavigateComponent navigate, SpriteComponent sprite, ColorPanelComponent colorPanel) =>
				{
					if (Input.Key == ConsoleKey.Spacebar)
					{
						if (navigate.Navigate.ContainsKey(ConsoleKey.Spacebar))
						{
							var brush = navigate.Navigate[ConsoleKey.Spacebar].GetComponent<BrushComponent>();

							switch (colorPanel.PanelType)
							{
								case ColorPanelType.Background:
									brush.Pixel.BackgroundColor = sprite.Bitmap.GetPixel(Cursor.LocalPosition).BackgroundColor;
									break;
								case ColorPanelType.Foreground:
									brush.Pixel.Color = sprite.Bitmap.GetPixel(Cursor.LocalPosition).BackgroundColor;
									break;
							}
						}
					}
				});

			Entities.Has(typeof(ActiveComponent), typeof(CanvasComponent)).Foreach((Entity entity, BrushComponent brush) =>
			{
				if (Input.KeyAlt == ConsoleKey.B)
				{
					brush.IsBackground = !brush.IsBackground;
				}
				else if (Input.KeyAlt == ConsoleKey.C)
				{
					brush.IsSymbol = !brush.IsSymbol;
				}

				if (Input.KeyCtrl != 0)
				{
					brush.Pixel.Symbol = (char)Input.KeyCtrl;
				}
			});
		}
	}
}
