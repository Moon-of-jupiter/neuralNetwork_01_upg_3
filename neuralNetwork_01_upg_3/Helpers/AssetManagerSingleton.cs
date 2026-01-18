using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Textures
{
    public static class AssetManagerSingleton
    {
        public static GenAssetManager<Texture2D> TextureManager { get; private set; } = new GenAssetManager<Texture2D>();
        public static GenAssetManager<SpriteFont> FontManager { get; private set; } = new GenAssetManager<SpriteFont>();


        


    }
}
