using System;
using System.Collections.Generic;
using System.Text;
using ECS.Input;

namespace ECS.Core
{
	public class DisplayText
	{
		private Carriage _carriage;
		private StringBuilder _text = new StringBuilder(".");

		public DisplayText(int displayLength)
		{
			DisplayLength = displayLength;
			_carriage = new Carriage(displayLength, _text);
		}

		public int CursorTextPosition => _carriage.LeftPos + CursorDisplayPos;

		public int CursorDisplayPos { get; private set; }

		public int DisplayLength { get; private set; }

		public void InsertChar(char symbol)
		{
			_text.Insert(CursorTextPosition, symbol);

			if (CursorDisplayPos < DisplayLength - 1)
			{
				CursorDisplayPos++;
			}
			else
			{
				_carriage.MoveRight();
			}
		}

		public void CursorMoveLeft()
		{
			if (CursorDisplayPos > 0)
			{
				CursorDisplayPos--;
			}
			else
			{
				_carriage.MoveLeft();
			}
		}

		public void CursorMoveRight()
		{
			if (CursorDisplayPos < DisplayLength - 1 && CursorTextPosition < _text.Length - 1)
			{
				CursorDisplayPos++;
			}
			else
			{
				_carriage.MoveRight();
			}
		}

		public void RemoveSymbolAfter()
		{
			if (CursorTextPosition <= _text.Length - 2)
			{
				_text.Remove(CursorTextPosition, 1);
				if (_text.Length < DisplayLength)
				{
					CursorMoveLeft();
				}
				else
				{
					_carriage.MoveLeft();
				}
			}
		}

		public void RemoveSymbolBefore()
		{
			if (CursorTextPosition <= _text.Length - 1 && CursorTextPosition > 0)
			{
				_text.Remove(CursorTextPosition - 1, 1);
				if (_text.Length < DisplayLength)
				{
					CursorMoveLeft();
				}
				else
				{
					_carriage.MoveLeft();
				}
			}
		}

		public string GetDislplayText()
		{
			int textLength = _text.Length < DisplayLength ? _text.Length : DisplayLength;

			return _text.ToString(_carriage.LeftPos, textLength);
		}

		public string GetAllText()
		{
			return _text.ToString(0, _text.Length - 1);
		}

		private class Carriage
		{
			private int _displayLength;
			private StringBuilder _text;

			public Carriage(int displayLength, StringBuilder text)
			{
				_displayLength = displayLength;
				_text = text;
			}

			public int LeftPos { get; private set; }

			public int RightPos => LeftPos + _displayLength;

			public void MoveRight()
			{
				if (_text.Length > RightPos)
				{
					LeftPos++;
				}
			}

			public void MoveLeft()
			{
				if (LeftPos > 0)
				{
					LeftPos--;
				}
			}
		}
	}
}
