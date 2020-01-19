﻿using System;
using System.Collections.Generic;
using ECS;
using ECS.BasicElemets;
using ECS.Drawing;
using ECS.UI;
using ConsolePaint.Components;
using ConsolePaint.Systems;
using ECS.Numerics;

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
		}

		protected override void ConfigureEntities()
		{
			ConfigureCanvas();
			Entity lbBackgrd = _UICreator.CreateLabel("Background color:");
			Entity clbrBackgrd = _UICreator.CreateColorBar(new Vector3(18, 0, 0));
			Entity lbTextColor = _UICreator.CreateLabel("Text color:", new Vector2(0, 2));
			Entity clbrTextColor = _UICreator.CreateColorBar(new Vector3(18, 2, 0));

			_menuList.AddComponents(
				new TransformComponent
				{
					Position = new Vector3(10, 10, 10)
				},
				new MenuListComponent
				{
					Items = new List<string>
					{
						"Создать",
						"Открыть",
						"Сохранить",
					},
					ColorElement = new ColorMask
					{
						Background = ConsoleColor.DarkBlue,
						ColorText = ConsoleColor.White
					},
					ColorSelect = new ColorMask
					{
						Background = ConsoleColor.Cyan,
						ColorText = ConsoleColor.DarkRed
					}
				},
				new DecorationUIComponent
				{
					
				});
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
				new ActiveComponent(),
				new BrushComponent { Pixel = new Pixel { Color = ConsoleColor.Red } },
				canvasNavigate,
				new CanvasComponent());
		}
	}
}
