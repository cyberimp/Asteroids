using System;

namespace AsteroidsApp
{
    public class ResourceLocator
    {
        private static Texture _texture;
        private static Model _model;

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

        public static void SetModel(Model model)
        {
            _model = model;
        }

        public static Model GetModel()
        {
            return _model ?? 
                   throw new Exception("Model not initialized", 
                       new NullReferenceException());
        }
    }
}