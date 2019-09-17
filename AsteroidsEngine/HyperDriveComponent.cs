namespace AsteroidsEngine
{
    public class HyperDriveComponent:UpdateComponent
    {
        public override void Update(Entity entity, float delta)
        {
            var position = entity.Position;
            if (position.X > 1 || position.X < -1)
                position.X = -position.X *0.9f;
            if (position.Y > 1 || position.Y < -1)
                position.Y = -position.Y* 0.9f;
            entity.Position = position;
        }
    }
}