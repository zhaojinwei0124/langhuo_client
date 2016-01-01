using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResourceLoad;

namespace GameCore
{
    public class PageInfo
    {
        public string name;
        public object arg;
        public bool inStack;
        public System.Action onPageClosed;
        public View view;
        public GameObject go;
    }

    public delegate void GameObjectCallBack(GameObject go);

    public class UIHandler : Single<UIHandler>
    {
        private Stack<PageInfo> PageStack = new Stack<PageInfo>();
        private Font mFont;
        private static UIHandler mInstance;

        public UIRoot Root{ get { return UIManager.Instance.root; } }

        public override void Init()
        {
            base.Init();
            mFont = Resources.Load<Font>("Font/msyh");
        }

        public Transform transFront{ get { return UIManager.Instance.front_obj.transform; } }

        public Transform transHome{ get { return UIManager.Instance.home_obj.transform; } }

        public Font DefaultFont { get { return mFont; } }

        public UICamera MainCamera{ get { return UIManager.Instance.uicamera; } }

       
        public PageInfo GetCurrentPageInfo()
        {
            return null;
        }

        public GameObject GetPage(string name)
        {
            return null;
        }

        public T GetPage<T>() where T : class
        {
//            string page = typeof(T).Name;
//            if (LoadedPage(page))
//                return PageSlots [page].view as T;
            return default(T);
        }

        public bool LoadedPage(string name)
        {
//            if (PageSlots.ContainsKey(name) && PageSlots [name].go != null)
//                return true;
            return false;
        }

        public string CurrentPageName
        {
            get
            {
                return PageStack.Count > 0 ? PageStack.Peek().name : null;
            }
        }
           
        public void ClearStack()
        {
            PageStack.Clear();
        }

        private void HideStackPage()
        {
            foreach (PageInfo page in PageStack)
            {
                if (page.inStack && page.go.activeSelf)
                {
                    page.go.SetActive(false);
                }
            }
        }

        public void Reset()
        {
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
