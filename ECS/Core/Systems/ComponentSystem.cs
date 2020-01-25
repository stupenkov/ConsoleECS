using System.Collections.Generic;
using ECS.Input;

namespace ECS
{
	public abstract class ComponentSystem : IComponentSystem
	{
		public IEnumerable<Entity> Entities => World.Entities;

		public KeyBoard Input { get; internal set; }

		public bool Enable { get; set; } = true;

		public World World { get; internal set; }

		public virtual void OnStart()
		{
		}

		public abstract void OnUpdate();

		void IComponentSystem.Execute()
		{
			if (Enable)
			{
				OnUpdate();
			}
		}

		void IComponentSystem.Start()
		{
			if (Enable)
			{
				OnStart();
			}
		}
	}
}
