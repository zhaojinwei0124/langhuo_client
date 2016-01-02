using UnityEngine;
using System.Collections;
using Network;

public class AccountPage : View
{
    public UITexture m_head;
    public UIInput m_user;
    public UIInput m_type;
    public UIInput m_receive;
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
        if (!CheckValid())
            return;

        if (!CheckLocal())
        {
            NetCommand.Instance.RegistUser(m_user.label.text, m_tel.label.text, (int)GameBaseInfo.Instance.payMode, 
                                           m_address.label.text, PayType.LANGJIAN, (res) =>
            {
                Debug.Log("res: " + res);
                if (res.Equals("true"))
                {
                    PlayerPrefs.SetString("userid", m_tel.label.text);
                    Debug.Log("regist use success!");
                    m_lblRegist.text = "确定";
                }
            });
        } else
        {
            NetCommand.Instance.SysnOrder((msg) =>
            {
                Debug.Log("commit order success!");
                Close();
            },
            (err)=>
            {
                Debug.LogError("sysnc order data fail!");
            });
        }
    }

    private void RefreshUI()
    {
        Debug.Log("username: " + PlayerPrefs.GetString("userid"));
        m_lblRegist.text = CheckLocal() ? "确定" : "注册 ";
        if (CheckLocal())
        {
            NetCommand.Instance.LoginUser(mUserid, (res) =>
            {
                Debug.Log("res: " + res);
                NUser nuser = GameCore.Util.Instance.Get<NUser>(res);
                GameBaseInfo.Instance.user = nuser;
                m_user.label.text = m_receive.label.text = nuser.name;
                m_type.text = nuser.type == 1 ? "浪尖" : "浪蹄";
                m_address.label.text = nuser.addr;
                m_tel.label.text = nuser.tel.ToString();
            });
        }

    }

}
