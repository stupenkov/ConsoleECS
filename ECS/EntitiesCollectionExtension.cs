using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
	public static class EntitiesCollectionExtension
	{
		public static IEnumerable<Entity> Has(this IEnumerable<Entity> entities, params Type[] componentTypes)
		{
			CheckType(componentTypes);

			foreach (var item in entities)
			{
				if (item.ContainsComponent(componentTypes.First()))
				{

				}
			}
			var ss = entities.Where(x => x.ContainsComponent(componentTypes.First()));
			var t = entities.Where(x => componentTypes.All(y => x.ContainsComponent(y)));
			return t;
		}

		public static IEnumerable<Entity> Exclude(this IEnumerable<Entity> entities, params Type[] componentTypes)
		{
			CheckType(componentTypes);

			return entities.Where(x => componentTypes.All(y => !x.ContainsComponent(y))).ToList();
		}

		public static void Foreach(this IEnumerable<Entity> entities, Action<Entity> action)
		{
			foreach (var entity in entities.ToList())
			{
				action.Invoke(entity);
			}
		}

		public static void Foreach<T1>(this IEnumerable<Entity> entities, Action<Entity, T1> action)
			where T1 : IComponent
		{
			foreach (var entity in entities.ToList())
			{
				T1 component1 = entity.GetComponent<T1>();
				if (component1 == null)
				{
					continue;
				}

				action.Invoke(entity, component1);
			}
		}

		public static void Foreach<T1, T2>(this IEnumerable<Entity> entities, Action<Entity, T1, T2> action)
			where T1 : IComponent
			where T2 : IComponent
		{
			foreach (var entity in entities.ToList())
			{
				T1 component1 = entity.GetComponent<T1>();
				T2 component2 = entity.GetComponent<T2>();
				if (component1 == null || component2 == null)
				{
					continue;
				}

				action.Invoke(entity, component1, component2);
			}
		}

		public static void Foreach<T1, T2, T3>(this IEnumerable<Entity> entities, Action<Entity, T1, T2, T3> action)
			where T1 : IComponent
			where T2 : IComponent
			where T3 : IComponent
		{
			foreach (var entity in entities.ToList())
			{
				T1 component1 = entity.GetComponent<T1>();
				T2 component2 = entity.GetComponent<T2>();
				T3 component3 = entity.GetComponent<T3>();
				if (component1 == null || component2 == null || component3 == null)
				{
					continue;
				}

				action.Invoke(entity, component1, component2, component3);
			}
		}

		private static void CheckType(Type[] componentTypes)
		{
			if (!componentTypes.All(x => x.GetInterface(nameof(IComponent)) != null))
			{
				throw new Exception($"Тип аргумента не соответствует типу {nameof(IComponent)}");
			}
		}
	}
}
