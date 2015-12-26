﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ResourceLoad
{
    public class Downloader 
    {
        // 已解压的Asset列表 [prefabPath, asset]
        private Dictionary<string, Object> dicAsset = new Dictionary<string, Object>();
        // "正在"加载的资源列表 [prefabPath, www]
        private Dictionary<string, WWW> dicLoadingReq = new Dictionary<string, WWW>();

        public string ASSET_URL = Network.GameServer.BASE_URL+"ClientRes/";

        public Object GetResource(string name)
        {
            Object obj = null;
            if (dicAsset.TryGetValue(name, out obj) == false)
            {
                Debug.LogWarning("<GetResource Failed> Res not exist, res.Name = " + name);
                if (dicLoadingReq.ContainsKey(name))
                {
                    Debug.LogWarning("<GetResource Failed> The res is still loading");
                }
            }
            return obj;
        }
      
        // name表示prefabPath，eg:Prefab/Pet/ABC
        public void LoadAsync(string name)
        {
            LoadAsync(name, typeof(Object));
        }


        // name表示prefabPath，eg:Prefab/Pet/ABC
        public void LoadAsync(string name, System.Type type)
        {
            // 如果已经下载，则返回
            if (dicAsset.ContainsKey(name))return;
            // 如果正在下载，则返回
            if (dicLoadingReq.ContainsKey(name))return;
            // 添加引用
            RefAsset(name);
            // 如果没下载，则开始下载
            GameEngine.Instance.StartCoroutine(AsyncLoadCoroutine(name, type));
        }


        private IEnumerator AsyncLoadCoroutine(string name, System.Type type)
        {
            string assetBundleName = name;
            string url = ASSET_URL+name;
            int verNum = 1;
            
            Debug.Log("WWW AsyncLoad name =" + assetBundleName + " versionNum = " + verNum);
            if (Caching.IsVersionCached(url, verNum) == false)
                Debug.Log("Version Is not Cached, which will download from net!");
            
            WWW www = WWW.LoadFromCacheOrDownload(url,verNum);
            dicLoadingReq.Add(name, www);
            while (www.isDone == false)
                yield return null;
            
            AssetBundleRequest req = www.assetBundle.LoadAsync(GetAssetName(name), type);
            while (req.isDone == false)
                yield return null;
            
            dicAsset.Add(name, req.asset);
            dicLoadingReq.Remove(name);
            www.assetBundle.Unload(false);
            www = null;
        }
        
        public bool IsResLoading(string name)
        {
            return dicLoadingReq.ContainsKey(name);
        }
        
        public bool IsResLoaded(string name)
        {
            return dicAsset.ContainsKey(name);
        }
        
        public WWW GetLoadingWWW(string name)
        {
            WWW www = null;
            dicLoadingReq.TryGetValue(name, out www);
            return www;
        }

        // 移除Asset资源的引用，name表示prefabPath
        public void UnrefAsset(string name)
        {
            dicAsset.Remove(name);
        }
        
        private string GetAssetName(string ResName)
        {
            int index = ResName.LastIndexOf('/');
            return ResName.Substring(index + 1, ResName.Length - index - 1);
        }
        
        public void UnloadUnusedAsset()
        {
             Resources.UnloadUnusedAssets();
        }

        // 根据资源路径添加资源引用，每个管理器管理自己的引用
        private void RefAsset(string name)
        {
        }


    }
}
