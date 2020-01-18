using ECS.Numerics;

namespace ECS.Drawing
{
	public interface IRendering
	{
		void Render();

		void AddBitmap(int x, int y, Bitmap bitmap);

		void AddBitmap(Vector2 position, Bitmap bitmap);

		void InitializeView(int width, int height);

		void InitializeView(Vector2 position);
	}
}
