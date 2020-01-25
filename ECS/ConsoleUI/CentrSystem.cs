using System;
using System.Collections.Generic;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(TransformGroup))]
	[UpdateAfter(typeof(AutoSizeSystem))]
	public class CentrSystem : ComponentSystem
	{
		private Rendering _rendering;

		public CentrSystem(Rendering rendering)
		{
			_rendering = rendering;
		}

		public override void OnUpdate()
		{
			Entities.Foreach((Entity etity, TransformComponent transform) =>
			{
				Vector3 position = transform.Position;
				if (transform.Center)
				{
					position.X = _rendering.WidthBuffer / 2 - transform.Size.X / 2;
					position.Y = _rendering.HeightBuffer / 2 - transform.Size.Y / 2;
				}

				transform.Position = position;
			});
		}
	}
}
