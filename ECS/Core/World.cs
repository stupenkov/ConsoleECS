using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ECS.Input;

namespace ECS
{
	public class World
	{
		private RootSystemGroup _rootSystemGroup = new RootSystemGroup();
		private List<ComponentSystemGroup> _allGroups = new List<ComponentSystemGroup>();

		public World()
		{
			EntityManager = new EntityManager(this);
			AddAllComponentTypes();
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
			foreach (var group in _allGroups)
			{
				SortSystems(group);
			}

			_rootSystemGroup.OnStart();
		}

		/// <summary>
		/// Сортирует системы в группах.
		/// </summary>
		/// <param name="systemGroup">Группа для сортировки.</param>
		private void SortSystems(ComponentSystemGroup systemGroup)
		{
			List<ComponentSystem> group = systemGroup.Systems;
			string nameGroup = systemGroup.GetType().Name;
			int currentIndex = -1;
			int newIndex = -1;
			bool dirty = true;
			while (dirty)
			{
				foreach (var system in group)
				{
					currentIndex = group.FindIndex(s => s.GetType() == system.GetType());
					UpdateBeforeAttribute before = system.GetType().GetCustomAttributes<UpdateBeforeAttribute>().FirstOrDefault();
					UpdateAfterAttribute after = system.GetType().GetCustomAttributes<UpdateAfterAttribute>().FirstOrDefault();

					int beforeIndex = -1;
					int afterIndex = -1;

					if (before != null)
					{
						beforeIndex = group.FindIndex(s => s.GetType() == before.SystemType);
						if (beforeIndex == -1)
						{
							throw new Exception($"Система {before.SystemType.Name} не найдена в группе {nameGroup}");
						}
					}

					if (after != null)
					{
						afterIndex = group.FindIndex(s => s.GetType() == after.SystemType);
						if (afterIndex == -1)
						{
							throw new Exception($"Система {after.SystemType.Name} не найдена в группе {nameGroup}");
						}
					}

					if (beforeIndex > -1 && afterIndex > -1)
					{
						if (beforeIndex < afterIndex)
						{
							throw new Exception($"Неправильный порядок систем before: {before.SystemType} " +
								$"и after:{after.SystemType} в системе: {system.GetType().Name}");
						}

						if (afterIndex < currentIndex && currentIndex < beforeIndex)
						{
							continue;
						}
					}

					if (afterIndex > -1 && afterIndex > currentIndex)
					{
						newIndex = afterIndex;
						break;
					}

					if (beforeIndex > -1 && beforeIndex < currentIndex)
					{
						newIndex = beforeIndex - 1 < 0 ? 0 : beforeIndex - 1;
						break;
					}
				}

				if (newIndex > -1 && currentIndex > -1)
				{
					ComponentSystem system = group[currentIndex];
					group.RemoveAt(currentIndex);
					group.Insert(newIndex, system);
					newIndex = -1;
					continue;
				}

				dirty = false;
			}
		}

		internal void RunSystems()
		{
			_rootSystemGroup.OnUpdate();
			CreateDebugInfo();
		}

		/// <summary>
		/// Возвращает все типы систем из подключенных сборок, которые соответствуют типу <see cref="ComponentSystem"/>.
		/// </summary>
		private static IEnumerable<Type> GetAllTypesSystems()
		{
			IEnumerable<Type> types = GetAllAssemblies()
			.SelectMany(x => x.GetTypes())
			.Where(x => x.GetCustomAttribute<DisableCreationAttribute>() == null)
			.Where(x => !x.IsAbstract);

			foreach (var type in types)
			{
				Type t = type;
				if (t == null || t.BaseType == null)
				{
					continue;
				}

				while (t.BaseType != typeof(ComponentSystem))
				{
					if (t == typeof(object))
					{
						t = null;
						break;
					}

					t = t.BaseType;
				}

				if (t != null)
				{
					yield return type;
				}
			}
		}

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

		/// <summary>
		/// Создает экземпляр системы по ее типу и производит инъекцию.
		/// </summary>
		/// <param name="type">Тип системы.</param>
		/// <returns>Созданная система.</returns>
		private ComponentSystem CreateSystem(Type type)
		{
			Type t = type;
			while (t.BaseType != typeof(ComponentSystem))
			{
				if (t.BaseType == typeof(object))
				{
					throw new Exception($"Система не наследует тип: {typeof(ComponentSystem)}");
				}

				t = t.BaseType;
			}

			ComponentSystem instanceObject = (ComponentSystem)Injection.InstanceObject(type);
			instanceObject.World = this;
			instanceObject.Input = Input;
			return instanceObject;
		}

		/// <summary>
		/// Создает экземпляры всех систем и группирует их.
		/// </summary>
		private void CreateAllSystems()
		{
			List<ComponentSystem> instanceSystems = InstanceSystems();

			_allGroups = instanceSystems.Where(x => x.GetType().BaseType == typeof(ComponentSystemGroup)).Cast<ComponentSystemGroup>().ToList();

			_rootSystemGroup = (RootSystemGroup)instanceSystems.FirstOrDefault(x => x.GetType() == typeof(RootSystemGroup));

			List<ComponentSystem> removeSystems = new List<ComponentSystem>();
			while (instanceSystems.Count > 0)
			{
				foreach (var system in instanceSystems)
				{
					if (system.GetType() == typeof(RootSystemGroup))
					{
						removeSystems.Add(system);
						continue;
					}

					UpdateInGroupAttribute group = system.GetType().GetCustomAttribute<UpdateInGroupAttribute>();

					ComponentSystemGroup findGroup = null;
					if (group != null)
					{
						findGroup = (ComponentSystemGroup)instanceSystems.Find(x => group.GroupType == x.GetType());
					}

					if (findGroup == null)
					{
						findGroup = (ComponentSystemGroup)instanceSystems.Find(x => typeof(UpdateSystemGroup) == x.GetType());
						if (findGroup == null)
						{
							throw new Exception($"Не найдена системная группа {nameof(UpdateSystemGroup)}");
						}
					}

					findGroup.AddSystem(system);
					removeSystems.Add(system);
					continue;
				}

				foreach (var system in removeSystems)
				{
					instanceSystems.Remove(system);
				}
			}

			removeSystems.Clear();
		}

		/// <summary>
		/// Создает экземпляры всех систем.
		/// </summary>
		private List<ComponentSystem> InstanceSystems()
		{
			List<ComponentSystem> instanceSystems = new List<ComponentSystem>();
			IEnumerable<Type> allTypeSystems = GetAllTypesSystems().ToList();
			foreach (var type in allTypeSystems)
			{
				ComponentSystem system = CreateSystem(type);
				instanceSystems.Add(system);
			}

			return instanceSystems;
		}

		private void AddAllComponentTypes()
		{
			foreach (var type in GetAllComponentTypes())
			{
				Components.Add(type, new Dictionary<int, IComponentData>());
			}
		}

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
						else if (f.GetValue(item) is IEnumerable<string>)
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
	}
}
