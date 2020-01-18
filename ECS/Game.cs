using System;
using System.Threading;

namespace ECS
{
	public class Game
	{
		private Scene scene;

		public Game(Scene scene)
		{
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
