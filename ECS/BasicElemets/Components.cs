using System;
using System.Collections.Generic;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.BasicElemets
{
	public class TransformComponent : IComponent { public Vector3 Position; public Vector2 Size; public bool Autosize = true; }
	public class SpriteComponent : IComponent { public Bitmap Bitmap; }
	public class NavigateComponent : IComponent { public Dictionary<ConsoleKey, Entity> Navigate = new Dictionary<ConsoleKey, Entity>(); }
	public class ActiveComponent : IComponent { }
	public class CursorLastPositionComponent : IComponent { public Vector2 Position; public ConsoleKey PressKey; }
}
