using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[GroupRenderingSystems]
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
				.Exclude(typeof(HiddenComponent))
				.OrderBy(x => x.GetComponent<TransformComponent>().Position.Z)
				.Foreach((Entity etity, TransformComponent transform, SpriteComponent sprite) =>
				{
					_rendering.AddBitmap(transform.Position.ToVector2(), sprite.Bitmap);
				});

			_rendering.Render();
		}
	}
}
