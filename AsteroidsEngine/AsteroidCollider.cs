namespace AsteroidsEngine
{
    public class AsteroidCollider : ICollider
    {
        private readonly GuiVariables _variables;
        private readonly EntityCollection _parent;

        public AsteroidCollider(GuiVariables variables, EntityCollection parent)
        {
            _variables = variables;
            _parent = parent;
        }

        public bool OnCollide(Entity entity1, Entity entity2)
        {
            if (entity2.Tag == Tags.Laser){
                _variables.Score += 1;
                return false;
            }
            
            if (entity2.Tag == Tags.Bullet){
                _variables.Score += 1;
                entity1.Scale /= 2.0f;
                if (entity1.Scale < 0.025f)
                {
                    return false;
                }
                var speed = entity1.Velocity.LengthFast * 1.5f;
                entity1.Velocity = entity2.Velocity.PerpendicularLeft.Normalized() * speed;
                var asteroid = _parent.CreateAsteroid();
                asteroid.Position = entity1.Position;
                asteroid.Scale = entity1.Scale;
                asteroid.Velocity = entity2.Velocity.PerpendicularRight.Normalized() * speed;
                asteroid.Visible = true;
                return true;
            }

            return true;
        }
    }
}