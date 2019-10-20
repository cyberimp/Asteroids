namespace AsteroidsApp
{
    internal static class Program
    {
        private static void Main()
        {
            var engine = new PolyEngine();
            engine.Run(60);
        }
    }
}