using System;
using OpenTK.Input;

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

            entity.Rotation -= input.Rotation* 45f * delta;
        }
    }
}