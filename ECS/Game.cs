using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace ECS
{
	public class Game
	{
		private Scene scene;

		public World World => scene.World;

		public Game(Scene scene)
		{
			this.scene = scene;
		}

		public bool IsRun { get; set; } = true;

		public void GameLoop(Action action)
		{
			scene.Run();
			scene.World.InitializeSystems();
			while (IsRun)
			{
				scene.World.RunSystems();

				action?.Invoke();
				Thread.Sleep(1);
			}
		}
	}
}
