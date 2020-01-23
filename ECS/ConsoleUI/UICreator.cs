using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	public class UICreator
	{
		private World _world;
		private EntityManager _entityManager;

		public UICreator(World world)
		{
			_world = world;
			_entityManager = world.EntityManager;
		}

		public Entity CreateColorBar(Vector3 position)
		{
			Entity entity = _entityManager.CreateEntity($"ColorBar");
			entity.AddComponents(
				new TransformComponent { Position = position },
				new SpriteComponent { Bitmap = CreateBitmapColorPanel() });

			return entity;
		}

		public Entity CreateLabel(string text, Vector2 position, ColorMask mask)
		{
			Entity entity = _entityManager.CreateEntity($"Lable");
			entity.AddComponents(
				new TransformComponent { Position = new Vector3(position.X, position.Y, 0) },
				new LableComponent { Text = text, Mask = mask });

			return entity;
		}

		public Entity CreateLabel(string text, Vector2 position)
		{
			return CreateLabel(text, position, ColorMask.Default);
		}

		public Entity CreateLabel(string text)
		{
			return CreateLabel(text, Vector2.Zero);
		}

		public Entity CreateTextEdit(Vector2 position, int lenght)
		{
			Entity entity = _entityManager.CreateEntity($"TextEdit");
			entity.AddComponents(
				new TransformComponent
				{
					Position = new Vector3(position.X, position.Y, 0),
					Size = new Vector2(lenght, 1)
				},
				new SpriteComponent(),
				new DecorationUIComponent
				{
					Border = new Pixel
					{
						BackgroundColor = ConsoleColor.Red,
						Symbol = '*'
					}
				},
				new TextEditComponent());

			return entity;
		}

		public Entity CreateModalDialog(Vector2 position)
		{
			ModalDialogComponent modalDialogComponent = new ModalDialogComponent();
			DecorationUIComponent decoration = new DecorationUIComponent();
			Bitmap bitmap = new Bitmap(decoration.Size);
			bitmap.FillColor(decoration.BackgroundColor);
			Pixel pixel = decoration.Border;
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

			Entity entity = _entityManager.CreateEntity($"ModalDialog");
			entity.AddComponents(
				new TransformComponent { Position = new Vector3(position.X, position.Y, -1) },
				new SpriteComponent { Bitmap = bitmap},
				modalDialogComponent);

			return entity;
		}

		private Bitmap CreateBitmapColorPanel()
		{
			Bitmap colorPanelBitmap = new Bitmap(16, 1);
			Pixel p = colorPanelBitmap.GetPixel(0, 0);
			p.Color = ConsoleColor.White;
			p.Symbol = 'B';
			colorPanelBitmap.SetPixel(0, 0, p);
			colorPanelBitmap.Foreach((int x, int y, ref Pixel p) =>
			{
				p.BackgroundColor = (ConsoleColor)x;
			});
			return colorPanelBitmap;
		}
	}
}
