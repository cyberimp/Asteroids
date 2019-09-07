using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AsteroidsEngine
{
    public class Entity
    {
        private LinkedList<EntityComponent> _components;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Scale { get; set; }
        public bool Active { get; set; }

        public Entity(Vector2 position)
        {
            _components = new LinkedList<EntityComponent>();
            Active = true;
            Position = position;
            Velocity = Vector2.Zero;
            Scale = 0.5f;

        }

        public virtual void Update(float delta)
        {
            if (!Active) return;
            Position += Velocity * delta;
            foreach (var component in _components)
            {
                component.Update(this, delta);
            }
        }

        public virtual void Render()
        {
            if (!Active) return;
            GL.LoadIdentity();
            GL.Translate(new Vector3(Position));
            GL.Scale(Vector3.One*Scale);
            foreach (var component in _components)
            {
                component.Render(this);
            }
        }
    }
}