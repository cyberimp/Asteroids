using AsteroidsEngine;
using NUnit.Framework;
using OpenTK;

namespace EngineTests
{
    public class EntityTest
    {
        private Entity _entity;
        [SetUp]
        public void Setup()
        {
            _entity = new Entity(Vector2.Zero);
        }

        [Test]
        public void EntityDontMoveByDefault()
        {
            var oldPos = _entity.Position;
            _entity.Update(1.0f);
            Assert.AreEqual(oldPos, _entity.Position);
        }
        
        [Test]
        public void EntityMovesWhenSpeedIsSet()
        {
            var oldPos = _entity.Position;
            _entity.Velocity = Vector2.One;
            _entity.Update(1.0f);
            Assert.AreNotEqual(oldPos, _entity.Position);
        }
        
        [Test]
        public void EntityDontMoveWhenInactive()
        {
            var oldPos = _entity.Position;
            _entity.Velocity = Vector2.One;
            _entity.Active = false;
            _entity.Update(1.0f);
            Assert.AreEqual(oldPos, _entity.Position);
        }
    }
}