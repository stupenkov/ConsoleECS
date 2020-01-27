using System;

namespace ECS.Input
{
	public enum InputMode
	{
		Key,
		Char
	}

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

		public InputMode InputMode { get; set; }

		public bool Enable { get; set; } = true;

		public void GetInput()
		{
			if (!Enable)
			{
				return;
			}

			switch (InputMode)
			{
				case InputMode.Key:
					break;
				case InputMode.Char:
					break;
				default:
					break;
			}

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
