using System;
using System.Collections.Generic;
using ECS;
using ECS.Drawing;
using ECS.Numerics;

namespace ConsolePaint.Components
{
	public enum ColorPanelType
	{
		Background,
		Foreground,
		Symbol
	}

	public class ClickComponent : IComponentData { public int X; public int Y; }
	public class CanvasComponent : IComponentData { }
	public class ColorPanelComponent : IComponentData { public ColorPanelType PanelType; }
	public class BrushComponent : IComponentData { public Pixel Pixel; public bool IsBackground = true; public bool IsForeground = true; public bool IsSymbol = true; }

	public class MenuComponent: IComponentData
	{

	}

}
