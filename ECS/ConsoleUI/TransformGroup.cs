using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.ConsoleUI
{
	[UpdateInGroup(typeof(RenderingSystemGroup))]
	[UpdateAfter(typeof(CreationUIGroup))]
	[UpdateBefore(typeof(RenderingSystem))]
	public class TransformGroup : ComponentSystemGroup
	{
	}
}
