using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS;
using ECS.Drawing;
using ECS.Numerics;
using ECS.ConsoleUI;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(CreationUIGroup))]
	public class ListItemsUISystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			Entities
				.Foreach((Entity entity, ListItemsComponent listItems, PropertiesUIComponent properties, TransformComponent transform) =>
				{
					int width = 0;
					int height = 0;

					foreach (var item in listItems.Items)
					{
						item.AddComponent(new InnerComponent { Parent = entity });

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
					bitmap.FillBackgroundColor(properties.Colors.Background);
					transform.Size = new Vector2(width, height);

					//WindowComponent window = Entities.Has(typeof(WindowComponent)).FirstOrDefault().GetComponent<WindowComponent>();
					//window.SetPositionCenter(ref transform);
					entity.AddComponent(new SpriteComponent { Bitmap = bitmap });

				});
		}
	}
}
