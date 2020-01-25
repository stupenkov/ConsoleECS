namespace ECS
{
	public interface IComponentSystem
	{
		bool Enable { get; set; }

		internal void Start();

		internal void Execute();
	}
}
