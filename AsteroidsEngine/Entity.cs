using System.Collections.Generic;
using OpenTK;

namespace AsteroidsEngine
{
    public sealed class Entity
    {
        private readonly Shader _shader;
        private readonly LinkedList<UpdateComponent> _updateComponents;
        private ICollider _collider;
        private RenderComponent _render;

        #region transform matrix fields

        private Matrix4 _transMatrix;
        private Vector2 _position = Vector2.Zero;
        private float _scale = 0.5f;
        private float _rotation;
        private bool _dirtyTrans = true;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _dirtyTrans = true;
            }
        }

        public float Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _dirtyTrans = true;
            }
        }

        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _dirtyTrans = true;
            }
        }

        #endregion

        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public bool Active { get; set; } = true;
        public float Timer { get; set; }
        public Tags Tag { get; set; }
        public bool Visible { get; set; } = true;
        public int ComponentsCount => _updateComponents.Count;

        public Entity(Shader shader)
        {
            _updateComponents = new LinkedList<UpdateComponent>();
            _shader = shader;
        }

        private void UpdateMatrix()
        {
            _transMatrix = Matrix4.CreateScale(Scale) *
                           Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation)) *
                           Matrix4.CreateTranslation(Position.X, Position.Y, 0.0f);
        }

        public bool Update(float delta)
        {
            if (!Active) return true;
            Position += Velocity * delta;
            var alive = true;
            foreach (var component in _updateComponents)
                if (!component.Update(this, delta))
                    alive = false;
            return alive;
        }

        public void Render()
        {
            if (!Active || !Visible || _render == null) return;
            if (_dirtyTrans)
            {
                UpdateMatrix();
                _dirtyTrans = false;
            }

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

        public bool Collide(Entity entity2)
        {
            return _collider == null || _collider.OnCollide(this, entity2);
        }
    }
}