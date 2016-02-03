using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using Network;

public class MyPage : View
{

    public GameObject[] m_tabpages;
    public UITab[] m_tabs;
    public GameObject m_set;
    public GameObject m_notify;
    public UILabel m_lblName;
    public UILabel m_lblBalance;

    public override void RefreshView()
    {
        base.RefreshView();
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerprefID.USERID, null)))
        {
            UIHandler.Instance.Push(PageID.Regist, () =>
            {
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerprefID.USERID)))
                {
                    UIManager.Instance.ResetIndex();
                }
            });
        } else
        {
            UIEventListener.Get(m_tabs [0].gameObject).onClick = (go) => Show(0);
            UIEventListener.Get(m_tabs [1].gameObject).onClick = (go) => Show(1);
            UIEventListener.Get(m_tabs [2].gameObject).onClick = (go) => Show(2);
            UIEventListener.Get(m_set).onClick = OnSetting;
            UIEventListener.Get(m_notify).onClick = OnNotify;
            RefreshAccount();
           
        }
    }



    private void RefreshAccount()
    {
        NetCommand.Instance.LoginUser(PlayerPrefs.GetString(PlayerprefID.USERID), (string res) =>
        {
            NUser nuser = Util.Instance.Get<NUser>(res);
            GameBaseInfo.Instance.user = nuser;
            RefreshUser();
            RefreshOrders();
            RefreshTab();
        });
    }
    private void RefreshTab()
    {
        int type = GameBaseInfo.Instance.user.type;
        m_tabs [1].gameObject.SetActive(type == 1 || type == 3);
        m_tabs [2].gameObject.SetActive(type == 2 || type == 3);
        if(type==2) m_tabs[2].transform.localPosition=Vector3.zero;
        else if(type==1||type==3) m_tabs[2].transform.localPosition=new Vector3(200,0,0);
    }


    private void RefreshOrders()
    {
        NetCommand.Instance.SearchOders(GameBaseInfo.Instance.user.tel, (string res) =>
        {
            GameBaseInfo.Instance.myOrders = Util.Instance.Get<List<NOrder>>(res);
        }, (string err) =>
        {
            Toast.Instance.Show(10026);
        });
    }

    private void RefreshUser()
    {
        m_lblName.text = GameBaseInfo.Instance.user.name;
        m_lblBalance.text = "￥" + string.Format("{0:f}", GameBaseInfo.Instance.user.balance);
    }

    private void OnSetting(GameObject go)
    {
        UIHandler.Instance.Push(PageID.SETTING);
    }

    private void OnNotify(GameObject go)
    {
        Toast.Instance.Show(10053);
    }

    private void HideAll()
    {
        foreach (var ite in m_tabpages)
        {
            ite.gameObject.SetActive(false);
        }
    }
    
    private void Show(int index)
    {
//        Debug.Log("show index: "+index);
        HideAll();
        m_tabpages [index].gameObject.SetActive(true);
        if (index == 1)
        {
            MyPage_langjian.Instance.Refresh();
        } else if (index == 2)
        {
            MyPage_langti.Instance.Refresh();
        }
    }
}
