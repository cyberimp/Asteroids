using OpenTK;

namespace AsteroidsEngine
{
    public class Entity
    {
        private Vector2 Position { get; set; }
        private Vector2 Velocity { get; set; }

        public Entity(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
        }

        public virtual void Update(float delta)
        {
            Position += Velocity * delta;
        }

        public virtual void Render()
        {
        }
    }
}