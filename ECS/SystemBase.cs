using System.Collections.Generic;
using ECS.Input;

namespace ECS
{
	public abstract class SystemBase : ISystem
	{
		private World worldState;

		public IEnumerable<Entity> Entities => WorldState.Entities;

		public KeyBoard Input { get; internal set; }

		public bool Enable { get; set; } = true;

		internal World WorldState
		{
			get => worldState;
			set
			{
				worldState = value;
				OnStart();
			}
		}

		public virtual void OnStart()
		{
		}

		public abstract void OnUpdate();

		void ISystem.Execute()
		{
			if (Enable)
			{
				OnUpdate();
			}
		}
	}
}
