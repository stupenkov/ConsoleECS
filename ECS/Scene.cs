using ECS.Drawing;

namespace ECS
{
	public abstract class Scene
	{
		public Scene()
		{
			EntityManager = World.EntityManager;
		}

		public Rendering Rendering { get; set; } = new Rendering(70, 30);

		protected EntityManager EntityManager { get; }

		internal World World { get; } = new World();

		protected abstract void Inject(DependencyInjection injection);

		protected abstract void RegisterEntities(World world);

		protected abstract void ConfigureEntities();

		internal void Run()
		{
			World.Injection.RegisterType<Rendering>(Rendering);
			Inject(World.Injection);

			RegisterEntities(World);
			ConfigureEntities();
		}
	}
}
