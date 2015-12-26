using UnityEngine;
using System.Collections.Generic;
using GameCore;

namespace ResourceLoad
{
    public delegate void TextureLoadedCallBack(Texture2D texture);

    public class TextureHandler : Single<TextureHandler>
    {
        private List<string> keys = new List<string>();
        private Dictionary<string, Texture2D> mTextureTable = new Dictionary<string, Texture2D>();

        public void UnLoadRes(string name)
        {
            if (mTextureTable.ContainsKey(name))
            {
                Texture2D.Destroy(mTextureTable[name]);
                mTextureTable[name] = null;
                mTextureTable.Remove(name);
            }
        }

        public Texture2D GetTexture(string name)
        {
            if (mTextureTable.ContainsKey(name))
            {
                return mTextureTable[name];
            }
            return null;
        }

        public void LoadUITexture(string name, TextureLoadedCallBack textureLoaded)
        {
            string directory = "Texture/UI/";
            LoadTexture(directory + name,  textureLoaded);
        }

        public void LoadTexture(string path, TextureLoadedCallBack textureLoaded)
        {
            if (mTextureTable.ContainsKey(path))
            {
                textureLoaded(mTextureTable[path]);
                return;
            }
            Texture2D texture = Resources.Load(path, typeof(Texture2D)) as Texture2D;
            if (texture != null)
            {
                mTextureTable[path] = texture;
                textureLoaded(texture);
                return;
            }

           // Downloader.Instance.LoadTexture(path,textureLoaded);
          
        }

        public void Release()
        {
            keys.Clear();
            foreach (string key in mTextureTable.Keys)
            {
                keys.Add(key);
            }
            foreach (string key in keys)
            {
                Resources.UnloadAsset(mTextureTable[key]);
                mTextureTable[key] = null;
                mTextureTable.Remove(key);
            }
        }
    }

}
