namespace AsteroidsEngine
{
    public class AsteroidCollider : ICollider
    {
        public void OnCollide(Entity entity1, Entity entity2)
        {
            switch (entity2.Tag)
            {
                case "laser":
                    entity1.Active = false;
                    ServiceLocator.GetVariables().Score += 1;
                    break;
                case "bullet":
                    ServiceLocator.GetVariables().Score += 1;
                    entity1.Scale /= 2.0f;
                    if (entity1.Scale < 0.025f)
                    {
                        entity1.Active = false;
                    }
                    else
                    {
                        var speed = entity1.Velocity.LengthFast * 1.5f;
                        entity1.Velocity = entity2.Velocity.PerpendicularLeft.Normalized() * speed;
                        var asteroid = ServiceLocator.GetEntities().CreateAsteroid();
                        asteroid.Position = entity1.Position;
                        asteroid.Scale = entity1.Scale;
                        asteroid.Velocity = entity2.Velocity.PerpendicularRight.Normalized() * speed;
                        asteroid.Visible = true;
                    }

                    break;
            }
        }
    }
}