namespace ECS.Numerics
{
	public struct Vector3
	{
		public int X;
		public int Y;
		public int Z;

		public Vector3(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector2 ToVector2()
		{
			return new Vector2(X, Y);
		}
	}
}
