namespace AsteroidsEngine
{
    public class UfoAiComponent : UpdateComponent
    {
        private const float UfoVelocity = 0.5f;
        
        private Entity _target;
        private readonly EntityCollection _parent;

        public UfoAiComponent(EntityCollection parent)
        {
            _parent = parent;
        }

        public override void Update(Entity entity, float delta)
        {
            if (_target == null)
                _target = _parent.FindByTag(Tags.Player);
            entity.Velocity = _target.Position - entity.Position;
            entity.Velocity.NormalizeFast();
            entity.Velocity *= UfoVelocity;
        }
    }
}