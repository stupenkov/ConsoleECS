using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Input;
using ECS.Drawing;
using ConsolePaint.Components;
using ECS.ConsoleUI;

namespace ConsolePaint.Systems
{
	[DisableCreation]
	public class SavingSystem : SystemBase
	{
		public override void OnUpdate()
		{
			if (Input.KeyAlt == ConsoleKey.S)
			{
				Entities.Has(typeof(CanvasComponent)).Foreach((Entity entity, SpriteComponent sprite) =>
				{
					sprite.Bitmap.Save("test.cbmp");
				});
			}
			else if (Input.KeyAlt == ConsoleKey.L)
			{
				Entities.Has(typeof(CanvasComponent)).Foreach((Entity entity, SpriteComponent sprite) =>
				{
					sprite.Bitmap = Bitmap.Load("test.cbmp");
				});
			}
		}
	}
}
