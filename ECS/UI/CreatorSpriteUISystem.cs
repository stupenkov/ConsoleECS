using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using ECS.BasicElemets;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.UI
{
	public class CreatorSpriteUISystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Foreach(
				(Entity entity, SpriteComponent sprite, DecorationUIComponent decor, TransformComponent transform) =>
				{
					if (!decor.Border.IsDefault)
					{
						transform.Size += 2;
					}

					sprite.Bitmap = new Bitmap(transform.Size);
					PaintBitmap(sprite.Bitmap, decor);
					CursorZone zone = CreateCursorZone(transform, decor);
					entity.AddComponent(zone);
					entity.RemoveComponent<DecorationUIComponent>();
				});

			// Lable sprite ctreator
			Entities.Foreach((Entity entity, LableComponent lable) =>
			{
				Bitmap bitmap = Bitmap.CreateFromText(lable.Text, lable.Mask);
				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
			});

			// Menu
			Entities.Foreach((Entity entity, MenuListComponent menuList) =>
			{
				int widht = menuList.Items.Max(x => x.Length);
				int height = menuList.Items.Count;
				Bitmap bitmap = new Bitmap(widht, height);
				bitmap.FillColor(menuList.ColorElement.Background);
				for (int i = 0; i < menuList.Items.Count; i++)
				{
					ColorMask mask = menuList.SelectedIndex == i ? menuList.ColorSelect : menuList.ColorElement;
					Bitmap textBitmap = Bitmap.CreateFromText(menuList.Items[i], mask);
					bitmap.AddBitmap(0, i, textBitmap);
				}

				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
			});
		}

		private void PaintBitmap(Bitmap bitmap, DecorationUIComponent decoration)
		{
			bitmap.FillColor(decoration.BackgroundColor);
			Pixel pixel = decoration.Border;
			if (pixel.IsDefault)
			{
				return;
			}

			for (int i = 0; i < bitmap.Width; i++)
			{
				bitmap.SetPixel(i, 0, pixel);
			}

			for (int i = 0; i < bitmap.Width; i++)
			{
				bitmap.SetPixel(i, bitmap.Height - 1, pixel);
			}

			for (int i = 0; i < bitmap.Height; i++)
			{
				bitmap.SetPixel(0, i, pixel);
			}

			for (int i = 0; i < bitmap.Height; i++)
			{
				bitmap.SetPixel(bitmap.Width - 1, i, pixel);
			}
		}

		private CursorZone CreateCursorZone(TransformComponent transform, DecorationUIComponent decor)
		{
			int x = transform.Position.X;
			int y = transform.Position.Y;
			int w = transform.Size.X;
			int h = transform.Size.Y;

			if (!decor.Border.IsDefault)
			{
				x += 1;
				y += 1;
				w -= 1;
				h -= 1;
			}

			CursorZone zone = new CursorZone
			{
				Zone = new Rectangle(x, y, w, h)
			};

			return zone;
		}
	}
}

