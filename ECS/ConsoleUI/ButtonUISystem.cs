using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(CreationUIGroup))]
	public class ButtonUISystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, ButtonComponent button, TransformComponent transform, PropertiesUIComponent properties) =>
			{
				bool isContainsCursorHover = entity.ContainsComponent<CursorHoverComponent>();
				ColorMask colorMask = isContainsCursorHover ? properties.ActiveColors : properties.Colors;
				StringBuilder sb = new StringBuilder(button.Caption);
				sb.Insert(0, new string(' ', properties.Padding.Left));
				sb.Append(new string(' ', properties.Padding.Right));
				Bitmap textBitmap = Bitmap.CreateFromText(sb.ToString(), colorMask);

				int height = properties.Padding.Top + properties.Padding.Bottom + textBitmap.Size.Y;

				Bitmap bitmap = new Bitmap(new Vector2(sb.Length, height));
				bitmap.FillColor(colorMask.Background);
				bitmap.AddBitmap(0, properties.Padding.Top, textBitmap);

				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });

				if (transform.Autosize && transform.Size == Vector2.Zero)
				{
					transform.Size = bitmap.Size;
				}
			});
		}
	}
}
