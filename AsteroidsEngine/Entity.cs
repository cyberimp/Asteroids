using System.Collections.Generic;
using OpenTK;

namespace AsteroidsEngine
{
    public sealed class Entity
    {
        private readonly LinkedList<UpdateComponent> _updateComponents;
        private ICollider _collider;
        private RenderComponent _render;
        private Matrix4 _transMatrix;
        private Vector2 _position;
        private float _scale;
        private float _rotation;
        private readonly Shader _shader;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateMatrix();
            }
        }

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public float Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                UpdateMatrix();
            }
        }

        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateMatrix();
            }
        }

        public bool Active { get; set; } = true;
        public float Timer { get; set; }
        public Tags Tag { get; set; }
        public bool Visible { get; set; } = true;
        public int ComponentsCount => _updateComponents.Count;

        public Entity(Shader shader)
        {
            _updateComponents = new LinkedList<UpdateComponent>();
            Position = Vector2.Zero;
            Scale = 0.5f;
            _shader = shader;
        }

        private void UpdateMatrix()
        {
            _transMatrix = Matrix4.CreateScale(Scale) *
                           Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation)) *
                           Matrix4.CreateTranslation(Position.X, Position.Y, 0.0f);
        }

        public void Update(float delta)
        {
            if (!Active) return;
            Position += Velocity * delta;
            foreach (var component in _updateComponents) component.Update(this, delta);
        }

        public void Render()
        {
            if (!Active || !Visible || _render == null) return;
            _shader.SetMatrix4("transform", _transMatrix);
            _render.Render();
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