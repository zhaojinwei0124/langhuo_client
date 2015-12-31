using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResourceLoad;

namespace GameCore
{
    public class UILayer
    {
        public const int PageDepth = 1000;
        public const int ControlDepth = 2000;
        public const int DialogDepth = 11000;
        public const int TooltipDepth = 4000;
        public const int FlowerDepth = 12000;
        public const int DepthOffset = 20;
    }

    public class PageInfo
    {
        public string name;
        public string arg;
        public bool forward;
        public GameObjectCallBack callBack;
        public int lifeTimes;
        public bool inStack;
        public System.Action onPageClosed;
        public View view;
        public int depth;
        public GameObject go;
        public object param;
    }

    public delegate void GameObjectCallBack(GameObject go);

    public class UIHandler : Single<UIHandler>
    {
        public static int LIFE_TIMES = 3;
        private Transform mMainPanel = null;
        private Camera mMainCamera = null;
        private Dictionary<string, PageInfo> PageSlots = new Dictionary<string, PageInfo>();
        private Stack<PageInfo> PageStack = new Stack<PageInfo>();
        private Font mFont;
        private static UIHandler mInstance;
        private UIRoot uiRoot;

        public UIRoot Root
        {
            get
            {
                if (uiRoot == null)
                {
                    uiRoot = GameObject.FindObjectOfType<UIRoot>();
                }
                return uiRoot;

            }
        }

        public UIHandler()
        {
            mFont = Resources.Load<Font>("Font/msyh");
        }

        public Font DefaultFont { get { return mFont; } }

        public Transform MainPanel
        {
            get
            {
                if (mMainPanel == null)
                {
                    mMainPanel = GameObject.Find("MainPanel").transform;
                }
                return mMainPanel;
            }
        }

        public PageInfo GetPageInfo(string name)
        {
            if (PageSlots.ContainsKey(name))
            {
                return PageSlots [name];
            } else
            {
                return null;
            }
        }

        public void AddPageInfo(string name, PageInfo info)
        {
            if (PageSlots.ContainsKey(name))
            {
                PageSlots [name] = info;
            } else
            {
                PageSlots.Add(name, info);
            }
        }

        public Camera MainCamera
        {
            get
            {
                if (mMainCamera == null)
                {
                    mMainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
                }
                return mMainCamera;
            }
        }

        public GameObject GetCurrentPage()
        {
            if (!string.IsNullOrEmpty(CurrentPageName) && PageSlots.ContainsKey(CurrentPageName))
            {
                return PageSlots [CurrentPageName].go;
            }
            return null;
        }

        public PageInfo GetCurrentPageInfo()
        {
            if (PageSlots.ContainsKey(CurrentPageName))
            {
                return PageSlots [CurrentPageName];
            }
            return null;
        }

        public GameObject GetPage(string name)
        {
            if (LoadedPage(name))
                return PageSlots [name].go;
            return null;
        }

        public T GetPage<T>() where T : class
        {
            string page = typeof(T).Name;
            if (LoadedPage(page))
                return PageSlots [page].view as T;
            return default(T);
        }

        public bool LoadedPage(string name)
        {
            if (PageSlots.ContainsKey(name) && PageSlots [name].go != null)
                return true;
            return false;
        }

        public string CurrentPageName
        {
            get
            {
                return PageStack.Count > 0 ? PageStack.Peek().name : null;
            }
        }

        public int CurrentDepth
        {
            get
            {
                return PageStack.Count > 0 ? PageStack.Peek().depth : UILayer.PageDepth;
            }
        }


        public bool HasLastPage
        {
            get
            {
                Debug.Log(">>>>>>>>>>>>>>>>>>>>>>PageStack.Count:" + PageStack.Count);
                return (PageStack.Count >= 2);
            }
        }
           
 

        public void ClearStack()
        {
            PageStack.Clear();
        }

        private void HideStackPage()
        {
            foreach (PageInfo page in PageSlots.Values)
            {
                if (page.inStack && page.go.activeSelf)
                {
                    page.go.SetActive(false);
                }
            }
        }

        private void ShowStackPage()
        {
        }


        public void Reset()
        {
            foreach (PageInfo page in PageSlots.Values)
            {
                if (page.go == null)
                    continue;
                page.go.SetActive(false);
                GameObject.Destroy(page.go);
            }
            PageSlots.Clear();
            PageStack.Clear();
        }

        public GameObject Instantiate(Object original, Transform parent)
        {
            if (parent != null)
            {
                return Instantiate(original, parent, parent.position, Quaternion.identity, Vector3.one);
            } else
                return Instantiate(original, parent, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public GameObject Instantiate(Object original, Transform parent, Vector3 position)
        {

            return Instantiate(original, parent, position, Quaternion.identity, Vector3.one);
        }

        public GameObject Instantiate(Object original, Transform parent, Vector3 position, Vector3 scale)
        {

            return Instantiate(original, parent, position, Quaternion.identity, scale);
        }

        public GameObject Instantiate(Object original, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (original == null)
                return null;
            GameObject instance;
            if (parent != null)
            {
                instance = GameObject.Instantiate(original, parent.position, rotation) as GameObject;
                instance.transform.parent = parent;
                instance.transform.localPosition = position;
            } else
            {
                instance = GameObject.Instantiate(original, position, rotation) as GameObject;
            }
            instance.name = original.name;
            instance.transform.localScale = scale;
            return instance;
        }
    }


}
