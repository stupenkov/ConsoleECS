﻿using ECS;
using ECS.Input;

namespace ECS.ConsoleUI
{
	[DisableCreation]

	[UpdateAfter(typeof(InputSystem))]
	public class NavigateSystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach(
				(Entity entity, TransformComponent transform, NavigateComponent nextObj) =>
				{
					if (nextObj.Navigate.ContainsKey(Input.Key))
					{
						CursorLastPositionComponent cursorLastPos = new CursorLastPositionComponent
						{
							Position = Cursor.GlobalPosition,
						};

						entity.AddComponent(cursorLastPos);
						entity.RemoveComponent<ActiveComponent>();
						nextObj.Navigate[Input.Key].AddComponent<ActiveComponent>();
					}
				});
		}
	}
}
