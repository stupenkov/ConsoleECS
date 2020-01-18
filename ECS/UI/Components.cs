using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.UI
{
	public class DecorationUIComponent : IComponent
	{
		public Vector2 Size = new Vector2(3, 3);
		public Pixel Border = new Pixel { BackgroundColor = ConsoleColor.White, Symbol = '+', Color = ConsoleColor.Black };
		public ConsoleColor BackgroundColor = ConsoleColor.DarkBlue;
	}

	public class ModalDialogComponent : IComponent { public List<Entity> InnerEntities = new List<Entity>(); }

	public class TextEditComponent : IComponent { public string Text; }

	public class CursorZone : IComponent { public Rectangle Zone; }
}
