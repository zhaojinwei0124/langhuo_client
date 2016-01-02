using UnityEngine;
using System.Collections;

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
    }

    private void OnClickItem0(GameObject go)
    {
        Debug.Log("user account");
    }

    private void OnClickItem1(GameObject go)
    {
        Debug.Log("user account");
    }

    private void OnClickItem2(GameObject go)
    { 
        Debug.Log("langjian get");
    }

    private void OnClickItem3(GameObject go)
    {
        Debug.Log("langti get");
    }

    private void OnClickItem4(GameObject go)
    {
        Debug.Log("friend invite");
    }

    private void OnClickItem5(GameObject go)
    {
        Debug.Log("help");
    }

    private void OnClickItem6(GameObject go)
    {
        Debug.Log("version");
    }
}
