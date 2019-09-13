using System;
using OpenTK;

namespace AsteroidsEngine
{
    public class PlayerComponent:EntityComponent
    {
        private float _bulletCd;
        private float _laserCd;
        private int _laser = 2;
        private bool _laserVisible;
        public override void Render(Entity entity)
        {
            if (!_laserVisible) return;
            var pos = entity.Position;
            var rot = MathHelper.DegreesToRadians(entity.Rotation);
            var matrix = Matrix4.CreateScale(0.01f) *
                         Matrix4.CreateRotationZ(rot);
            var diff = Vector2.Transform(Vector2.UnitY*0.02f,
                Quaternion.FromAxisAngle(Vector3.UnitZ, rot));

            pos += diff * 5;
            while (Vector2.Clamp(pos,-Vector2.One, Vector2.One) == pos)
            {
                ServiceLocator.GetShader().
                    SetMatrix4("transform",matrix*
                                           Matrix4.CreateTranslation(
                                               pos.X,pos.Y,0.0f));
                pos += diff;
                ServiceLocator.GetTexture().RenderQuad(11);
            }


        }

        public override void Update(Entity entity, float delta)
        {
            if(_bulletCd > 0.0f)
                _bulletCd -= delta;
            
            var input = ServiceLocator.GetController();

            entity.Rotation -= input.Rotation * 270f * delta;
            if (input.Thrust)
                entity.Velocity+= Vector2.Transform(Vector2.UnitY * .01f,
                    Quaternion.FromEulerAngles(Vector3.UnitZ*
                                               MathHelper.DegreesToRadians(entity.Rotation)));
            
            if (input.Fire1 && _bulletCd < 0.001f)
            {
                ServiceLocator.GetEngine().CreateBullet(entity);
                _bulletCd = 0.1f;
                entity.Velocity-= Vector2.Transform(Vector2.UnitY * .005f,
                    Quaternion.FromEulerAngles(Vector3.UnitZ*
                                               MathHelper.DegreesToRadians(entity.Rotation)));
            }

            _laserVisible = input.Fire2;
            
            if (entity.Velocity.LengthSquared > 0.25f)
                entity.Velocity = Vector2.Normalize(entity.Velocity) * 0.5f;

        }
    }
}