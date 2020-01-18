using System;
using System.Collections.Generic;
using System.Text;
using ECS.BasicElemets;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.UI
{
	public class UICreator
	{
		private World _world;

		public UICreator(World world)
		{
			_world = world;
		}

		public Entity CreateTextEdit(Vector2 position, int lenght)
		{
			Entity entity = _world.CreateEntity($"TextEdti");
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

			Entity entity = _world.CreateEntity($"ModalDialog");
			entity.AddComponents(
				new TransformComponent { Position = new Vector3(position.X, position.Y, int.MaxValue) },
				new SpriteComponent());

			return entity;
		}
	}
}
