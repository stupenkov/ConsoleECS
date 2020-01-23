using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ECS;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	public class DecorationUIComponent : IComponentData
	{
		public Vector2 Size = new Vector2(3, 3);
		public Pixel Border = new Pixel { BackgroundColor = ConsoleColor.White, Symbol = '+', Color = ConsoleColor.Black };
		public ConsoleColor BackgroundColor = ConsoleColor.DarkBlue;
	}

	public class ModalDialogComponent : IComponentData { public List<Entity> InnerEntities = new List<Entity>(); }

	public class TextEditComponent : IComponentData { public string Text; }

	public class CursorZone : IComponentData { public Rectangle Zone; }

	public class StepperComponent : IComponentData { public Pixel Mask; }

	public class LableComponent : IComponentData { public string Text; public ColorMask Mask; }

	public class MenuListComponent : IComponentData
	{
		public List<string> Items = new List<string>();
		public ColorMask ColorElement;
		public ColorMask ColorSelect;
		public int SelectedIndex;
	}

	public class TransformComponent : IComponentData { public Vector3 Position; public Vector2 Size; public bool Autosize = true; }
	public class SpriteComponent : IComponentData { public Bitmap Bitmap; }
	public class NavigateComponent : IComponentData { public Dictionary<ConsoleKey, Entity> Navigate = new Dictionary<ConsoleKey, Entity>(); }
	public class ActiveComponent : IComponentData { public Entity PreviousActive; }
	public class CursorLastPositionComponent : IComponentData { public Vector2 Position; public ConsoleKey PressKey; }
}
