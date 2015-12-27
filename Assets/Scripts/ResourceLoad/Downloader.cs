﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ResourceLoad
{
    public delegate void LoadedCallBack(Object obj);

    public struct CallBackNode
    {
        public string name;
        public LoadedCallBack cb;

        public CallBackNode(string _name, LoadedCallBack _cb)
        {
            name = _name;
            cb = _cb;
        }
    };

    public class Downloader : Single<Downloader>
    {
        // 已解压的Asset列表 [prefabPath, asset]
        private Dictionary<string, Object> dicAsset = new Dictionary<string, Object>();

        //已解压的回调派发
        private List<CallBackNode> listCB = new List<CallBackNode>();

        // "正在"加载的资源列表 [prefabPath, www]
        private Dictionary<string, WWW> dicLoadingReq = new Dictionary<string, WWW>();

        public string ASSET_URL = Network.GameServer.BASE_URL + "ClientRes/";

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


        public void LoadAsync(string name, System.Type type, LoadedCallBack cb)
        {
            if (dicAsset.ContainsKey(name)) cb(dicAsset[name]);
            else
            {
                listCB.Add(new CallBackNode(name, cb));
                LoadAsync(name,type);
            }
        }

        // name表示prefabPath，eg:Prefab/Pet/ABC
        public void LoadAsync(string name, System.Type type)
        {
            // 如果已经下载，则返回
            if (dicAsset.ContainsKey(name)) return;
            // 如果正在下载，则返回
            if (dicLoadingReq.ContainsKey(name)) return;
            // 如果没下载，则开始下载
            GameEngine.Instance.StartCoroutine(AsyncLoadCoroutine(name, type));
        }


        private IEnumerator AsyncLoadCoroutine(string name, System.Type type)
        {
            string url = ASSET_URL + name + "." + LoadFileType.GetBundlePostfix(type);
            int verNum = 1;

            Debug.Log("WWW AsyncLoad name =" + name + " versionNum = " + verNum);
            if (Caching.IsVersionCached(url, verNum) == false)
                Debug.Log("Version Is not Cached, which will download from net!");

            WWW www = WWW.LoadFromCacheOrDownload(url, verNum);
            dicLoadingReq.Add(name, www);
            while (www.isDone == false)
                yield return null;

            AssetBundleRequest req = www.assetBundle.LoadAsync(GetAssetName(name), type);
            while (req.isDone == false) yield return null;

            dicAsset.Add(name, req.asset);
            dicLoadingReq.Remove(name);
            Dispacher(name, req.asset);
            www.assetBundle.Unload(false);
            www = null;
        }

        //加载完成派发
        private void Dispacher(string name, Object asset)
        {
            foreach (var item in listCB)
            {
                if (item.name.Equals(name))
                {
                    item.cb(asset);
                }
            }
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

        private string GetAssetName(string ResName)
        {
            int index = ResName.LastIndexOf('/');
            return ResName.Substring(index + 1, ResName.Length - index - 1);
        }

        public void UnloadUnusedAsset()
        {
            Resources.UnloadUnusedAssets();
        }

    }


}
