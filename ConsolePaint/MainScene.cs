using System;
using System.Collections.Generic;
using ECS;
using ECS.BasicElemets;
using ECS.Drawing;
using ECS.Numerics;
using ECS.UI;
using ConsolePaint.Components;
using ConsolePaint.Systems;

namespace ConsolePaint
{
	public class MainScene : Scene
	{
		private Entity _colorBPanel;
		private Entity _LableColorBPanel;
		private Entity _colorFPanel;
		private Entity _canvas;
		private LayoutUI _layoutUI = new LayoutUI();
		private UICreator _UICreator;

		protected override void Inject(DependencyInjection injection)
		{
			injection.RegisterType<LayoutUI>(_layoutUI);
		}

		protected override void RegisterEntities(World world)
		{
			_colorBPanel = world.CreateEntity("ColorPanelB");
			_colorFPanel = world.CreateEntity("ColorPanelF");
			_canvas = world.CreateEntity("Canvas");
			_LableColorBPanel = world.CreateEntity("LableColorPanelB");
			_UICreator = new UICreator(world);
		}

		protected override void ConfigureEntities()
		{
			ConfigureBColorPanel();
			ConfigureLableBColorPanel();
			ConfigureFColorPanel();
			ConfigureCanvas();

			_layoutUI.Add(_LableColorBPanel);
			_layoutUI.AddRight(_colorBPanel);
			_layoutUI.AddBottom(_colorFPanel, 1);
			_layoutUI.AddBottom(_canvas, 1);

			//Entity textedit = _UICreator.CreateTextEdit(new Vector2(0, 0), 10);

			Rendering.InitializeView(_layoutUI.GetSize());
		}

		protected override void RegisterSystems(World world)
		{
			world.AddSystem<BrushSystem>();
			world.AddSystem<PaintSystem>();
			world.AddSystem<SavingSystem>();
		}

		private void ConfigureLableBColorPanel()
		{
			Bitmap bitmap = Bitmap.CreateFromText("(B)ackground color:");
			_LableColorBPanel.AddComponents(
				new TransformComponent(),
				new SpriteComponent { Bitmap = bitmap });
		}

		private void ConfigureBColorPanel()
		{
			Bitmap colorPanelBitmap = CreateBitmapColorPanel();
			NavigateComponent colorPanelNavigate = new NavigateComponent();
			colorPanelNavigate.Navigate.Add(ConsoleKey.Spacebar, _canvas);

			_colorBPanel.AddComponents(
				new TransformComponent(),
				new ColorPanelComponent { PanelType = ColorPanelType.Background },
				new SpriteComponent { Bitmap = colorPanelBitmap },
				colorPanelNavigate);
		}

		private void ConfigureFColorPanel()
		{
			Bitmap colorPanelBitmap = CreateBitmapColorPanel();
			NavigateComponent colorPanelNavigate = new NavigateComponent();
			colorPanelNavigate.Navigate.Add(ConsoleKey.Spacebar, _canvas);

			_colorFPanel.AddComponents(
				new TransformComponent(),
				new ColorPanelComponent { PanelType = ColorPanelType.Foreground },
				new SpriteComponent { Bitmap = colorPanelBitmap },
				colorPanelNavigate);
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
				new TransformComponent(),
				new SpriteComponent { Bitmap = canvasBitmap },
				new ActiveComponent(),
				new BrushComponent { Pixel = new Pixel { Color = ConsoleColor.Red } },
				canvasNavigate,
				new CanvasComponent());
		}

		private Bitmap CreateBitmapColorPanel()
		{
			Bitmap colorPanelBitmap = new Bitmap(16, 1);
			Pixel p = colorPanelBitmap.GetPixel(0, 0);
			p.Color = ConsoleColor.White;
			p.Symbol = 'B';
			colorPanelBitmap.SetPixel(0, 0, p);
			colorPanelBitmap.Foreach((int x, int y, ref Pixel p) =>
			{
				p.BackgroundColor = (ConsoleColor)x;
			});
			return colorPanelBitmap;
		}
	}
}
