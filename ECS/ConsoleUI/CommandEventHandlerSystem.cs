using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ECS.ConsoleUI
{
	[GroupInputSystems]
	[UpdateAfter(typeof(InputSystem))]
	public class CommandEventHandlerSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(CursorHoverComponent))
				.Foreach((Entity entity, CommandComponent command) =>
				{
					if (Input.Key != ConsoleKey.Enter)
					{
						return;
					}

					if (command.CommandName == "commandTest0")
					{
						Debug.Print(command.CommandName);
					}
				});
		}
	}
}
