using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(InputSystemGroup))]
	[UpdateAfter(typeof(InputSystem))]
	public class CommandEventHandlerSystem : ComponentSystem
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

					if (command.CommandName=="MenuOpen")
					{
						Entities.FirstOrDefault(x => x.Name == "MainMenu").RemoveComponent<HiddenComponent>();
					}
				});
		}
	}
}
