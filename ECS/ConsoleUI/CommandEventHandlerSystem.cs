using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ECS.Drawing;
using ECS.Input;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(InputSystemGroup))]
	[UpdateAfter(typeof(InputSystem))]
	public class CommandEventHandlerSystem : ComponentSystem
	{
		private Rendering _rendering;
		private readonly UICreator _uICreator;

		public CommandEventHandlerSystem(Rendering rendering, UICreator uICreator)
		{
			_rendering = rendering;
			_uICreator = uICreator;
		}

		public override void OnUpdate()
		{
			Entities.Has(typeof(CursorHoverComponent))
				.Foreach((Entity entity, CommandComponent command) =>
				{
					if (Input.Key == ConsoleKey.Enter)
					{
						if (command.Command == "VisibleToggel")
						{
							if (command.Entity.ContainsComponent<HiddenComponent>())
							{
								command.Entity.RemoveComponent<HiddenComponent>();
							}
							else
							{
								command.Entity.AddComponent<HiddenComponent>();
							}

							return;
						}

						if (command.Command == "NewFile")
						{
							Entity parent = entity.GetComponent<InnerComponent>().Parent;
							parent.AddComponent<HiddenComponent>();

							Entity textEdit = _uICreator.CreateTextEdit(new Vector3(5, 5, 10), 10);

							_rendering.SetView(150, 20);
							Cursor.Reset();
							return;
						}
					}
				});
		}
	}
}
