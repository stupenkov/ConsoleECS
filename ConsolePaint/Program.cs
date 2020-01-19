using System;
using System.Reflection;
using ECS;
using ECS.Drawing;
using ECS.Input;

namespace ConsolePaint
{
	class Program
	{
		static void Main(string[] args)
		{
			MainScene mainScene = new MainScene();
			Game game = new Game(mainScene);
			DataTransfer dataTransfer = new DataTransfer(game.World);
			game.GameLoop(() => {

				
			});
		}
	}
}
