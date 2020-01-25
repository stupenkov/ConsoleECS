using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
	[UpdateInGroup(typeof(RootSystemGroup))]
	[UpdateAfter(typeof(RenderingSystemGroup))]
	public class InputSystemGroup : ComponentSystemGroup
	{
	}
}
