using System;
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
			game.GameLoop(() => {

				
			});
		}
	}
}
