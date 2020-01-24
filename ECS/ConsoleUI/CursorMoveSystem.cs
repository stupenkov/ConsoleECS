using System.Drawing;
using ECS.Input;
using System.Linq;

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

			// Удаляем hover с наложенных компонентов.
			Entities
				.Has(typeof(CursorHoverComponent), typeof(TransformComponent))
				.OrderByDescending(x=>x.GetComponent<TransformComponent>().Position.Z)
				.Skip(1)
				.Foreach((Entity entity) =>
				{
					entity.RemoveComponent<CursorHoverComponent>();
				});
		}
	}
}
