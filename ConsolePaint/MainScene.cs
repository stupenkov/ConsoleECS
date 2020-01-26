using System;
using System.Collections.Generic;
using ECS;
using ECS.Drawing;
using ConsolePaint.Components;
using ECS.Numerics;
using ECS.ConsoleUI;
using System.Diagnostics;

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
			_UICreator = new UICreator(World);

			injection.RegisterType<LayoutUI>(_layoutUI);
			injection.RegisterType<UICreator>(_UICreator);
		}

		protected override void RegisterEntities(World world)
		{
			_colorBPanel = EntityManager.CreateEntity("ColorPanelB");
			_colorFPanel = EntityManager.CreateEntity("ColorPanelF");
			_canvas = EntityManager.CreateEntity("Canvas");
			_menuList = EntityManager.CreateEntity("MenuList");
			_textEdit = EntityManager.CreateEntity("TextEdit");
		}

		protected override void ConfigureEntities()
		{
			ConfigureCanvas();

			Entity lbBackgrd = _UICreator.CreateLabel("Background color:", new Vector2(0, 5));
			Entity clbrBackgrd = _UICreator.CreateColorBar(new Vector3(18, 5, 1));
			Entity lbTextColor = _UICreator.CreateLabel("Text color:", new Vector2(0, 7));
			Entity clbrTextColor = _UICreator.CreateColorBar(new Vector3(18, 7, 0));

			Entity liMenu = _UICreator.CreateListItems(
				"MainMenu",
				new Vector3(3, 1, 100),
				new List<Entity>
				{
					_UICreator.CreateButton("New file", "NewFile"),
					_UICreator.CreateButton("Open file", "OpenFile"),
				});
			liMenu.AddComponent<HiddenComponent>();

			Entity btnMenu = _UICreator.CreateButton(new Vector3(0, 0, 0), "Menu", "VisibleToggel", liMenu);
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
