using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Textures
{
    public class GenAssetManager<T>
    {
        private Dictionary<string, T> assets = new Dictionary<string, T>();
        private ContentManager _content;
        private string missingAssetName = "_no_tex_found";

        

        public void Initialize(ContentManager content, string missingAsset_fileName)
        {
            _content = content;
            LoadTexture(missingAssetName, missingAsset_fileName);
        }

        public void LoadTexture(string assetName, string fileName)
        {
            if (_content == null) return;

            StoreTexture(assetName, _content.Load<T>(fileName));
        }

        public void StoreTexture(string textureName, T asset)
        {
            if (assets.ContainsKey(textureName))
            {
                assets[textureName] = asset;
                return;
            }

            assets.Add(textureName, asset);
        }

        public T GetAsset(string assetName)
        {
            if (!assets.ContainsKey(assetName))
            {
                return assets[missingAssetName];
            }

            return assets[assetName];
        }
    }
}
