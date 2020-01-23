using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
	/// <summary>
	/// Указывает после какой системы должна создаваться данная система.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class UpdateAfterAttribute : Attribute
	{
		public UpdateAfterAttribute(Type systemType)
		{
			SystemType = systemType;
		}

		public Type SystemType { get; }
	}
}
