using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ECS.Input;

namespace ECS
{
	public class World
	{
		private Dictionary<Type, Dictionary<int, IComponent>> components = 
			new Dictionary<Type, Dictionary<int, IComponent>>();

		private List<ISystem> _renderingSystems = new List<ISystem>();
		private List<ISystem> _inputSystems = new List<ISystem>();
		private List<ISystem> _updateSystems = new List<ISystem>();

		private bool _firstRunSystems = true;

		public DependencyInjection Injection { get; set; } = new DependencyInjection();

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

		private ISystem CreateSystem(Type type)
		{
			object instanceObject = Injection.InstanceObject(type);
			(instanceObject as SystemBase).WorldState = this;
			(instanceObject as SystemBase).Input = Input;
			return (ISystem)instanceObject;
		}

		public void RunSystems()
		{
			if (_firstRunSystems)
			{
				_firstRunSystems = false;
				CreateAllSystems();
			}

			_updateSystems.ForEach(x => x.Execute());
			_renderingSystems.ForEach(x => x.Execute());
			_inputSystems.ForEach(x => x.Execute());
		}

		private void CreateAllSystems()
		{
			IEnumerable<Type> allSystems = GetAllSystems();

			foreach (var type in allSystems)
			{
				List<ISystem> systems = SelectGroupSystems(type);

				UpdateBeforeAttribute beforeAttr =
					type.GetCustomAttribute(typeof(UpdateBeforeAttribute)) as UpdateBeforeAttribute;

				UpdateAfterAttribute afterAttr =
					type.GetCustomAttribute(typeof(UpdateAfterAttribute)) as UpdateAfterAttribute;

				if (beforeAttr != null && afterAttr != null)
				{
					throw new Exception($"Нельзя применять аттрибуты {nameof(UpdateBeforeAttribute)} и" +
						$"{nameof(UpdateAfterAttribute)} вместе.");
				}

				int indexInsert = -1;
				if (beforeAttr != null)
				{
					indexInsert = systems.FindIndex(s => s.GetType() == beforeAttr.SystemType) - 1;
				}
				else if (afterAttr != null)
				{
					indexInsert = systems.FindIndex(s => s.GetType() == afterAttr.SystemType) + 1;
				}

				if (indexInsert == -1)
				{
					indexInsert = systems.Count;
				}

				ISystem systemInstance = CreateSystem(type);

				systems.Insert(indexInsert, systemInstance);
			}
		}

		private List<ISystem> SelectGroupSystems(Type type)
		{
			List<ISystem> systems = _updateSystems;
			if (type.GetCustomAttribute(typeof(GroupInputSystems)) != null)
			{
				systems = _inputSystems;
			}
			else if (type.GetCustomAttribute(typeof(GroupRenderingSystems)) != null)
			{
				systems = _renderingSystems;
			}

			if (systems.Any(x => x.GetType() == type))
			{
				throw new Exception($"Попытка повторного добавления системы {type.Name}");
			}

			return systems;
		}

		private static IEnumerable<Type> GetAllSystems()
		{
			Assembly internalAssembly = Assembly.GetCallingAssembly();
			Type[] typesInternalAsm = internalAssembly.GetTypes()
				.Where(x => x.BaseType == typeof(SystemBase)).ToArray();

			Assembly externalAssembly = Assembly.GetEntryAssembly();
			Type[] typesExternalAsm = externalAssembly.GetTypes()
				.Where(x => x.BaseType == typeof(SystemBase)).ToArray();

			var allSystems = typesInternalAsm.Union(typesExternalAsm);
			return allSystems;
		}
	}
}
