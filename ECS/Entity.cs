using System;

namespace ECS
{
	public class Entity
	{
		private static int IdCounter = 0;
		private World worldState;

		internal Entity(World worldState)
		{
			this.worldState = worldState;
			Id = IdCounter++;
			worldState.AddEntity(this);
		}

		public int Id { get; }

		public string Name { get; internal set; }

		public Entity AddComponent(IComponent component)
		{
			worldState.AddComponent(Id, component);
			return this;
		}

		public Entity AddComponents(params IComponent[] components)
		{
			foreach (var component in components)
			{
				AddComponent(component);
			}

			return this;
		}

		public Entity AddComponent<T>() where T : IComponent, new()
		{
			worldState.AddComponent(Id, new T());
			return this;
		}

		public void RemoveComponent<T>()
		{
			worldState.RemoveComponent(Id, typeof(T));
		}

		public T GetComponent<T>() where T : IComponent
		{
			return worldState.GetComponent<T>(Id);
		}

		public IComponent GetComponent(Type type)
		{
			return worldState.GetComponent(Id, type);
		}

		public bool ContainsComponent(Type type)
		{
			return worldState.ContainsComponent(Id, type);
		}

		public bool ContainsComponent<T>()
		{
			return worldState.ContainsComponent(Id, typeof(T));
		}

	}
}
