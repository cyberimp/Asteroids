using System;

namespace AsteroidsEngine
{
    public static class ServiceLocator
    {
        private static Shader _shader;
        private static Texture _texture;
        private static Engine _engine;
        private static Controller _controller;
        private static EntityCollection _entities;
        private static GuiVariables _variables;


        public static void SetShader(Shader shader)
        {
            _shader = shader;
        }

        public static Shader GetShader()
        {
            return _shader ?? 
                   throw new Exception("Shader not initialized", 
                       new NullReferenceException());
        }
        
        public static void SetTexture(Texture texture)
        {
            _texture = texture;
        }

        public static Texture GetTexture()
        {
            return _texture ?? 
                   throw new Exception("Texture not initialized", 
                       new NullReferenceException());
        }
        
        public static void SetEngine(Engine engine)
        {
            _engine = engine;
        }

        public static Engine GetEngine()
        {
            return _engine ?? 
                   throw new Exception("Engine not initialized", 
                       new NullReferenceException());
        }
        public static Controller GetController()
        {
            return _controller ?? (_controller = new Controller());
        }

        public static void SetEntities(EntityCollection entities)
        {
            _entities = entities;
        }

        public static EntityCollection GetEntities()
        {
            return _entities ?? 
                   throw new Exception("Entities collection not initialized", 
                       new NullReferenceException());
            
        }

        public static GuiVariables GetVariables()
        {
            return _variables ?? (_variables = new GuiVariables());
        }
    }
}