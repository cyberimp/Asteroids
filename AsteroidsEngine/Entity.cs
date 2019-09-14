using System.Collections.Generic;
using OpenTK;

namespace AsteroidsEngine
{
    public class Entity
    {
        private LinkedList<UpdateComponent> _updateComponents;
        private RenderComponent _render;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Scale { get; set; }
        
        public float Rotation { get; set; }
        public bool Active { get; set; }
        
        public float Timer { get; set; }
        
        public string Tag { get; set; }
        public int ComponentsCount => _updateComponents.Count;

        private ICollider _collider;

        private Matrix4 _transMatrix;
        

        public Entity(Vector2 position)
        {
            _updateComponents = new LinkedList<UpdateComponent>();
            Active = true;
            Position = position;
            Velocity = Vector2.Zero;
            Scale = 0.5f;

        }

        private void UpdateMatrix()
        {
            _transMatrix = Matrix4.CreateScale(Scale) * 
                           Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation)) *
                           Matrix4.CreateTranslation(Position.X, Position.Y, 0.0f);
        }

        public virtual void Update(float delta)
        {
            if (!Active) return;
            Position += Velocity * delta;
            foreach (var component in _updateComponents)
            {
                component.Update(this, delta);
            }
            UpdateMatrix();
        }

        public virtual void Render()
        {
            if (!Active) return;
            ServiceLocator.GetShader().SetMatrix4("transform", _transMatrix);
            _render.Render(this);
        }

        public void AddComponent(UpdateComponent component)
        {
            _updateComponents.AddLast(component);
        }

        public void SetRender(RenderComponent component)
        {
            _render = component;
        }

        public void SetCollider(ICollider collider)
        {
            _collider = collider;
        }

        public void Collide(Entity entity2)
        {
            _collider?.OnCollide(this, entity2);
        }
    }
}