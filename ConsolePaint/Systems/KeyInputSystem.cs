using System;
using System.Collections.Generic;
using System.Text;
using ConsolePaint.Components;
using ECS.ConsoleUI;
using ECS;
using ECS.Input;

namespace ConsolePaint.Systems
{
	[DisableCreation]
	public class KeyInputSystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			if (Input.Key == ConsoleKey.Escape)
			{
				Entity lastActive;
				Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity) =>
				{
					lastActive = entity;
					entity.RemoveComponent<ActiveComponent>();
					Cursor.Enable = false;
				});


				Entities.Has(typeof(MenuComponent)).Foreach(
					(Entity entity, ModalDialogComponent dialog, TransformComponent transform) =>
					{
						entity.AddComponent<ActiveComponent>();
						transform.Position.Z = 100;
					});
			}

			if (Input.Key == ConsoleKey.Enter)
			{
				Entities.Foreach((Entity entity, ActiveComponent active, ListItemsComponent menuList) =>
				{
					if (active.PreviousActive != null)
					{
						active.PreviousActive.AddComponent<ActiveComponent>();
						entity.RemoveComponent<ActiveComponent>();
					}
				});

				Entities.Exclude(typeof(ListItemsComponent)).Foreach((Entity entity, ActiveComponent active) =>
				{
					entity.RemoveComponent<ActiveComponent>();

					Entities.Has(typeof(ListItemsComponent)).Foreach((Entity entity) =>
					{
						entity.AddComponent(new ActiveComponent { PreviousActive = entity });
					});
				});
			}

			if (Input.Key == ConsoleKey.DownArrow)
			{
				Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity, ListItemsComponent menuList) =>
				{
					if (menuList.SelectedIndex == menuList.Items.Count - 1)
					{
						menuList.SelectedIndex = 0;
					}
					else
					{
						menuList.SelectedIndex++;
					}
				});
			}

			if (Input.Key == ConsoleKey.UpArrow)
			{
				Entities.Has(typeof(ActiveComponent)).Foreach((Entity entity, ListItemsComponent menuList) =>
				{
					if (menuList.SelectedIndex == 0)
					{
						menuList.SelectedIndex = menuList.Items.Count - 1;
					}
					else
					{
						menuList.SelectedIndex--;
					}
				});
			}
		}
	}
}
