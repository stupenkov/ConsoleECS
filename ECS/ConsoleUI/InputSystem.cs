using System;
using ECS.Input;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(InputSystemGroup))]
	public class InputSystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			Input.GetInput();

			switch (Input.Key)
			{
				case ConsoleKey.LeftArrow:
					Cursor.MoveLeft();
					break;
				case ConsoleKey.RightArrow:
					Cursor.MoveRight();
					break;
				case ConsoleKey.UpArrow:
					Cursor.MoveUp();
					break;
				case ConsoleKey.DownArrow:
					Cursor.MoveDown();
					break;
			}
		}
	}
}
