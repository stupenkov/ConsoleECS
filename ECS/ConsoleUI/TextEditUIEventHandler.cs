using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Input;

namespace ECS.ConsoleUI
{
	public class TextEditUIEventHandler : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity, TextEditComponent textEdit) =>
			{
				Console.BackgroundColor = textEdit.Mask.Background;
				textEdit.Text = Console.ReadLine();
			});
		}
	}
}
