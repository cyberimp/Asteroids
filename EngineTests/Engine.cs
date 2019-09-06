using AsteroidsEngine;
using NUnit.Framework;

namespace EngineTests
{
    public class Tests
    {
        private Engine _testedEngine;
        [SetUp]
        public void Setup()
        {
            _testedEngine = new Engine();
            _testedEngine.Run();
        }

        [Test]
        public void Engine()
        {
            Assert.AreEqual(true, true);
        }
    }
}