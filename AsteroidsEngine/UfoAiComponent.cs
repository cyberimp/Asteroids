namespace AsteroidsEngine
{
    public class UfoAiComponent: UpdateComponent
    {
        private Entity _target;

        public override void Update(Entity entity, float delta)
        {
            if (_target == null)
                _target = ServiceLocator.GetEntities().FindByTag("player"); 
            entity.Velocity = _target.Position - entity.Position;
            entity.Velocity.NormalizeFast();
            entity.Velocity *= 0.2f;
        }
    }
}