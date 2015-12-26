using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using System.Linq;
using System.IO;

public delegate void LoadFinishCb();
public delegate void LoadGoFinishCb(GameObject go);

namespace ResourceLoad
{
	public delegate void ResLoadedCallBack (Object res);

	public class ResourceHandler:Single<ResourceHandler>
	{
		private List<string> keys = new List<string>();
		private Dictionary<string, Object> mResourceTable = new Dictionary<string, Object> ();
		private Dictionary<string,  List<ResLoadedCallBack>> mCallBackTable = new Dictionary<string,  List<ResLoadedCallBack>> ();
		
		public bool CheckResLoaded (string name)
		{
			if (mResourceTable.ContainsKey (name)) {
				return true;
			}
			return false;
		}

		public Object GetRes (string name)
		{
			if (mResourceTable.ContainsKey (name)) {
				return mResourceTable [name];
			}
			return null;
		}
		
		public void UnLoadRes(string name)
		{
			if(mResourceTable.ContainsKey(name))
			{
				if(!(mResourceTable[name] is UnityEngine.GameObject))
				{
#if !UNITY_WINRT
					Resources.UnloadAsset(mResourceTable[name]);
#endif
				}
				mResourceTable[name] = null;
				mResourceTable.Remove(name);
			}
		}
		
		
		public void LoadRes (string path, ResLoadedCallBack resLoaded)
		{
			LoadRes (path, resLoaded, false ,true);
		}

		public void LoadRes (string path, ResLoadedCallBack resLoaded,bool noticeError)
		{
			LoadRes (path, resLoaded, noticeError, true);
		}

        public void LoadRes(string path, ResLoadedCallBack resLoaded, bool noticeError, bool compress)
        {
            if (path == null) return;
            if (mResourceTable.ContainsKey(path))
            {
                resLoaded(mResourceTable[path]);
                return;
            }
            Object prefab = Resources.Load(path, typeof(GameObject));
            if (prefab != null)
            {
                mResourceTable[path] = prefab;
                resLoaded(prefab);
            }
        }

        public void LoadResAsync(string path, ResLoadedCallBack resLoaded)
        {
            GameEngine.Instance.StartCoroutine(DoLoadResAsync(path, resLoaded));
        }

        private IEnumerator DoLoadResAsync(string path, ResLoadedCallBack resLoaded)
        {
            if (path == null)
            {
            }
            else
            {
                if (mResourceTable.ContainsKey(path))
                {
                    resLoaded(mResourceTable[path]);
                }
                else
                {
                    ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(path);
                    yield return null;
                    while (!resourceRequest.isDone)
                    {
                        yield return null;
                    }
                    GameObject prefab = resourceRequest.asset as GameObject;
                    if (prefab != null)
                    {
                        mResourceTable[path] = prefab;
                        resLoaded(prefab);
                    }
                }
            }
        }

		public void Release()
		{
			keys.Clear();
			foreach(string key in mResourceTable.Keys)
			{
				keys.Add(key);
			}
			foreach(string key in keys)
			{
				mResourceTable[key] = null;
				mResourceTable.Remove(key);
			}
		}

		
		public void Clear()
		{
			foreach(List<ResLoadedCallBack> callbacks in mCallBackTable.Values)
			{
				callbacks.Clear();
			}
		}

		public bool IsFinished()
		{
			return mCallBackTable.Count == 0;
		}
	}


}
