using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
	/// <summary>
	/// Указывает перед какой системой должна создаваться данная система.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class UpdateBeforeAttribute : Attribute
	{
		public UpdateBeforeAttribute(Type systemType)
		{
			SystemType = systemType;
		}

		public Type SystemType { get; }
	}
}
