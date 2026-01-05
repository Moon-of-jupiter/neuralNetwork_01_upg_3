using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Textures
{
    public static class TextureManager
    {
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static ContentManager _content;
        private static string missingTexName = "_no_tex_found";
        public static void Initialize(ContentManager content, string missingTex_fileName)
        {
            _content = content;
            LoadTexture(missingTexName, missingTex_fileName);
        }

        public static void LoadTexture(string textureName, string fileName)
        {
            if(_content == null) return;

            StoreTexture(textureName,_content.Load<Texture2D>(fileName));
        }

        public static void StoreTexture(string textureName, Texture2D texture)
        {
            if (textures.ContainsKey(textureName))
            {
                textures[textureName] = texture;
                return;
            }

            textures.Add(textureName, texture);
        }

        public static Texture2D GetTexture(string textureName)
        {
            if (!textures.ContainsKey(textureName))
            {
                return textures[missingTexName];
            }

            return textures[textureName];
        }


    }
}
