using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[GroupRenderingSystems]
	[UpdateBefore(typeof(AutoSizeSystem))]
	public class ButtonUISystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, ButtonComponent button, TransformComponent transform) =>
			{
				PropertiesUIComponent prop = entity.GetComponent<PropertiesUIComponent>();
				if (prop == null)
				{
					prop = new PropertiesUIComponent();
					entity.AddComponent(prop);
				}

				bool isContainsCursorHover = entity.ContainsComponent<CursorHoverComponent>();
				ColorMask colorMask = isContainsCursorHover ? prop.ActiveColors : prop.Colors;
				StringBuilder sb = new StringBuilder(button.Caption);
				sb.Insert(0, new string(' ', prop.Padding.Left));
				sb.Append(new string(' ', prop.Padding.Right));
				Bitmap textBitmap = Bitmap.CreateFromText(sb.ToString(), colorMask);

				int height = prop.Padding.Top + prop.Padding.Bottom + textBitmap.Size.Y;

				Bitmap bitmap = new Bitmap(new Vector2(sb.Length, height));
				bitmap.FillColor(colorMask.Background);
				bitmap.AddBitmap(0, prop.Padding.Top, textBitmap);

				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
			});
		}
	}

	[GroupInputSystems]
	public class ButtonUIEventSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity) =>
			{
				if (Input.Key == ConsoleKey.Enter)
				{
					Debug.Print("Button: click Enter");
				}
			});
		}
	}
}
