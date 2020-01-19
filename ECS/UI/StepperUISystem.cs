﻿using System;
using System.Collections.Generic;
using System.Text;
using ECS.BasicElemets;
using ECS.Drawing;

namespace ECS.UI
{
	public class StepperUISystem : SystemBase
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
