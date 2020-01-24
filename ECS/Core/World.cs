using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using ECS.Input;

namespace ECS
{
	public class World
	{
		private List<ISystem> _inputSystemsGroup = new List<ISystem>();
		private List<ISystem> _updateSystemsGroup = new List<ISystem>();
		private List<ISystem> _renderingSystemsGroup = new List<ISystem>();

		public World()
		{
			EntityManager = new EntityManager(this);
			AddAllComponentsTypes();
		}

		public event EventHandler<List<DataDebug>> UpdateDataDebugs;

		public List<DataDebug> DataDebugs { get; private set; } = new List<DataDebug>();

		public DependencyInjection Injection { get; } = new DependencyInjection();

		public List<Entity> Entities { get; } = new List<Entity>();

		public KeyBoard Input { get; set; } = new KeyBoard();

		public EntityManager EntityManager { get; }

		internal Dictionary<Type, Dictionary<int, IComponentData>> Components { get; private set; } =
			new Dictionary<Type, Dictionary<int, IComponentData>>();

		internal void InitializeSystems()
		{
			CreateAllSystems();
			_updateSystemsGroup.ForEach(x => x.Start());
			_renderingSystemsGroup.ForEach(x => x.Start());
			_inputSystemsGroup.ForEach(x => x.Start());
		}

		internal void RunSystems()
		{
			_updateSystemsGroup.ForEach(x => x.Execute());
			_renderingSystemsGroup.ForEach(x => x.Execute());
			_inputSystemsGroup.ForEach(x => x.Execute());
			CreateDebugInfo();
		}

		/// <summary>
		/// Возвращает все типы систем из подключенных сборок.
		/// </summary>
		private static IEnumerable<Type> GetAllTypesSystems() =>
			GetAllAssemblies()
			.SelectMany(x => x.GetTypes())
			.Where(x => x.BaseType == typeof(SystemBase))
			.Where(x => x.GetCustomAttribute<DisableCreationAttribute>() == null);

		/// <summary>
		/// Возвращает все сборки приложения.
		/// </summary>
		private static IEnumerable<Assembly> GetAllAssemblies()
		{
			yield return Assembly.GetEntryAssembly();
			AssemblyName[] allAssemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies();
			foreach (var asmName in allAssemblyNames)
			{
				yield return Assembly.Load(asmName);
			}
		}

		/// <summary>
		/// Возвращает все типы компонентов из подключенных сборок.
		/// </summary>
		private static IEnumerable<Type> GetAllComponentTypes() =>
			GetAllAssemblies()
			.SelectMany(x => x.GetTypes())
			.Where(x => x.GetInterface(nameof(IComponentData)) != null);

		private void CreateDebugInfo()
		{
			var s = Components.SelectMany(x => x.Value).GroupBy(x => x.Key).Select(x => new
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
						StringBuilder sb = new StringBuilder();
						string strField = string.Empty;
						if (f.FieldType.GetInterface(nameof(IDictionary<object, object>)) != null)
						{
							sb.Append("[");
							foreach (var str in f.GetValue(item) as IEnumerable<KeyValuePair<ConsoleKey, Entity>>)
							{
								sb.Append($"{str.Key}:{str.Value    }; ");
							}
							strField = $"{f.Name} - {sb.ToString()}]";
						}
						else if (f.GetValue(item) is IEnumerable<object>)
						{
							sb.Append("[");
							foreach (var str in f.GetValue(item) as IEnumerable<string>)
							{
								sb.Append($"{str}; ");
							}
							strField = $"{f.Name} - {sb.ToString()}]";
						}
						else
						{
							strField = $"{f.Name} - {f.GetValue(item)}";
						}
						fieldValues.Add(strField);
					}

					dataDebug.EntityComponents.Add(item.GetType().Name, fieldValues);
				}

				DataDebugs.Add(dataDebug);
			}

