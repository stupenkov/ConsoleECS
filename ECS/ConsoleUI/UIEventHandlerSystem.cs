using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Input;

namespace ECS.ConsoleUI
{
	public class UIEventHandlerSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity, TextEditComponent textEdit) =>
			{
				Console.ReadLine();
			});
		}
	}
}
