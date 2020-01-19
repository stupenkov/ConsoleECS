using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
	public class EntityManager
	{
		private World _world;
		private List<Entity> _entities;
		private Dictionary<Type, Dictionary<int, IComponentData>> _components;

		public EntityManager(World world)
		{
			_world = world;
			_entities = world.Entities;
			_components = world.Components;
		}

		public Entity CreateEntity(string name = "")
		{
			Entity entity = new Entity(_world);
			if (!string.IsNullOrEmpty(name))
			{
				entity.Name = name;
			}

			return entity;
		}

		internal void AddEntity(Entity entity)
		{
			if (_entities.Contains(entity))
			{
				throw new Exception($"Мир уже содержит сущность с id {entity.Id}");
			}

			_entities.Add(entity);
		}

		public void AddComponent(int id, IComponentData component)
		{
			Type type = component.GetType();
			_components[type][id] = component;
		}

		public void RemoveComponent(int id, Type componentType)
		{
			_components[componentType].Remove(id);
		}

		public T GetComponent<T>(int id) where T : IComponentData
		{
			if (_components[typeof(T)].TryGetValue(id, out IComponentData component))
			{
				return (T)component;
			}

			return default;
		}

		public IComponentData GetComponent(int id, Type typeComponent)
		{
			if (_components[typeComponent].TryGetValue(id, out IComponentData component))
			{
				return component;
			}

			return default;
		}

		public bool ContainsComponent(int id, Type componentType)
		{
			return _components[componentType].ContainsKey(id);
		}

		public IDictionary<Type, IComponentData> GetComponents(int id)
		{
			return _components.Where(x => x.Value.ContainsKey(id))
				.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault().Value);
		}
	}
}
