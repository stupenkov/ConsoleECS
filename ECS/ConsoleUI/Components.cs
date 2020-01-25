using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ECS;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	// Cursor
	public class CursorHoverComponent : IComponentData { public Vector2 Position; }
	public class CursorZoneComponent : IComponentData { public Rectangle Zone; }
	public class CursorLastPositionComponent : IComponentData { public Vector2 Position; public ConsoleKey PressKey; }

	// UI components
	public class ModalDialogComponent : IComponentData { public List<Entity> InnerEntities = new List<Entity>(); }

	public class TextEditComponent : IComponentData { public string Text = string.Empty; public int Length = 5; public ColorMask Mask = ColorMask.Default; }

	public class StepperComponent : IComponentData { public Pixel Mask; }

	public class LableComponent : IComponentData { public string Text; public ColorMask Mask; }

	public class ButtonComponent : IComponentData { public string Caption; }

	public class DecorationUIComponent : IComponentData
	{
		public Pixel Border= new Pixel { BackgroundColor = ConsoleColor.White, Symbol = '+', Color = ConsoleColor.Black };
	}

	public class PropertiesUIComponent : IComponentData
	{
		public ColorMask Colors = new ColorMask { Background = ConsoleColor.DarkGreen, ColorText = ConsoleColor.White };
		public ColorMask ActiveColors = new ColorMask { Background = ConsoleColor.Green, ColorText = ConsoleColor.Black };
		public Indent Padding;
	}

	public class ListItemsComponent : IComponentData
	{
		public List<Entity> Items = new List<Entity>();
		public int SelectedIndex;
	}

	public class CommandComponent : IComponentData { public string CommandName; }

	public class InnerComponent: IComponentData { public Entity Parent; }

	// Base components.
	public class TransformComponent : IComponentData { public Vector3 Position; public Vector2 Size; public bool Autosize = true; public bool Center; }
	public class SpriteComponent : IComponentData { public Bitmap Bitmap; }
	public class NavigateComponent : IComponentData { public Dictionary<ConsoleKey, Entity> Navigate = new Dictionary<ConsoleKey, Entity>(); }
	public class ActiveComponent : IComponentData { public Entity PreviousActive; }
	public class HiddenComponent : IComponentData { public bool Hidden; }
	public class WindowComponent : IComponentData { public Vector2 Size; }
}
