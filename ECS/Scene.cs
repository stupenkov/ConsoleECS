using ECS.BasicElemets;
using ECS.Drawing;
using ECS.Input;
using ECS.UI;

namespace ECS
{
	public abstract class Scene
	{
		public Rendering Rendering { get; set; } = new Rendering(70, 30);

		internal World World { get; } = new World();

		protected abstract void Inject(DependencyInjection injection);

		protected abstract void RegisterEntities(World world);

		protected abstract void ConfigureEntities();

		internal void Run()
		{
			World.Injection.RegisterType<IRendering>(Rendering);
			Inject(World.Injection);

			RegisterEntities(World);
			ConfigureEntities();
		}
	}
}
