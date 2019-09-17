using OpenTK;

namespace AsteroidsEngine
{
    public class PlayerComponent:UpdateComponent
    {
        private float _bulletCd;
        private float _laserCd;

        private const float LaserChargeTime = 5.0f;

        public override void Update(Entity entity, float delta)
        {
            var vars = ServiceLocator.GetVariables();
            if(_bulletCd > 0.0f)
                _bulletCd -= delta;
            if(_laserCd > 0.0f)
                _laserCd -= delta;

            if (entity.Timer > 0.0f && vars.LaserCharges < Engine.MaxLaserCharges)
                entity.Timer -= delta;

            if (entity.Timer <= 0.0f && vars.LaserCharges < Engine.MaxLaserCharges)
            {
                vars.LaserCharges++;
                entity.Timer = LaserChargeTime;
            }
                

            
            var input = ServiceLocator.GetController();

            entity.Rotation -= input.Rotation * 270f * delta;
            if (input.Thrust)
                entity.Velocity+= Vector2.Transform(Vector2.UnitY * .01f,
                    Quaternion.FromEulerAngles(Vector3.UnitZ*
                                               MathHelper.DegreesToRadians(entity.Rotation)));
            
            if (input.Fire1 && _bulletCd < 0.001f)
            {
                ServiceLocator.GetEntities().CreateBullet(entity);
                _bulletCd = 0.1f;
                entity.Velocity-= Vector2.Transform(Vector2.UnitY * .005f,
                    Quaternion.FromEulerAngles(Vector3.UnitZ*
                                               MathHelper.DegreesToRadians(entity.Rotation)));
            }

            if (input.Fire2 && _laserCd < 0.001f && vars.LaserCharges > 0)
            {
                ServiceLocator.GetEntities().CreateLaser(entity);
                _laserCd = 2f;
                vars.LaserCharges--;
                entity.Timer = LaserChargeTime;
            }
//            _laserVisible = input.Fire2;
            
            if (entity.Velocity.LengthSquared > 0.25f)
                entity.Velocity = Vector2.Normalize(entity.Velocity) * 0.5f;

        }
    }
}