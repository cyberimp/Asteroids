using OpenTK;

namespace AsteroidsEngine
{
    public class HyperDriveComponent:UpdateComponent
    {
        public override void Update(Entity entity, float delta)
        {
            var position = entity.Position;
            if (position.X > 1 || position.X < -1)
                position.X = -position.X;
            if (position.Y > 1 || position.Y < -1)
                position.Y = -position.Y;
            entity.Position = position;
        }
    }
}