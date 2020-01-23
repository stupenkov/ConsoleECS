using ECS.Input;

namespace ECS.ConsoleUI
{
	public class SetCursorSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity, TransformComponent transform) =>
			{
				Cursor.Begin = transform.Position.ToVector2();
				Cursor.End = transform.Position.ToVector2() + transform.Size - 1;
				if (entity.ContainsComponent<CursorLastPositionComponent>())
				{
					Cursor.SetPosition(entity.GetComponent<CursorLastPositionComponent>().Position);
					entity.RemoveComponent<CursorLastPositionComponent>();
				}
			});
		}
	}
}
