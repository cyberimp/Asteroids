namespace AsteroidsEngine
{
    public struct EngineSettings
    {
        private int XRes { get; set; }
        private int YRes { get; set; }

        public EngineSettings(int xRes, int yRes)
        {
            XRes = xRes;
            YRes = yRes;
        }
    }
}