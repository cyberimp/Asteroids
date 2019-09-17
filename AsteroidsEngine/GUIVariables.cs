namespace AsteroidsEngine
{
    public class GuiVariables
    {
        public int Score { get; set; }
        public int LaserCharges { get; set; } = Engine.MaxLaserCharges;
        public bool GameOver { get; set; }
    }
}