using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ECS.Input;

namespace ECS
{
	public class World
	{
		private Dictionary<Type, Dictionary<int, IComponent>> components = new Dictionary<Type, Dictionary<int, IComponent>>();
		private List<ISystem> systems = new List<ISystem>();

		public DependencyInjection Injection { get; set; } = new DependencyInjection();

		public ReadOnlyCollection<ISystem> Systems => systems.AsReadOnly();

		public List<Entity> Entities { get; } = new List<Entity>();

		public KeyBoard Input { get; set; } = new KeyBoard();

		public Entity CreateEntity(string name = "")
		{
			Entity entity = new Entity(this);
			if (!string.IsNullOrEmpty(name))
			{
				entity.Name = name;
			}

			return entity;
		}

		public void AddEntity(Entity entity)
		{
			if (!Entities.Contains(entity))
			{
				Entities.Add(entity);
			}
		}

		public void AddComponent(int id, IComponent component)
		{
			Type type = component.GetType();
			if (!components.ContainsKey(type))
			{
				components.Add(type, new Dictionary<int, IComponent>());
			}

			components[type][id] = component;
		}

		public void RemoveComponent(int id, Type componentType)
		{
			components[componentType].Remove(id);
		}

		public T GetComponent<T>(int id) where T : IComponent
		{
			IComponent component;
			Type componentType = typeof(T);
			if (components.ContainsKey(componentType))
			{
				if (components[componentType].TryGetValue(id, out component))
				{
					return (T)component;
				}
			}

			return default;
		}

		public IComponent GetComponent(int id, Type typeComponent)
		{
			IComponent component;
			if (components[typeComponent].TryGetValue(id, out component))
			{
				return component;
			}

			return default;
		}

		public bool ContainsComponent(int id, Type componentType)
		{
			if (!components.ContainsKey(componentType))
			{
				return false;
			}

			return components[componentType].ContainsKey(id);
		}


		public IDictionary<Type, IComponent> GetComponents(int id)
		{
			return components.Where(x => x.Value.ContainsKey(id))
				.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault().Value);
		}

		public void AddSystem<T>() where T : ISystem
		{
			if (!systems.Any(x => x.GetType() == typeof(T)))
			{
				ISystem system = CreateSystem(typeof(T));
				systems.Add(system);
			}
		}

		private ISystem CreateSystem(Type type)
		{
			object instanceObject = Injection.InstanceObject(type);
			(instanceObject as SystemBase).WorldState = this;
			(instanceObject as SystemBase).Input = Input;
			return (ISystem)instanceObject;
		}

		public void RemoveSystem<T>() where T : ISystem
		{
			ISystem system = systems.FirstOrDefault(x => x.GetType() == typeof(T));
			systems.Remove(system);
		}

		public void RunSystems()
		{
			systems.ForEach(x => x.Execute());
		}
	}
}
