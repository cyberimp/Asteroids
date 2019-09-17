using System;
using AsteroidsEngine;

namespace AsteroidsApp
{
    public class ResourceLocator
    {
        private static Texture _texture;
        
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

    }
}