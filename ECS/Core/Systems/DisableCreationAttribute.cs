using System;

namespace ECS
{
	/// <summary>
	/// Указывает что система не должна автоматически создаваться.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DisableCreationAttribute : Attribute
	{
	}
}
