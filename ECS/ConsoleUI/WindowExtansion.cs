using System;
using System.Collections.Generic;
using System.Text;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	public static class WindowExtansion
	{
		public static void SetPositionCenter(this WindowComponent window, ref TransformComponent transform)
		{
			transform.Position.X = window.Size.X / 2 - transform.Size.X / 2;
			transform.Position.Y = window.Size.Y / 2 - transform.Size.Y / 2;
		}
	}
}
