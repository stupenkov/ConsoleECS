﻿using System;
using System.Collections.Generic;
using ECS;
using ECS.Drawing;
using ConsolePaint.Components;
using ECS.Numerics;
using ECS.ConsoleUI;

namespace ConsolePaint
{
	public class MainScene : Scene
	{
		private Entity _colorBPanel;
		private Entity _colorFPanel;
		private Entity _canvas;
		private Entity _dialog;
		private Entity _menuList;
		private LayoutUI _layoutUI = new LayoutUI();
		private UICreator _UICreator;
		private Entity _textEdit;

		protected override void Inject(DependencyInjection injection)
		{
			injection.RegisterType<LayoutUI>(_layoutUI);
		}

		protected override void RegisterEntities(World world)
		{
			_colorBPanel = EntityManager.CreateEntity("ColorPanelB");
			_colorFPanel = EntityManager.CreateEntity("ColorPanelF");
			_canvas = EntityManager.CreateEntity("Canvas");
			_UICreator = new UICreator(world);
			_menuList = EntityManager.CreateEntity("MenuList");
			_textEdit = EntityManager.CreateEntity("TextEdit");
		}

		protected override void ConfigureEntities()
		{
			ConfigureCanvas();
			Entity btnMenu = _UICreator.CreateButton(new Vector3(0, 0, 0), "Menu");
			btnMenu.AddComponent(new CommandComponent { CommandName = "MenuOpen" });

			Entity lbBackgrd = _UICreator.CreateLabel("Background color:", new Vector2(0,5));
			Entity clbrBackgrd = _UICreator.CreateColorBar(new Vector3(18, 5, 1));
			Entity lbTextColor = _UICreator.CreateLabel("Text color:", new Vector2(0, 7));
			Entity clbrTextColor = _UICreator.CreateColorBar(new Vector3(18, 7, 0));

			Entity liMenu = _UICreator.CreateListButton("MainMenu",new Vector3(3, 1, 100), 5);
			liMenu.AddComponent<HiddenComponent>();
			//liMenu.GetComponent<TransformComponent>().Center = true;


			//_menuList.AddComponents(
			//	new TransformComponent
			//	{
			//		Position = new Vector3(10, 10, 10)
			//	},
			//	new ListItemsComponent
			//	{
			//		Items = new List<string>
			//		{
			//			"Создать",
			//			"Открыть",
			//			"Сохранить",
			//		},
			//		ColorElement = new ColorMask
			//		{
			//			Background = ConsoleColor.DarkBlue,	
			//			ColorText = ConsoleColor.White
			//		},
			//		ColorSelect = new ColorMask
			//		{
			//			Background = ConsoleColor.Cyan,
			//			ColorText = ConsoleColor.DarkRed
			//		}
			//	});

			//_textEdit.AddComponents(
			//	new TransformComponent
			//	{
			//		Position = new Vector3(0, 0, 30000)
			//	},
			//	new TextEditComponent
			//	{
			//		Mask = new ColorMask { Background = ConsoleColor.Red }
			//	}
			//	);
		}

		private void ConfigureCanvas()
		{
			Bitmap canvasBitmap = new Bitmap(20, 10);
			canvasBitmap.FillColor(ConsoleColor.White);

			NavigateComponent canvasNavigate = new NavigateComponent();
			canvasNavigate.Navigate = new Dictionary<ConsoleKey, Entity>();
			canvasNavigate.Navigate.Add(ConsoleKey.B, _colorBPanel);
			canvasNavigate.Navigate.Add(ConsoleKey.F, _colorFPanel);

			_canvas.AddComponents(
				new TransformComponent { Position = new Vector3(0, 5, 0) },
				new SpriteComponent { Bitmap = canvasBitmap },
				//new ActiveComponent(),
				new BrushComponent { Pixel = new Pixel { Color = ConsoleColor.Red } },
				canvasNavigate,
				new CanvasComponent());
		}
	}
}
