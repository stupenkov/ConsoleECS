using System;

namespace ECS
{
	/// <summary>
	/// Указывает что система не должна автоматически создаваться.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DisableCreationAttribute : Attribute
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="DisableCreationAttribute"/>
		/// </summary>
		/// <param name="systemType">Тип системы.</param>
		public DisableCreationAttribute(Type systemType)
		{
			SystemType = systemType;
		}

		public Type SystemType { get; }
	}
}
