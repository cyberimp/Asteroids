namespace AsteroidsEngine
{
    public class RenderComponent
    {
        protected readonly int QuadNum;

        protected RenderComponent(int quadNum)
        {
            QuadNum = quadNum;
        }

        public virtual void Render()
        {
            //do nothing
        }
    }
}