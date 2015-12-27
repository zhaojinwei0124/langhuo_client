using UnityEngine;
using System.Collections.Generic;
using GameCore;

namespace ResourceLoad
{
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

        public void LoadUITexture(string name, LoadedCallBack loadedCB)
        {
            string directory = "Texture/UI/";
            LoadTexture(directory + name, loadedCB);
        }

        /// <summary>
        /// Load Asset check path local first and remote then
        /// </summary>
        public void LoadTexture(string path, LoadedCallBack loadedCB)
        {
            if (mTextureTable.ContainsKey(path))
            {
                loadedCB(mTextureTable[path]);
                return;
            }
            Texture2D texture = Resources.Load(path, typeof(Texture2D)) as Texture2D;
            if (texture != null)
            {
                mTextureTable[path] = texture;
                loadedCB(texture);
                return;
            }
            Downloader.Instance.LoadAsyncTexture(path,loadedCB);
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
