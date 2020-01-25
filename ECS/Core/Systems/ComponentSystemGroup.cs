using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
	public abstract class ComponentSystemGroup : ComponentSystem
	{
		public void AddSystem(ComponentSystem system)
		{
			Systems.Add(system);
		}

		internal List<ComponentSystem> Systems { get; set; } = new List<ComponentSystem>();

		//public void RemoveSystem(ComponentSystem system)
		//{
		//	_systems.Remove(system);
		//}

		//public bool	ContainsSystem(ComponentSystem system)
		//{
		//	return _systems.Contains(system);
		//}

		public override void OnStart()
		{
			Systems.ForEach(x => x.OnStart());
		}

		public override void OnUpdate()
		{
			Systems.ForEach(x => x.OnUpdate());
		}
	}
}
