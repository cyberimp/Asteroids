using System;
using OpenTK;

namespace AsteroidsEngine
{
    public class PlayerComponent:EntityComponent
    {
        public override void Render(Entity entity)
        {
            //do not render anything
        }

        public override void Update(Entity entity, float delta)
        {
            var input = ServiceLocator.GetController();

            entity.Rotation -= input.Rotation * 270f * delta;
            if (input.Thrust)
                entity.Velocity+= Vector2.Transform(Vector2.UnitY * .01f,
                    Quaternion.FromEulerAngles(Vector3.UnitZ*
                                               MathHelper.DegreesToRadians(entity.Rotation)));
            
            if (entity.Velocity.LengthSquared > 0.25f)
                entity.Velocity = Vector2.Normalize(entity.Velocity) * 0.5f;
            
            if(input.Fire1)
                Console.WriteLine(entity.Position);
        }
    }
}