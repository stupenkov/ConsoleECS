using System;
using ECS.Input;

namespace ECS.ConsoleUI
{
	[GroupInputSystems]
	public class InputSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Input.GetKey();

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
