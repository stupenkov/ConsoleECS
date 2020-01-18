using System;

namespace ECS.Drawing
{
	[Serializable]
	public struct Pixel
	{
		public ConsoleColor Color;
		public ConsoleColor BackgroundColor;
		public char Symbol;

		public bool IsDefault => Color == 0 && BackgroundColor == 0 && Symbol == '\0';

		public void Clear()
		{
			Color = 0;
			BackgroundColor = 0;
			Symbol = '\0';
		}
	}
}
