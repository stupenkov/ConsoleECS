using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS;
using ECS.BasicElemets;
using ECS.Drawing;

namespace ECS.ConsoleUI
{
	public class MenuListCreationSystem : SystemBase
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, MenuListComponent menuList) =>
			{
				int widht = menuList.Items.Max(x => x.Length);
				int height = menuList.Items.Count;
				Bitmap bitmap = new Bitmap(widht, height);
				bitmap.FillColor(menuList.ColorElement.Background);
				for (int i = 0; i < menuList.Items.Count; i++)
				{
					ColorMask mask = menuList.SelectedIndex == i ? menuList.ColorSelect : menuList.ColorElement;
					Bitmap textBitmap = Bitmap.CreateFromText(menuList.Items[i], mask);
					bitmap.AddBitmap(0, i, textBitmap);
				}

				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
			});
		}
	}
}
