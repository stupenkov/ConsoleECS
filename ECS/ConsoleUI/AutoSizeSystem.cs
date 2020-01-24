using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[GroupRenderingSystems]
	[UpdateBefore(typeof(RenderingSystem))]
	public class AutoSizeSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity etity, TransformComponent transform, SpriteComponent sprite) =>
				{
					if (transform.Autosize && transform.Size == Vector2.Zero)
					{
						transform.Size = sprite.Bitmap.Size;
					}
				});
		}
	}
}
