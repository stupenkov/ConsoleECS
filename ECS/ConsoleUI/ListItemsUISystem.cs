using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS;
using ECS.Drawing;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[GroupRenderingSystems]
	//[UpdateAfter(typeof(AutoSizeSystem))]
	[UpdateBefore(typeof(CentrSystem))]
	public class ListItemsUISystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities
				.Foreach((Entity entity, ListItemsComponent listItems, PropertiesUIComponent properties, TransformComponent transform) =>
				{
					//bool isContainsCursorHover = entity.ContainsComponent<CursorHoverComponent>();
					//ColorMask colorMask = isContainsCursorHover ? properties.ActiveColors : properties.Colors;
					int width = 0;
					int height = 0;
					foreach (var item in listItems.Items)
					{
						TransformComponent transformItem = item.GetComponent<TransformComponent>();
						if (width < transformItem.Size.X)
						{
							width = transformItem.Size.X;
						}


						transformItem.Position.Z = transform.Position.Z + 1;
						transformItem.Position.X = transform.Position.X + properties.Padding.Left;
						transformItem.Position.Y = transform.Position.Y + properties.Padding.Top + height;

						height += transformItem.Size.Y;
					}

					width += properties.Padding.Left + properties.Padding.Right;
					height += properties.Padding.Top + properties.Padding.Bottom;

					Bitmap bitmap = new Bitmap(width, height);
					bitmap.FillColor(properties.Colors.Background);
					transform.Size = new Vector2(width, height);
					entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
				});
		}
	}
}
