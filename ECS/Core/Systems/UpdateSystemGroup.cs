using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
	[UpdateInGroup(typeof(RootSystemGroup))]
	public class UpdateSystemGroup : ComponentSystemGroup
	{
	}
}
