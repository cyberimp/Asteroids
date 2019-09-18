namespace AsteroidsApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var engine = new PolyEngine();
            engine.Run(60);
        }
    }
}