using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Drawing;

namespace ECS.ConsoleUI
{
	[DisableCreation]
	public class StepperUISystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach(
				(Entity entity, StepperComponent stepper, SpriteComponent sprite) =>
				{
					Bitmap bitmap = sprite.Bitmap.GetCopy();


				});
		}
	}
}