			UpdateDataDebugs?.Invoke(this, DataDebugs);
		}

		/// <summary>
		/// Создает экземпляр системы по ее типу и производит инъекцию.
		/// </summary>
		/// <param name="type">Тип системы.</param>
		/// <returns>Созданная система.</returns>
		private ISystem CreateSystem(Type type)
		{
			if (type.BaseType != typeof(SystemBase))
			{
				throw new Exception($"Система не наследует тип: {typeof(SystemBase)}");
			}

			object instanceObject = Injection.InstanceObject(type);
			(instanceObject as SystemBase).WorldState = this;
			(instanceObject as SystemBase).Input = Input;
			return (ISystem)instanceObject;
		}

		/// <summary>
		/// Добавляет экземпляры всех систем в список.
		/// </summary>
		private void CreateAllSystems()
		{
			IEnumerable<Type> allSystems = GetAllTypesSystems();
			foreach (var type in allSystems)
			{
				List<ISystem> groupSystems = SelectGroupSystems(type);
				ISystem systemInstance = CreateSystem(type);
				groupSystems.Add(systemInstance);
			}

			// TODO: Автоматический поиск группы системы по UpdateBeforeAttribute и UpdateAfterAttribute.
			SortSystemsInGroup(ref _inputSystemsGroup);
			SortSystemsInGroup(ref _updateSystemsGroup);
			SortSystemsInGroup(ref _renderingSystemsGroup);
		}

		private void SortSystemsInGroup(ref List<ISystem> unsortGroup)
		{
			List<ISystem> sortGroup = new List<ISystem>();

			int counter = 0;
			while (unsortGroup.Count > 0)
			{
				ISystem system = unsortGroup[counter++];
				Type type = system.GetType();

				UpdateBeforeAttribute beforeAttr =
					type.GetCustomAttribute(typeof(UpdateBeforeAttribute)) as UpdateBeforeAttribute;

				UpdateAfterAttribute afterAttr =
					type.GetCustomAttribute(typeof(UpdateAfterAttribute)) as UpdateAfterAttribute;

				if (beforeAttr == null && afterAttr == null)
				{
					sortGroup.Add(system);
					unsortGroup.Remove(system);
					counter = 0;
				}
				else if (beforeAttr != null && afterAttr != null)
				{
					int findIndexBefore = sortGroup.FindIndex(s => s.GetType() == beforeAttr.SystemType);
					int findIndexAfter = sortGroup.FindIndex(s => s.GetType() == beforeAttr.SystemType);
					if (findIndexBefore == -1 || findIndexAfter == -1)
					{
						continue;
					}

					if (findIndexBefore > findIndexAfter)
					{
						throw new Exception($"Неправильный порядок систем before: {beforeAttr.SystemType} " +
							$"и after:{afterAttr.SystemType} в системе: {system.GetType().Name}");
					}

					sortGroup.Insert(findIndexAfter + 1, system);
					unsortGroup.Remove(system);
					counter = 0;
				}
				else if (afterAttr != null)
				{
					int findIndexAfter = sortGroup.FindIndex(s => s.GetType() == afterAttr.SystemType);
					if (findIndexAfter == -1)
					{
						continue;
					}

					sortGroup.Insert(findIndexAfter + 1, system);
					unsortGroup.Remove(system);
					counter = 0;
				}
				else if (beforeAttr != null)
				{
					int findIndexBefore = sortGroup.FindIndex(s => s.GetType() == beforeAttr.SystemType);
					if (findIndexBefore == -1)
					{
						continue;
					}

					int insertIndex = findIndexBefore - 1 < 0 ? 0 : findIndexBefore - 1;
					sortGroup.Insert(insertIndex, system);
					unsortGroup.Remove(system);
					counter = 0;
				}
			}

			unsortGroup = sortGroup;
		}

		/// <summary>
		/// Выбирает группу систем для типа.
		/// </summary>
		/// <param name="type">Группа выбирается в зависимости от атрибутов этого типа.</param>
		/// <returns>Группа систем.</returns>
		private List<ISystem> SelectGroupSystems(Type type)
		{
			List<ISystem> systems = _updateSystemsGroup;
			if (type.GetCustomAttribute(typeof(GroupInputSystemsAttribute)) != null)
			{
				systems = _inputSystemsGroup;
			}
			else if (type.GetCustomAttribute(typeof(GroupRenderingSystemsAttribute)) != null)
			{
				systems = _renderingSystemsGroup;
			}

			if (systems.Any(x => x.GetType() == type))
			{
				throw new Exception($"Попытка повторного добавления системы {type.Name}");
			}

			return systems;
		}

		/// <summary>
		/// Добавляет все типы компонентов в словарь.
		/// </summary>
		private void AddAllComponentsTypes()
		{
			foreach (var type in GetAllComponentTypes())
			{
				Components.Add(type, new Dictionary<int, IComponentData>());
			}
		}
	}
}
