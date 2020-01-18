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

	public class ClickComponent : IComponent { public int X; public int Y; }
	public class CanvasComponent : IComponent { }
	public class ColorPanelComponent : IComponent { public ColorPanelType PanelType; }
	public class BrushComponent : IComponent { public Pixel Pixel; public bool IsBackground = true; public bool IsForeground = true; public bool IsSymbol = true; }
}
