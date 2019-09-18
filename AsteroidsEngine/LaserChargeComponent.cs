namespace AsteroidsEngine
{
    public class LaserChargeComponent : UpdateComponent
    {
        public override void Update(Entity entity, float delta)
        {
            entity.Visible = ServiceLocator.GetVariables().LaserCharges > entity.Timer;
        }
    }
}