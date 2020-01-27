using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Input;

namespace ECS.ConsoleUI
{
	[DisableCreation]
	public class TextEditUIEventHandler : ComponentSystem
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity, TextEditComponent textEdit) =>
			{
				Console.BackgroundColor = textEdit.Mask.Background;
			});
		}
	}
}
