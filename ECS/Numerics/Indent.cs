namespace ECS.Numerics
{
	public struct Indent
	{
		public int Top;
		public int Right;
		public int Bottom;
		public int Left;

		public Indent(int topBottom, int leftRight)
		{
			Top = topBottom;
			Bottom = topBottom;
			Left = leftRight;
			Right = leftRight;
		}
	}
}
