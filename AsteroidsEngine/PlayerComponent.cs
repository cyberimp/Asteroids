using OpenTK;

namespace AsteroidsEngine
{
    public class PlayerComponent : UpdateComponent
    {
        private const float LaserChargeTime = 5.0f;
        private float _bulletCd;
        private float _laserCd;
        private readonly GuiVariables _variables;
        private readonly Controller _controller;
        private readonly EntityCollection _parent;

        public PlayerComponent(GuiVariables variables, Controller controller,
            EntityCollection parent)
        {
            _variables = variables;
            _controller = controller;
            _parent = parent;
        }

        public override bool Update(Entity entity, float delta)
        {
            if (_bulletCd > 0.0f)
                _bulletCd -= delta;
            if (_laserCd > 0.0f)
                _laserCd -= delta;

            if (entity.Timer > 0.0f && _variables.LaserCharges < Engine.MaxLaserCharges)
                entity.Timer -= delta;

            if (entity.Timer <= 0.0f && _variables.LaserCharges < Engine.MaxLaserCharges)
            {
                _variables.LaserCharges++;
                entity.Timer = LaserChargeTime;
            }

            entity.Rotation -= _controller.Rotation * 270f * delta;
            if (_controller.Thrust)
                entity.Velocity += Vector2.Transform(Vector2.UnitY * .01f,
                    Quaternion.FromEulerAngles(Vector3.UnitZ *
                                               MathHelper.DegreesToRadians(entity.Rotation)));

            if (_controller.Fire1 && _bulletCd < 0.001f)
            {
                _parent.CreateBullet(entity);
                _bulletCd = 0.1f;
                entity.Velocity -= Vector2.Transform(Vector2.UnitY * .005f,
                    Quaternion.FromEulerAngles(Vector3.UnitZ *
                                               MathHelper.DegreesToRadians(entity.Rotation)));
            }

            if (_controller.Fire2 && _laserCd < 0.001f && _variables.LaserCharges > 0)
            {
                _parent.CreateLaser(entity);
                _laserCd = 2f;
                _variables.LaserCharges--;
                entity.Timer = LaserChargeTime;
            }

            if (entity.Velocity.LengthSquared > 0.25f)
                entity.Velocity = Vector2.Normalize(entity.Velocity) * 0.5f;
            return true;
        }
    }
}