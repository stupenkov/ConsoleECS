using System;
using System.Collections.Generic;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.BasicElemets
{
	public class TransformComponent : IComponentData { public Vector3 Position; public Vector2 Size; public bool Autosize = true; }
	public class SpriteComponent : IComponentData { public Bitmap Bitmap; }
	public class NavigateComponent : IComponentData { public Dictionary<ConsoleKey, Entity> Navigate = new Dictionary<ConsoleKey, Entity>(); }
	public class ActiveComponent : IComponentData { public Entity PreviousActive; }
	public class CursorLastPositionComponent : IComponentData { public Vector2 Position; public ConsoleKey PressKey; }
}
