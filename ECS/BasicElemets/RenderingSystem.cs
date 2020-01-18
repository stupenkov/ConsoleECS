using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.BasicElemets
{
	internal class RenderingSystem : SystemBase
	{
		private IRendering _rendering;

		public RenderingSystem(IRendering rendering)
		{
			_rendering = rendering;
		}

		public override void OnUpdate()
		{
			Entities
				.Has(typeof(TransformComponent))
				.OrderByDescending(x => x.GetComponent<TransformComponent>().Position.Y)
				.Foreach((Entity etity, TransformComponent transform, SpriteComponent sprite) =>
				{
					if (transform.Position.Z < 0)
					{
						return;
					}

					if (transform.Size == Vector2.Zero)
					{
						transform.Size = sprite.Bitmap.Size;
					}

					_rendering.AddBitmap(transform.Position.ToVector2(), sprite.Bitmap);
				});

			_rendering.Render();
		}
	}
}
