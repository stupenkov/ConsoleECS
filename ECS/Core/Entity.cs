using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace ECS
{
	[Serializable]
	public class Entity
	{
		private static int IdCounter = 0;
		[NonSerialized]
		private EntityManager _entityManager;

		internal Entity(World world)
		{
			_entityManager = world.EntityManager;
			Id = IdCounter++;
			_entityManager.AddEntity(this);
		}

		public int Id { get; }

		public string Name { get; internal set; } = string.Empty;

		public Entity AddComponent(IComponentData component)
		{
			_entityManager.AddComponent(Id, component);
			return this;
		}

		public Entity AddComponents(params IComponentData[] components)
		{
			foreach (var component in components)
			{
				AddComponent(component);
			}

			return this;
		}

		public Entity AddComponent<T>() where T : IComponentData, new()
		{
			_entityManager.AddComponent(Id, new T());
			return this;
		}

		public void RemoveComponent<T>()
		{
			_entityManager.RemoveComponent(Id, typeof(T));
		}

		public T GetComponent<T>() where T : IComponentData
		{
			return _entityManager.GetComponent<T>(Id);
		}

		public IComponentData GetComponent(Type type)
		{
			return _entityManager.GetComponent(Id, type);
		}

		public bool ContainsComponent(Type type)
		{
			return _entityManager.ContainsComponent(Id, type);
		}

		public bool ContainsComponent<T>()
		{
			return _entityManager.ContainsComponent(Id, typeof(T));
		}

		public override string ToString()
		{
			return $"Name: {Name}, ID: {Id}";
		}
	}
}
