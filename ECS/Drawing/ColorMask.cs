using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Drawing
{
	/// <summary>
	/// Представляет цвет текста и фона.
	/// </summary>
	[Serializable]
	public struct ColorMask
	{
		public ConsoleColor Background;

		public ConsoleColor ColorText;

		public static ColorMask Default =>
			new ColorMask { Background = ConsoleColor.Black, ColorText = ConsoleColor.White };

		public override string ToString()
		{
			return $"Background color: {Background}, Text color: {ColorText}";
		}
	}
}
