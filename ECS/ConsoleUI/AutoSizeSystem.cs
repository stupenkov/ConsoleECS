using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(TransformGroup))]
	//[UpdateBefore(typeof(RenderingSystem))]
	public class AutoSizeSystem : ComponentSystem
	{
		
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, TransformComponent transform, SpriteComponent sprite) =>
			{
				if (transform.Autosize && transform.Size == Vector2.Zero)
				{
					transform.Size = sprite.Bitmap.Size;
				}
			});
		}
	}
}
