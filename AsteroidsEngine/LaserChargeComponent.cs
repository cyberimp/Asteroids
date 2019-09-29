namespace AsteroidsEngine
{
    public class LaserChargeComponent : UpdateComponent
    {
        private readonly GuiVariables _variables;
        public LaserChargeComponent(GuiVariables variables)
        {
            _variables = variables;
        }
        public override void Update(Entity entity, float delta)
        {
            entity.Visible = _variables.LaserCharges > entity.Timer;
        }
    }
}