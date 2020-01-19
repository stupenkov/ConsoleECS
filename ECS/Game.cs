using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace ECS
{
	public class Game
	{
		private Scene scene;

		public Game(Scene scene)
		{
			foreach (var item in Assembly.GetEntryAssembly().GetTypes())
			{
				if (item.BaseType == typeof(SystemBase))
				{
					Debug.Print(item.Name); 
				}

			}
			this.scene = scene;
		}

		public bool IsRun { get; set; } = true;

		public void GameLoop(Action action)
		{
			scene.Run();
			while (IsRun)
			{
				scene.World.RunSystems();
				action?.Invoke();
				Thread.Sleep(1);
			}
		}
	}
}
