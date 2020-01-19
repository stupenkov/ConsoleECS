using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ECS.Input;

namespace ECS
{
	public class World
	{
		private List<ISystem> _renderingSystems = new List<ISystem>();
		private List<ISystem> _inputSystems = new List<ISystem>();
		private List<ISystem> _updateSystems = new List<ISystem>();

		private bool _firstRunSystems = true;
		private KeyBoard _input = new KeyBoard();
		private Dictionary<Type, Dictionary<int, IComponentData>> _components = new Dictionary<Type, Dictionary<int, IComponentData>>();
		private readonly DependencyInjection _injection = new DependencyInjection();
		private readonly EntityManager _entityManager;
		private readonly List<Entity> _entities = new List<Entity>();

		public World()
		{
			_entityManager = new EntityManager(this);
			AddAllComponents();
		}

		public event EventHandler<List<DataDebug>> UpdateDataDebugs;

		public List<DataDebug> DataDebugs { get; private set; } = new List<DataDebug>();

		public DependencyInjection Injection => _injection;
		public List<Entity> Entities => _entities;
		public KeyBoard Input { get => _input; set => _input = value; }
		public EntityManager EntityManager => _entityManager;
		internal Dictionary<Type, Dictionary<int, IComponentData>> Components { get => _components; private set => _components = value; }
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



			var s = _components.SelectMany(x => x.Value).GroupBy(x => x.Key).Select(x => new
			{
				Id = x.Key,
				Components = EntityManager.GetComponents(x.Key).Values
			});

			DataDebugs.Clear();
			foreach (var elem in s)
			{
				DataDebug dataDebug = new DataDebug();
				dataDebug.Entity = Entities.Where(x => x.Id == elem.Id).FirstOrDefault();
				dataDebug.EntityComponents = new Dictionary<string, List<string>>();
				foreach (var item in elem.Components)
				{
					var fields = item.GetType().GetFields();
					List<string> fieldValues = new List<string>();
					foreach (var f in fields)
					{
						string strField = $"{f.Name} - {f.GetValue(item)}";
						fieldValues.Add(strField);
					}

					dataDebug.EntityComponents.Add(item.GetType().Name, fieldValues);
				}

				DataDebugs.Add(dataDebug);
			}

			UpdateDataDebugs?.Invoke(this, DataDebugs);
		}

		private ISystem CreateSystem(Type type)
		{
			object instanceObject = Injection.InstanceObject(type);
			(instanceObject as SystemBase).WorldState = this;
			(instanceObject as SystemBase).Input = Input;
			return (ISystem)instanceObject;
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
					throw new Exception($"Нельзя применять атрибуты {nameof(UpdateBeforeAttribute)} и" +
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

		private void AddAllComponents()
		{
			Assembly internalAssembly = Assembly.GetCallingAssembly();
			Type[] typesInternalAsm = internalAssembly.GetTypes()
				.Where(x => x.GetInterface(nameof(IComponentData)) != null).ToArray();

			Assembly externalAssembly = Assembly.GetEntryAssembly();
			Type[] typesExternalAsm = externalAssembly.GetTypes()
				.Where(x => x.GetInterface(nameof(IComponentData)) != null).ToArray();

			var allComponentsType = typesInternalAsm.Union(typesExternalAsm);
			foreach (var type in allComponentsType)
			{
				Components.Add(type, new Dictionary<int, IComponentData>());
			}
		}
	}
}
