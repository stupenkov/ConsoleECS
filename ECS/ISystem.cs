namespace ECS
{
	public interface ISystem
	{
		bool Enable { get; set; }

		internal void Execute();
	}
}
