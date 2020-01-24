using System.Drawing;
using ECS.Input;

namespace ECS.ConsoleUI
{
	[GroupInputSystems]
	[UpdateAfter(typeof(InputSystem))]
	public class CursorMoveSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities
				.Exclude(typeof(HiddenComponent))
				.Foreach((Entity entity, TransformComponent transform) =>
				{
					Rectangle rect = transform.GetRectangle();
					if (Cursor.GlobalPosition.IsInRectangle(rect))
					{
						entity.AddComponent(new CursorHoverComponent { Position = Cursor.GlobalPosition });
					}
					else if (entity.ContainsComponent<CursorHoverComponent>())
					{
						entity.RemoveComponent<CursorHoverComponent>();
					}
				});
		}
	}
}
