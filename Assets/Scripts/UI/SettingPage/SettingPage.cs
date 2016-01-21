using UnityEngine;
using System.Collections.Generic;
using GameCore;


public class SettingPage : View
{

    public GameObject[] items;

    public GameObject logoutBtn;

	public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(items[0]).onClick=OnClickItem0;
        UIEventListener.Get(items[1]).onClick=OnClickItem1;
        UIEventListener.Get(items[2]).onClick=OnClickItem2;
        UIEventListener.Get(items[3]).onClick=OnClickItem3;
        UIEventListener.Get(items[4]).onClick=OnClickItem4;
        UIEventListener.Get(items[5]).onClick=OnClickItem5;
        UIEventListener.Get(items[6]).onClick=OnClickItem6;
        UIEventListener.Get(logoutBtn).onClick=OnLogout;
    }


    private void OnLogout(GameObject go)
    {
        Debug.Log("logout");
        Caching.CleanCache();
        PlayerPrefs.DeleteAll();
        Toast.Instance.Show(10049);
        Close();
    }

    private void OnClickItem0(GameObject go)
    {
        Debug.Log("user account");
        UIHandler.Instance.Push(PageID.ACCOUNT);
    }

    private void OnClickItem1(GameObject go)
    {
        Toast.Instance.Show(10010);
    }

    private void OnClickItem2(GameObject go)
    { 
        UIHandler.Instance.Push(PageID.LANGJIAN);
    }

    private void OnClickItem3(GameObject go)
    {
        UIHandler.Instance.Push(PageID.LANGTI);
    }

    private void OnClickItem4(GameObject go)
    {
        UIHandler.Instance.Push(PageID.INVITE);
    }

    private void OnClickItem5(GameObject go)
    {
        UIHandler.Instance.Push(PageID.TEXT,new StrText(10054,10055));
    }

    private void OnClickItem6(GameObject go)
    {
        UIHandler.Instance.Push(PageID.TEXT,new StrText(10056,10057));
    }
}
