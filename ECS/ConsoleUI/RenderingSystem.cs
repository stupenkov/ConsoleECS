using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(RenderingSystemGroup))]
	internal class RenderingSystem : ComponentSystem
	{
		private Rendering _rendering;

		public RenderingSystem(Rendering rendering)
		{
			_rendering = rendering;
		}

		public override void OnStart()
		{
			Entity window = World.EntityManager.CreateEntity("Window");
			window.AddComponent(new WindowComponent { Size = new Vector2(_rendering.WidthBuffer, _rendering.HeightBuffer) });
		}

		public override void OnUpdate()
		{
			Bitmap background = new Bitmap(_rendering.WidthBuffer, _rendering.HeightBuffer);
			background.FillColor(ConsoleColor.White);

			Entities
				.Has(typeof(TransformComponent))
				.Exclude(typeof(HiddenComponent))
				.OrderBy(x => x.GetComponent<TransformComponent>().Position.Z)
				.Foreach((Entity entity, TransformComponent transform, SpriteComponent sprite) =>
				{
					background.AddBitmap(transform.Position.ToVector2(), sprite.Bitmap);
					_rendering.AddBitmap(0, 0, background);
				});

			_rendering.Render();
		}
	}
}
