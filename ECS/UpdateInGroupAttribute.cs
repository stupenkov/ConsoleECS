using System;

namespace ECS
{
	/// <summary>
	/// Указывает к какой группе систем относится данная система.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class UpdateInGroupAttribute : Attribute
	{
		public UpdateInGroupAttribute(Type groupType)
		{
			GroupType = groupType;
		}

		public Type GroupType { get; }
	}
}
