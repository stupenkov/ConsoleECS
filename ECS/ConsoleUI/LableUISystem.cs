using System;
using System.Collections.Generic;
using System.Text;
using ECS.Drawing;

namespace ECS.ConsoleUI
{
	[GroupRenderingSystems]
	[UpdateBefore(typeof(AutoSizeSystem))]
	public class LableUISystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, LableComponent lable) =>
			{
				Bitmap bitmap = Bitmap.CreateFromText(lable.Text, lable.Mask);
				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
			});
		}
	}
}
