using UnityEngine;
using System.Collections;
using Network;

public class AccountPage : View
{
    public UITexture m_head;
    public UIInput m_user;
    public UIInput m_name;
    public UIInput m_reciver;
    public UIInput m_address;
    public UIInput m_tel;
    public GameObject m_objRegist;
    public UILabel m_lblRegist;
    private string mUserid;

    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_objRegist).onClick = OnRegist;
        UIEventListener.Get(m_head.gameObject).onClick = OnTextClick;
        RefreshUI();
    }
    
    private void GetLocalInfo()
    {
    }

    private bool CheckLocal()
    {
        mUserid = PlayerPrefs.GetString("userid", null);
        return !string.IsNullOrEmpty(mUserid);
    }

    private bool CheckValid()
    {
        if (m_tel.label.text.Length != 11)
        {
            Debug.LogError("tel is not valid!");
            return false;
        }
        return true;
    }

    private void OnTextClick(GameObject go)
    {
        Debug.Log("Text click");
    }

    private void OnRegist(GameObject go)
    {
        Debug.Log("Onregist");
        if (!CheckValid())
            return;
        NetCommand.Instance.RegistUser(m_user.label.text, m_tel.label.text, (int)GameBaseInfo.Instance.payMode, m_address.label.text, (res) =>
        {
            Debug.Log("res: " + res);
            if (res.Equals("true"))
            {
                PlayerPrefs.SetString("userid", m_tel.label.text);
                Debug.Log("regist use success!");
            }
        });
    }

    private void RefreshUI()
    {
        Debug.Log("username: " + PlayerPrefs.GetString("userid"));
        m_lblRegist.text = CheckLocal() ? "确定" : "注册 ";
        if (CheckLocal())
        {
            NetCommand.Instance.LoginUser(mUserid, (res) =>
            {
                Debug.Log("res: "+res);
                NUser nuser=GameCore.Util.Instance.Get<NUser>(res);
                Debug.Log("name: "+nuser.name+" tel: "+nuser.tel);
                GameBaseInfo.Instance.userid= nuser.tel;
                GameBaseInfo.Instance.payMode=(PayMode)nuser.paymode;
                GameBaseInfo.Instance.balance=nuser.balance;
                GameBaseInfo.Instance.addr=nuser.addr;
                m_user.label.text=m_reciver.label.text=m_name.label.text=nuser.name;
                m_address.label.text=nuser.addr;
                m_tel.label.text=nuser.tel.ToString();
            });
        }

    }

}
