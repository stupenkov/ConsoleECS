using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(RenderingSystemGroup))]
	[UpdateBefore(typeof(RenderingSystem))]
	public class HiddenSystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, InnerComponent inner) =>
			{
				if (inner.Parent.ContainsComponent<HiddenComponent>())
				{
					entity.AddComponent<HiddenComponent>();
				}
				else
				{
					entity.RemoveComponent<HiddenComponent>();
				}
			});
		}
	}
}
