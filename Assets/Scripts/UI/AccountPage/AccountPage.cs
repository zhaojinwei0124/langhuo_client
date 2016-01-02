using UnityEngine;
using System.Collections;

public class AccountPage : View
{
    public UITexture m_head;
    public UIInput m_user;
    public UIInput m_name;
    public UIInput m_reciver;
    public UIInput m_address;
    public UIInput m_tel;
    public GameObject m_regist;

    private string mUserid;


    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_regist).onClick=OnRegist;
        UIEventListener.Get(m_head.gameObject).onClick=OnTextClick;
    }

    
    private void GetLocalInfo()
    {
        
    }

    private bool CheckLocal()
    {
        mUserid = PlayerPrefs.GetString("userid",null);
        return !string.IsNullOrEmpty(mUserid);
    }


    private void CheckValid()
    {

    }

    private void OnTextClick(GameObject go)
    {
        Debug.Log("Text click");
    }

    private void OnRegist(GameObject go)
    {
        Debug.Log("Onregist");
    }

    private void RefreshUI()
    {
        
    }

}
