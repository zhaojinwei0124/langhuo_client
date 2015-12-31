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

    public enum DialogStyle
    {
        DS_ConfirmOnly = 0,
        DS_ConfirmCancel,
        DS_RechargeCancel,
        DS_JumpCancel,
    }

    public enum UnitStyle
    {
        DEFAULT = 0,
        MINI = 1,
    }

    public enum ViewStyle
    {
        DEFAULT = 0,
        SCAN = 1,
        EXCHANGE = 2,
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

    public class ControlInfo
    {
        public string name;
        public string arg;
        public int lifeTimes;
        public View view;
        public GameObject go;
        public object param;
    }

    public class DialogInfo
    {
        public DialogStyle mStyle;
        public string mTitle;
        public string mContent;
        public StringCallBack mCallback;
    }

    public delegate void CallBack();

    public delegate void IntCallBack(int i);

    public delegate void FloatCallBack(float f);

    public delegate void StringCallBack(string s);

    public delegate void GameObjectCallBack(GameObject go);

    public class UIHandler : Single<UIHandler>
    {
        public static int LIFE_TIMES = 3;
        private Transform mMainPanel = null;
        private Camera mMainCamera = null;
        private Dictionary<string, ControlInfo> ControlSlots = new Dictionary<string, ControlInfo>();
        private Dictionary<string, PageInfo> PageSlots = new Dictionary<string, PageInfo>();
        private Stack<PageInfo> PageStack = new Stack<PageInfo>();
        private Queue<DialogInfo> DialogQueue = new Queue<DialogInfo>();
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

        public ControlInfo GetControlInfo(string name)
        {
            if (ControlSlots.ContainsKey(name))
            {
                return ControlSlots [name];
            } else
            {
                return null;
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

        public void Clear()
        {
            ClearDialogs();
        }

        #region Unit
        public GameObject GetUnit(string _name)
        {
            GameObject pref = null;
            ResourceHandler.Instance.LoadRes("Prefabs/Unit/" + _name, delegate(Object res)
            {
                pref = res as GameObject;
            });
            return pref;
        }

        public GameObject GetUnitInstance(string _name)
        {
            GameObject _pref = GetUnit(_name);
            if (_pref != null)
            {
                return (GameObject.Instantiate(_pref) as GameObject);
            } else
            {
                return null;
            }
        }

        public void GetUnitAsync(string _name, LoadGoFinishCb cb)
        {
            GameObject pref = null;
            ResourceHandler.Instance.LoadResAsync("Prefabs/Unit/" + _name, delegate(Object res)
            {
                pref = res as GameObject;
                if (cb != null)
                    cb(pref);
            });
        }

        public Component CreateUnit(string unitName, System.Type componentType, Transform parentTrans, int depth, UIComponentDict pageComponents)
        {
            Transform trans = MonoUtil.CreatePrefab(UIHandler.Instance.GetUnit(unitName), unitName, parentTrans);
            Component comp = trans.gameObject.GetComponent(componentType);
            if (comp == null)
                comp = trans.gameObject.AddComponent(componentType);
            componentType.GetMethod("SetDepth").Invoke(comp, new object[] { depth });
            UIComponentDict unitComponents = trans.GetComponent<UIComponentDict>();
            if (pageComponents != null && unitComponents != null)
                pageComponents.m_panelList.AddRange(unitComponents.m_panelList);
            return comp;
        }

        #endregion
        #region PAGE
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

        public void RemovePageInStack(string name)
        {
            PageInfo[] pages = PageStack.ToArray();
            PageStack.Clear();
            string cachePageName = CurrentPageName;
            for (int i = pages.Length - 1; i >= 0; i--)
            {
                if (name.Equals(pages [i].name))
                {
                    PageSlots [pages [i].name].inStack = false;
                    HidePage(pages [i].name);
                } else
                {
                    PageStack.Push(pages [i]);
                }
            }
        }

        private void ShowLastPage(string name, GameObjectCallBack OnPageCallback = null)
        {
            while (PageStack.Count >= 2 && !CurrentPageName.Equals(name))
            {
                PageInfo curPage = PageStack.Pop();
                if (curPage != null)
                {
                    PageSlots [curPage.name].inStack = false;
                    HidePage(curPage.name);
                }
            }
            PageInfo pageInfo = PageSlots [CurrentPageName];
            pageInfo.lifeTimes = LIFE_TIMES;
            pageInfo.callBack = OnPageCallback;
            pageInfo.onPageClosed = null;
            if (PageStack.Count >= 1)
            {
                pageInfo = PageStack.Pop();
                SwitchPage(pageInfo);
            }
        }

        public void SetLastPage(string name)
        {
            PageInfo curPage = PageStack.Pop();
            while (!CurrentPageName.Equals(name) && PageStack.Count >= 1)
            {
                PageInfo page = PageStack.Pop();
                PageSlots [page.name].inStack = false;
                HidePage(page.name);
            }
            PageStack.Push(curPage);
        }

        public void ShowLastPage()
        {
            ShowLastPage(default(GameObjectCallBack));
        }

        public void ShowLastPage(GameObjectCallBack OnPageCallback)
        {
            if (PageStack.Count >= 2)
            {
                PageInfo curPage = PageStack.Pop();
                PageSlots [curPage.name].inStack = false;
                HidePage(curPage.name);
                PageInfo lastPage = PageStack.Peek();
                lastPage.lifeTimes = LIFE_TIMES;
                lastPage.callBack = OnPageCallback;
                lastPage.forward = false;
                if (PageStack.Count >= 1)
                {
                    SwitchPage(PageStack.Pop());
                }
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

        public View ShowAndGetPage(string name)
        {
            ShowPage(name);
            name = name.Split('&') [0];
            return UIHandler.Instance.GetPageInfo(name).view;
        }

        public View ShowAndGetControl(string name)
        {
            ShowControl(name);
            name = name.Split('&') [0];
            return UIHandler.Instance.GetControlInfo(name).view;
        }

        public void ShowPage(string name)
        {
            ShowPage(name, null);
        }

        public void ShowPage(string name, bool hideBack, bool history)
        {
            ShowPage(name);
        }

        public View ShowPage(string name, object param)
        {
            return ShowPage(name, param, null);
        }

        public View ShowPage(string name, object param, GameObjectCallBack onPageCallBack)
        {
            string rule = PageCheckIn(name, "");
            if (!string.IsNullOrEmpty(rule))
            {
                string[] info = rule.Split('&');
                if (info [0].Equals("False"))
                {
                    Debug.LogError(info [2]);
                    return null;
                }
            }
            if (!LoadedPage(name))
                AddPageInfo(name, new PageInfo());
            PageInfo pageInfo = GetPageInfo(name);
            pageInfo.arg = "";
            pageInfo.param = param;
            if (pageInfo.inStack)
            {
                ShowLastPage(name, onPageCallBack);
                return pageInfo.view;
            }
            pageInfo.name = name;
            pageInfo.callBack = onPageCallBack;
            pageInfo.lifeTimes = UIHandler.LIFE_TIMES;
            pageInfo.forward = true;
            pageInfo.onPageClosed = null;

            SwitchPage(pageInfo);
            return pageInfo.view;
        }

        public void ShowPage(string name, GameObjectCallBack OnPageCallback)
        {
            string arg = "";
            int argMarkIndex = name.IndexOf('&');
            if (argMarkIndex != -1)
            {
                arg = name.Substring(argMarkIndex + 1);
                name = name.Substring(0, argMarkIndex);
            }
            string rule = PageCheckIn(name, arg);
            if (!string.IsNullOrEmpty(rule))
            {
                string[] info = rule.Split('&');
                if (info [0].Equals("False"))
                {
                    Debug.LogError(info [2]);
                    return;
                }
            }
            if (!PageSlots.ContainsKey(name))
            {
                PageSlots [name] = new PageInfo();
            }
            PageInfo pageInfo = PageSlots [name];
            pageInfo.arg = arg;
            if (PageSlots [name].inStack)
            {
                ShowLastPage(name, OnPageCallback);
                return;
            }

            pageInfo.name = name;
            pageInfo.callBack = OnPageCallback;
            pageInfo.lifeTimes = LIFE_TIMES;
            pageInfo.forward = true;
            pageInfo.onPageClosed = null;
            SwitchPage(pageInfo);
        }

        private void SwitchPage(PageInfo curPage)
        {
            if (curPage == null)
                return;
            if (curPage.go != null)
            {
                if (!curPage.inStack)
                    ClearLastPage();
                curPage.go.SetActive(true);
                curPage.inStack = false;
                curPage.view.Param = curPage.param;
                SetCurrentPage(curPage);
                if (PageStack.Count == 1)
                {
                    ClearAllMemory();
                }
            } else
            {
                curPage.inStack = false;
                ResourceHandler.Instance.LoadRes("Prefabs/GamePage/" + curPage.name,
                delegate(Object prefab)
                {
                    if (prefab == null)
                    {
                        Debug.LogError("prefab is null " + curPage.name);
                        return;
                    }
                    ClearLastPage();
                    GameObject go = Instantiate(prefab, MainPanel, Vector3.zero);
                    curPage.view = go.GetComponent<View>();
                    if (curPage.view == null)
                    {
                        Component component = go.AddComponent(curPage.name);
                        if (component == null)
                        {
                            Debug.LogWarning("Can't find component class " + curPage.name);
                            return;
                        }
                        curPage.view = component as View;
                    }
                    curPage.go = go;
                    curPage.view.Param = curPage.param;
                    SetCurrentPage(curPage);
                }, true);
            }
        }

        private void ClearLastPage()
        {
            Clear();
            if (PageStack.Count > 0)
            {
                PageInfo lastPage = PageStack.Peek();
                if (!lastPage.view.History)
                {
                    PageStack.Pop();
                    lastPage.inStack = false;
                    HidePage(lastPage.name);
                } else
                {
                    lastPage.view.UnFocusView();
                }
            }
        }

        private void SetCurrentPage(PageInfo curPage)
        {
            curPage.depth = CurrentDepth + UILayer.DepthOffset;
            curPage.view.SetDepth(curPage.depth);
            if (curPage.view.HideBack || curPage.view.FullScreen)
                HideStackPage();
            else
                ShowStackPage();
            PageStack.Push(curPage);
            curPage.inStack = true;
        }

        public string PageCheckIn(string name, string arg = "")
        {
            if (string.IsNullOrEmpty(name))
                return "";
            string msg = string.Empty;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Type type = assembly.GetType(name);
            if (type == null)
                return "";
            System.Reflection.MethodInfo methodInfo = type.GetMethod("CheckIn", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (methodInfo == null)
                return "";
            msg = (string)methodInfo.Invoke(null, new string[] { arg });
            return msg;
        }

        public void RefreshStackPage()
        {
            foreach (string name in PageSlots.Keys)
            {
                if (PageSlots [name].inStack && !name.Equals(PageStack.Peek().name))
                {
                    PageSlots [name].view.DoRefreshView();
                }
            }
        }

        public void ClearAllMemory()
        {
            Debug.Log(">>>>>>>>>>>>>>:ClearMemoryImmediately");
            ClearMemory();
            Resources.UnloadUnusedAssets();
        }

        public void ClearMemory()
        {
            List<string> keys = new List<string>();
            foreach (string n in PageSlots.Keys)
            {
                keys.Add(n);
            }

            foreach (string key in keys)
            {
                if (PageSlots.ContainsKey(key) && PageSlots [key].lifeTimes <= 0)
                {
                    if (!PageSlots [key].inStack)
                    {
                        DestroyPage(key);
                    }
                }
            }
            TextureHandler.Instance.Release();
            ResourceHandler.Instance.Release();
        }

        public void HidePage(string name)
        {
            PageInfo pageInfo = GetPageInfo(name);
            if (pageInfo != null && !pageInfo.inStack)
            {
                if (pageInfo.view != null)
                    pageInfo.view.CloseView();
                System.Action onPageClosed = pageInfo.onPageClosed;
                pageInfo.onPageClosed = null;
                if (onPageClosed != null)
                    onPageClosed();
            }

            if (PageSlots.ContainsKey(name) && PageSlots [name] != null && PageSlots [name].go != null)
            {
                PageSlots [name].go.SetActive(false);
            }
        }

        public void DestroyPage(string name)
        {
            HidePage(name);
            if (PageSlots.ContainsKey(name) && PageSlots [name] != null && PageSlots [name].go != null)
            {
                GameObject.Destroy(PageSlots [name].go);
                PageSlots [name] = null;
            }
            PageSlots.Remove(name);
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
            PageInfo[] pages = PageStack.ToArray();
            int index = 0;
            for (index = 0; index < pages.Length; index++)
            {
                pages [index].go.SetActive(true);
                if (pages [index].view.FullScreen == true)
                    break;
            }
            while (++index < pages.Length)
            {
                pages [index].go.SetActive(false);
            }
        }

        #endregion PAGE
        #region Control
        public bool LoadedControl(string name)
        {
            if (ControlSlots.ContainsKey(name))
                return true;
            return false;
        }

        public GameObject GetControl(string name)
        {
            if (LoadedControl(name))
                return ControlSlots [name].go;
            return null;
        }

        public T GetControl<T>() where T : class
        {
            string ctl = typeof(T).Name;
            if (LoadedControl(ctl))
                return ControlSlots [ctl].view as T;
            return default(T);
        }

        public void ShowControl(string name)
        {
            ShowControl(name, UILayer.ControlDepth, null);
        }

        public ControlInfo ShowControl(string name, Transform parent, ref List<ControlInfo> controls, bool isDestroyCtl = false)
        {
            if (controls == null)
                controls = new List<ControlInfo>();
            foreach (var v in controls)
            {
                if (v == null || v.view == null || v.view.name.Equals(name.Split('&') [0]))
                    continue;
                if (v.view.gameObject.activeSelf && v.view.History)
                {
                    v.view.UnFocusView();
                    if (isDestroyCtl)
                        DestroyControl(v.view.name);
                    else
                        HideControl(v.view.name);

                }
            }
            if (string.IsNullOrEmpty(name))
                return null;
            ControlInfo ci = ShowControl(name, parent);
            if (ci != null)
            {
                if (!controls.Contains(ci))
                    controls.Add(ci);
            }
            return ci;
        }

        public ControlInfo ShowControl(string name, Transform parent)
        {
            ShowControl(name);
            ControlInfo ci = GetControlInfo(name.Split('&') [0]);
            if (ci != null)
            {
                if (ci.view != null)
                {
                    ci.view.SetDepth(GetCurrentPage().GetComponent<View>().GetDepth() + 1);
                    ci.view.transform.parent = parent;
                    ci.view.transform.localPosition = Vector3.zero;
                    ci.view.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            return ci;
        }

        public void ShowControl(string name, GameObjectCallBack GameObjectCallBack)
        {
            ShowControl(name, UILayer.ControlDepth, GameObjectCallBack);
        }

        public void ShowControl(string name, int depth)
        {
            ShowControl(name, depth, null);
        }

        public void ShowControl(string name, int depth, GameObjectCallBack GameObjectCallBack)
        {
            string arg = "";
            int argMarkIndex = name.IndexOf('&');
            if (argMarkIndex != -1)
            {
                arg = name.Substring(argMarkIndex + 1);
                name = name.Substring(0, argMarkIndex);
            }

            if (!ControlSlots.ContainsKey(name))
            {
                ControlSlots [name] = new ControlInfo();
            }
            ControlSlots [name].lifeTimes = LIFE_TIMES;
            ControlSlots [name].arg = arg;
            if (ControlSlots [name].go != null)
            {
                ControlSlots [name].go.SetActive(true);
                View view = ControlSlots [name].view;
                view.SetDepth(depth);
            } else
            {
                ResourceHandler.Instance.LoadRes("Prefabs/GameControl/" + name,
                delegate(Object prefab)
                {
                    if (prefab == null)
                    {
                        Debug.LogError("control is null " + name);
                        return;
                    }
                    GameObject go = Instantiate(prefab, MainPanel);
                    View view = go.GetComponent<View>();
                    if (view == null)
                    {
                        Component component = go.AddComponent(name);
                        if (component == null)
                        {
                            Debug.LogWarning("Can't find component class " + name);
                            return;
                        }
                        view = component as View;
                    }
                    view.SetDepth(depth);
                    ControlSlots [name].view = view;
                    ControlSlots [name].go = go;
                }, true);
            }
        }

        public void HideControl(string name)
        {
            ControlInfo controlInfo = GetControlInfo(name);
            if (controlInfo != null)
            {
                controlInfo.view.CloseView();
            }
            if (ControlSlots.ContainsKey(name) && ControlSlots [name] != null && ControlSlots [name].go != null)
            {
                ControlSlots [name].go.SetActive(false);
            }
        }

        public void HideAllControl()
        {
            foreach (string name in ControlSlots.Keys)
            {
                HideControl(name);
            }
        }

        public void DestroyControl(string name)
        {
            HideControl(name);
            if (ControlSlots.ContainsKey(name) && ControlSlots [name] != null && ControlSlots [name].go != null)
            {
                GameObject.Destroy(ControlSlots [name].go);
                ControlSlots [name] = null;
            }
            ControlSlots.Remove(name);
        }

        #endregion Control

        public void ClearDialogs()
        {
            DialogQueue.Clear();
        }

        public void Reset()
        {
            DialogQueue.Clear();
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
