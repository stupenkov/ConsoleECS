using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using ECS.Core;
using ECS.Drawing;
using ECS.Input;
using ECS.Numerics;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(CreationUIGroup))]
	public class TextEditUISystem : ComponentSystem
	{
		public override void OnUpdate()
		{
			Entities.Foreach((Entity entity, TextEditComponent textEdit, TransformComponent transform, PropertiesUIComponent properties) =>
			{
				if (textEdit.InputMode)
				{
					if (textEdit.DisplayText == null)
					{
						textEdit.DisplayText = new DisplayText(textEdit.Length);
					}

					Cursor.Rectangle = new Rectangle(transform.Position.X, transform.Position.Y, textEdit.DisplayText.DisplayLength - 1, 0);

					if (!char.IsWhiteSpace(Input.KeyChar) && !char.IsControl(Input.KeyChar))
					{
						textEdit.DisplayText.InsertChar(Input.KeyChar);
					}
					else if (Input.Key == ConsoleKey.Delete)
					{
						textEdit.DisplayText.RemoveSymbolAfter();
					}
					else if (Input.Key == ConsoleKey.Backspace)
					{
						textEdit.DisplayText.RemoveSymbolBefore();
					}
					else if (Input.Key == ConsoleKey.LeftArrow)
					{
						textEdit.DisplayText.CursorMoveLeft();
					}
					else if (Input.Key == ConsoleKey.RightArrow)
					{
						textEdit.DisplayText.CursorMoveRight();
					}

					Cursor.SetPosition(new Vector2(transform.Position.X + textEdit.DisplayText.CursorDisplayPos, transform.Position.Y));
				}
				else
				{
					Cursor.ResetRectangel();
				}

				Bitmap textBitmap = Bitmap.CreateFromText(textEdit.DisplayText.GetDislplayText());
				Bitmap bitmap = new Bitmap(textEdit.Length, 1);
				bitmap.AddBitmap(0, 0, textBitmap);
				bitmap.FillBackgroundColor(properties.Colors.Background);

				entity.AddComponent(new SpriteComponent { Bitmap = bitmap });
			});
		}
	}
}
