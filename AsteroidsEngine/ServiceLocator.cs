using System;

namespace AsteroidsEngine
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;

        private static Shader _shader;
        private static Texture _texture;
        private static Engine _engine;
        private static Controller _controller;
        
        private ServiceLocator(){}
        
        public static ServiceLocator GetInstance()
        { 
            return _instance ?? (_instance = new ServiceLocator());
        }

        public static void SetShader(Shader shader)
        {
            GetInstance();
            _shader = shader;
        }

        public static Shader GetShader()
        {
            GetInstance();
            return _shader ?? 
                   throw new Exception("Shader not initialized", 
                       new NullReferenceException());
        }
        
        public static void SetTexture(Texture texture)
        {
            GetInstance();
            _texture = texture;
        }

        public static Texture GetTexture()
        {
            GetInstance();
            return _texture ?? 
                   throw new Exception("Texture not initialized", 
                       new NullReferenceException());
        }
        
        public static void SetEngine(Engine engine)
        {
            GetInstance();
            _engine = engine;
        }

        public static Engine GetEngine()
        {
            GetInstance();
            return _engine ?? 
                   throw new Exception("Engine not initialized", 
                       new NullReferenceException());
        }
        public static Controller GetController()
        {
            GetInstance();
            return _controller ?? (_controller = new Controller());
        }
    }
}