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
        public System.Action onPageClosed;
        public View view;
        public GameObject go;
    }

    public delegate void GameObjectCallBack(GameObject go);

    public class UIHandler : Single<UIHandler>
    {
        private Stack<PageInfo> mPageStack = new Stack<PageInfo>();
        private Font mFont;
        private TextAsset mText;
        private static UIHandler mInstance;

        public UIRoot Root{ get { return UIManager.Instance.root; } }

        public override void Init()
        {
            base.Init();
            mFont = Resources.Load<Font>("Fonts/msyh");
            mText=Resources.Load<TextAsset>("Fonts/loc_data");
            Localization.Load(mText);
        }

        public Transform transFront{ get { return UIManager.Instance.front_obj.transform; } }

        public Transform transHome{ get { return UIManager.Instance.home_obj.transform; } }

        public Font DefaultFont { get { return mFont; } }

        public UICamera MainCamera{ get { return UIManager.Instance.uicamera; } }
       
        public PageInfo GetCurrentPageInfo()
        {
            return mPageStack.Count > 0 ? mPageStack.Peek() : null;;
        }

        public GameObject GetCurrentPage()
        {
            return mPageStack.Count > 0 ? mPageStack.Peek().go : null;
        }

        public string CurrentPageName
        {
            get
            {
                return mPageStack.Count > 0 ? mPageStack.Peek().name : null;
            }
        }
           
        public void ClearStack()
        {
            foreach(PageInfo info in mPageStack)
            {
                info.onPageClosed();
                GameObject.Destroy(info.go);
            }
            mPageStack.Clear();
        }

        private void HideStackPage()
        {
            foreach (PageInfo page in mPageStack)
            {
                if (page.go.activeSelf)
                {
                    page.go.SetActive(false);
                }
            }
        }

        public PageInfo Push(PageID id)
        {
            return Push(id, null);
        }

        public PageInfo Push(PageID id, object arg)
        {
            return Push(id,arg,null);
        }

        public PageInfo Push(PageID id,System.Action onClose)
        {
            return Push(id,null,onClose);
        }

        public PageInfo Push(PageID id, object arg,System.Action onClose)
        {
          //  Debug.Log("push: "+id);
            Object obj = Resources.Load("Prefabs/GamePage/" + id);
            GameObject go = Instantiate(obj);
            PageInfo info = new PageInfo();
            info.go = go;
            info.name = id;
            info.arg = arg;
            info.onPageClosed=onClose;
            info.view=go.GetComponent<View>();
            mPageStack.Push(info);
            info.view.SetDepth(mPageStack.Count);
            if(arg!=null) info.view.Refresh(arg);
            else info.view.RefreshView();
            if(!UIManager.Instance.IsFront()) 
            {
                UIManager.Instance.ShowFront(true);
            }
            obj=null;
            return info;
        }


        public void Pop()
        {
            PageInfo info=mPageStack.Peek();
            if(info.onPageClosed!=null)info.onPageClosed();
            GameObject.Destroy(info.go);
            mPageStack.Pop();
//            Debug.LogError("stask count: "+mPageStack.Count);
            if(mPageStack.Count<=0)
            {
                UIManager.Instance.ShowFront(false);
            }
            else
            {
                info=mPageStack.Peek();
          //      Debug.Log("page name: "+info.name+" arg: "+(info.arg.ToString()));
                if(info.arg==null) info.view.RefreshView();
                else info.view.Refresh(info.arg);
            }
        }

        private GameObject Instantiate(Object original)
        {
            return Instantiate(original, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        private GameObject Instantiate(Object original, Vector3 position)
        {
            return Instantiate(original, position, Quaternion.identity, Vector3.one);
        }

        private GameObject Instantiate(Object original, Vector3 position, Vector3 scale)
        {
            return Instantiate(original, position, Quaternion.identity, scale);
        }

        private GameObject Instantiate(Object original, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (original == null)
                return null;
            Transform parent = transFront;
            GameObject instance = GameObject.Instantiate(original, parent.position, rotation) as GameObject;
            instance.transform.parent = parent;
            instance.transform.localPosition = position;
            instance.name = original.name;
            instance.transform.localScale = scale;
            return instance;
        }
    }


}
