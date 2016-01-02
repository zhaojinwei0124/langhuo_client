using UnityEngine;
using System.Collections;
using GameCore;

public class MyPage : View 
{

    public GameObject[] m_tabpages;

    public UITab[] m_tabs;

    public GameObject m_set;

    public GameObject m_notify;

    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_tabs[0].gameObject).onClick=(go)=>Show(0);
        UIEventListener.Get(m_tabs[1].gameObject).onClick=(go)=>Show(1);
        UIEventListener.Get(m_tabs[2].gameObject).onClick=(go)=>Show(2);
        UIEventListener.Get(m_set).onClick=OnSetting;
        UIEventListener.Get(m_notify).onClick=OnNotify;
    }


    private void OnSetting(GameObject go)
    {
        UIHandler.Instance.Push(PageID.SETTING);
    }

    private void OnNotify(GameObject go)
    {
        Debug.Log("onnotify");
    }

    private void HideAll()
    {
        foreach(var ite in m_tabpages)
        {
            ite.gameObject.SetActive(false);
        }
    }
    
    
    private void Show(int index)
    {
        Debug.Log("show index: "+index);
        HideAll();
        m_tabpages[index].gameObject.SetActive(true);
    }
}
