using System;

namespace ECS.Input
{
	public class KeyBoard
	{
		public KeyBoard()
		{
			Console.TreatControlCAsInput = true;
		}

		public ConsoleKey Key { get; private set; }
		public ConsoleKey KeyAlt { get; private set; }
		public ConsoleKey KeyCtrl { get; private set; }
		public ConsoleKey KeyShift { get; private set; }
		public char KeyChar { get; private set; }
		public void GetKey()
		{
			ConsoleKeyInfo keyInfo = Console.ReadKey(true);
			KeyChar = keyInfo.KeyChar;
			Key = 0;
			KeyAlt = 0;
			KeyCtrl = 0;
			KeyShift = 0;

			switch (keyInfo.Modifiers)
			{
				case ConsoleModifiers.Alt:
					KeyAlt = keyInfo.Key;
					break;
				case ConsoleModifiers.Control:
					KeyCtrl = keyInfo.Key;
					break;
				case ConsoleModifiers.Shift:
					KeyShift = keyInfo.Key;
					break;
				default:
					Key = keyInfo.Key;
					break;
			}
		}
	}
}
