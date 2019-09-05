namespace AsteroidsEngine
{
    public class Entity
    {
        private float _x = 0;
        private float _y = 0;
        private float _vx = 0;
        private float _vy = 0;

        public virtual void Update(uint delta)
        {
            _x += _vx * delta;
            _y += _vy * delta;
        }

        public virtual void Render()
        {
        }
    }
}