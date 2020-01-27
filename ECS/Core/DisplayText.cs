using System.Text;

namespace ECS.Core
{
	/// <summary>
	/// Представляет редактируемый текст в ограниченной области.
	/// </summary>
	public class DisplayText
	{
		private TextArea _textArea;
		private StringBuilder _text = new StringBuilder(" ");

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="DisplayText"/>
		/// </summary>
		/// <param name="displayLength">Длина области вывода текста в символах.</param>
		public DisplayText(int displayLength)
		{
			DisplayLength = displayLength;
			_textArea = new TextArea(displayLength, _text);
		}

		/// <summary>
		/// Возвращает позицию курсора в тексте.
		/// </summary>
		public int CursorTextPosition => _textArea.LeftPos + CursorDisplayPos;

		/// <summary>
		/// Возвращает позицию курсора в ограниченной области.
		/// </summary>
		public int CursorDisplayPos { get; private set; }

		/// <summary>
		/// Возвращает размер ограниченной области вывода текста.
		/// </summary>
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
				_textArea.MoveRight();
			}
		}

		/// <summary>
		/// Перемещает курсор влево.
		/// </summary>
		public void CursorMoveLeft()
		{
			if (CursorDisplayPos > 0)
			{
				CursorDisplayPos--;
			}
			else
			{
				_textArea.MoveLeft();
			}
		}

		/// <summary>
		/// Перемещает курсор вправо.
		/// </summary>
		public void CursorMoveRight()
		{
			if (CursorDisplayPos < DisplayLength - 1 && CursorTextPosition < _text.Length - 1)
			{
				CursorDisplayPos++;
			}
			else
			{
				_textArea.MoveRight();
			}
		}

		/// <summary>
		/// Удаляет символ после курсора.
		/// </summary>
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
					_textArea.MoveLeft();
				}
			}
		}

		/// <summary>
		/// Удаляет символ перед курсором.
		/// </summary>
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
					_textArea.MoveLeft();
				}
			}
		}

		/// <summary>
		/// Возвращает текст находящийся в ограниченной области.
		/// </summary>
		public string GetDislplayText()
		{
			int textLength = _text.Length < DisplayLength ? _text.Length : DisplayLength;

			return _text.ToString(_textArea.LeftPos, textLength);
		}

		/// <summary>
		/// Возвращает весь текст.
		/// </summary>
		public string GetAllText()
		{
			return _text.ToString(0, _text.Length - 1);
		}


		/// <summary>
		/// Представляет ограниченную область вывода текста.
		/// </summary>
		private class TextArea
		{
			private int _displayLength;
			private StringBuilder _text;

			/// <summary>
			/// Инициализирует новый экземпляра класса <see cref="TextArea"/>
			/// </summary>
			public TextArea(int displayLength, StringBuilder text)
			{
				_displayLength = displayLength;
				_text = text;
			}

			/// <summary>
			/// Позиция левой границы области.
			/// </summary>
			public int LeftPos { get; private set; }

			/// <summary>
			/// Граница правой граница области.
			/// </summary>
			public int RightPos => LeftPos + _displayLength;

			/// <summary>
			/// Сдвинуть область вправо на один символ.
			/// </summary>
			public void MoveRight()
			{
				if (_text.Length > RightPos)
				{
					LeftPos++;
				}
			}

			/// <summary>
			/// Сдвинуть область влево на один символ.
			/// </summary>
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
