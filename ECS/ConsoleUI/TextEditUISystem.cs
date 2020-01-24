using System;
using System.Collections.Generic;
using System.Text;
using ECS.Drawing;

namespace ECS.ConsoleUI
{
	[GroupRenderingSystems]
	[UpdateBefore(typeof(AutoSizeSystem))]
	public class TextEditUISystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, TextEditComponent textEdit, TransformComponent transform) =>
			{
				Bitmap bitmap = new Bitmap(textEdit.Length, 1);
				bitmap.FillColor(textEdit.Mask.Background);
				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
			});
		}
	}
}
