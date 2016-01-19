using UnityEngine;
using System.Collections;
using GameCore;

public class UIManager : MonoSingle<UIManager>
{

    public UITab[] home_tabs;

    public View[] home_views;

    public UICamera uicamera;

    public UIRoot root;

    public GameObject home_obj;

    public GameObject front_obj;

    public GameObject close_obj;

    private bool mFront=false;

    private int mIndex=0;

    public void Init()
    {
        UIEventListener.Get(home_tabs [0].gameObject).onClick = (go) => ShowHomeView(0);
        UIEventListener.Get(home_tabs [1].gameObject).onClick = (go) => ShowHomeView(1);
        UIEventListener.Get(home_tabs [2].gameObject).onClick = (go) => ShowHomeView(2);
        UIEventListener.Get(close_obj).onClick=Close;

        mIndex=0;
        ShowFront(false);
    }
   
    public bool IsFront()
    {
        return mFront;
    }

    public void Close(GameObject go)
    {
        UIHandler.Instance.Pop();
    }


    public void HideHomeViews()
    {
        for(int i=0;i<home_views.Length;i++)
        {
            home_views[i].gameObject.SetActive(false);
        }
    }
   
    public void ResetIndex()
    {
        mIndex=0;
        home_tabs[0].Set(true);
    }

    public void ShowHomeView(int index)
    {
        mIndex=index;
        HideHomeViews();
        home_views[index].gameObject.SetActive(true);
        home_views[index].RefreshView();
    }

    public void ShowFront(bool show)
    {
        mFront=show;
        home_obj.SetActive(!show);
        front_obj.SetActive(show);
        if(!show) ShowHomeView(mIndex);
    }


}
