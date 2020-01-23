using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS;
using ECS.ConsoleUI;
using ECS.Numerics;

namespace ConsolePaint
{
	public class LayoutUI
	{
		private List<Entity> entities = new List<Entity>();

		public Vector2 GetSize()
		{
			if (entities.Count < 1)
			{
				throw new Exception($"Нет ни одного элемента для расчета размеров {this}");
			}

			int x = entities.Max(x => x.GetComponent<TransformComponent>().Position.X + x.GetComponent<TransformComponent>().Size.X);
			int y = entities.Max(x => x.GetComponent<TransformComponent>().Position.Y + x.GetComponent<TransformComponent>().Size.Y);
			return new Vector2(x, y + 1);
		}

		public void Add(Entity entity)
		{
			var transform = GetNormalizeTransform(entity);
			transform.Position.X = 0;
			transform.Position.Y = 0;
			entities.Add(entity);
		}

		public void AddRight(Entity entity, int shift = 0)
		{
			if (entities.Count > 0)
			{
				var lastTransform = entities[entities.Count - 1].GetComponent<TransformComponent>();
				var transform = GetNormalizeTransform(entity);
				transform.Position.X = lastTransform.Position.X + lastTransform.Size.X + 1 + shift;
				transform.Position.Y = lastTransform.Position.Y;
				entities.Add(entity);
			}
		}

		public void AddBottom(Entity entity, int shift = 0)
		{
			if (entities.Count > 0)
			{
				var entityMaxY = entities.OrderBy(x => x.GetComponent<TransformComponent>().Position.Y).LastOrDefault();
				var modifyTransform = entityMaxY.GetComponent<TransformComponent>();
				var transform = GetNormalizeTransform(entity);

				transform.Position.X = 0;
				transform.Position.Y = modifyTransform.Position.Y + modifyTransform.Size.Y + shift;
				entities.Add(entity);
			}
		}

		private TransformComponent GetNormalizeTransform(Entity entity)
		{
			if (!entity.ContainsComponent<TransformComponent>())
			{
				throw new Exception($"Сущность не имеет компонента {nameof(TransformComponent)}");
			}

			var transform = entity.GetComponent<TransformComponent>();
			if (transform.Size == Vector2.Zero)
			{
				if (entity.ContainsComponent<SpriteComponent>())
				{
					transform.Size = entity.GetComponent<SpriteComponent>().Bitmap.Size;
				}
			}

			return transform;
		}
	}
}
