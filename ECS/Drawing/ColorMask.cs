using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Drawing
{
	[Serializable]
	public struct ColorMask
	{
		public ConsoleColor Background;

		public ConsoleColor ColorText;

		public static ColorMask Default =>
			new ColorMask { Background = ConsoleColor.Black, ColorText = ConsoleColor.White };
	}
}
