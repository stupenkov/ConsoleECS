using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
	public class DependencyInjection
	{
		private Dictionary<Type, object> types = new Dictionary<Type, object>();

		public void RegisterType<TInerface, TClass>() where TClass : class, TInerface
		{
			throw new NotImplementedException();
		}

		public void RegisterType<TClass>(object instance) where TClass : class
		{
			types.Add(typeof(TClass), instance);
		}

		public object InstanceObject(Type type)
		{
			List<object> arguments = new List<object>();
			var p = type.GetConstructors().FirstOrDefault().GetParameters();
			foreach (var param in p)
			{
				if (types.ContainsKey(param.ParameterType))
				{
					arguments.Add(types[param.ParameterType]);
				}
				else
				{
					throw new Exception("Не найден зарегистрированный тип для аргумента конструктора: " + type.ToString());
				}
			}

			if (arguments.Count == 0)
			{
				return Activator.CreateInstance(type);

			}

			return Activator.CreateInstance(type, arguments.ToArray());
		}
	}
}
