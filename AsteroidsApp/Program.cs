namespace AsteroidsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new PolyEngine();
            engine.Run(60);
        }
    }
}