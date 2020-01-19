using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
	[Serializable]
	public class DataDebug
	{
		public Entity Entity { get; set; }
		public Dictionary<string,List<string>> EntityComponents { get; set; }
	}
}
