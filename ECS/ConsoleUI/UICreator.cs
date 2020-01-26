using System;
using System.Collections.Generic;
using System.Linq;
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

		public Entity CreateButton(Vector3 position, string caption, string command = null, Entity entity = null)
		{
			Entity ent = _entityManager.CreateEntity("Button");

			ent.AddComponents(
				new TransformComponent { Position = position },
				new ButtonComponent { Caption = caption },
				new PropertiesUIComponent
				{
					Padding = new Indent(1, 2),
					ActiveColors = new ColorMask { Background = ConsoleColor.Green, ColorText = ConsoleColor.DarkBlue }
				},
				new CommandComponent { Command = command, Entity = entity }
				);

			return ent;
		}

		public Entity CreateButton(string caption, string command = null, Entity entity = null) =>
			CreateButton(new Vector3(), caption, command, entity);

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

		public Entity CreateTextEdit(Vector3 position, int lenght, string command = null, Entity entity = null)
		{
			Entity ent = _entityManager.CreateEntity($"TextEdit");
			ent.AddComponents(
				new TransformComponent
				{
					Position = position,
					Size = new Vector2(lenght, 1)
				},
				new SpriteComponent(),
				new TextEditComponent(),
				new CommandComponent
				{
					Command = command,
					Entity = entity,
				}

				);

			return ent;
		}

		public Entity CreateListItems(
			string name,
			Vector3 position,
			List<Entity> entities)
		{
			Entity entity = _entityManager.CreateEntity(name);
			entity.AddComponents(
				new TransformComponent { Position = position },
				new ListItemsComponent { Items = entities },
				new PropertiesUIComponent());

			return entity;
		}

		//public Entity CreateModalDialog(Vector2 position)
		//{
		//	ModalDialogComponent modalDialogComponent = new ModalDialogComponent();
		//	PropertiesUIComponent prop = new PropertiesUIComponent();
		//	Bitmap bitmap = new Bitmap(prop.Size);
		//	bitmap.FillColor(prop.BackgroundColor);
		//	Pixel pixel = prop.Border;
		//	for (int i = 0; i < bitmap.Width; i++)
		//	{
		//		bitmap.SetPixel(i, 0, pixel);
		//	}

		//	for (int i = 0; i < bitmap.Width; i++)
		//	{
		//		bitmap.SetPixel(i, bitmap.Height - 1, pixel);
		//	}

		//	for (int i = 0; i < bitmap.Height; i++)
		//	{
		//		bitmap.SetPixel(0, i, pixel);
		//	}

		//	for (int i = 0; i < bitmap.Height; i++)
		//	{
		//		bitmap.SetPixel(bitmap.Width - 1, i, pixel);
		//	}

		//	Entity entity = _entityManager.CreateEntity($"ModalDialog");
		//	entity.AddComponents(
		//		new TransformComponent { Position = new Vector3(position.X, position.Y, -1) },
		//		new SpriteComponent { Bitmap = bitmap },
		//		modalDialogComponent);

		//	return entity;
		//}

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
