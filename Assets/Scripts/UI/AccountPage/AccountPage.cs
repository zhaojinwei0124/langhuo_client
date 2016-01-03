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
                    m_lblRegist.text = "提交订单";
                }
            });
          
        } else
        {
            int price=0;
            foreach(var item in GameBaseInfo.Instance.buy_list)
            {
                ItemNode node=Home.Instance.items.Find(x=>x.n_item.id==item.id);
                price+=node.n_item.nprice*item.cnt;
            }
            if(GameBaseInfo.Instance.buy_list.Count<=0)
            {
                Toast.Instance.Show("请先选择商品");
            }
            else if(GameBaseInfo.Instance.user.balance<price)
            {
                Toast.Instance.Show("余额不足,请去充值");
            }
            else
            {

                Dialog.Instance.Show("确定使用上述信息提交订单吗", (g) =>
                {
                    NetCommand.Instance.SysnOrder(price,(msg) =>
                    {
                        Debug.Log("commit order success!");
                        Toast.Instance.Show("下单成功");
                        GameBaseInfo.Instance.buy_list.Clear();
                        Close();
                    },
                    (err) =>
                    {
                        if (!string.IsNullOrEmpty(err))
                            Debug.LogError("sysnc order data fail!");
                    });
                });
            }
        }
    }

    private void RefreshUI()
    {
        Debug.Log("username: " + PlayerPrefs.GetString("userid"));
        m_lblRegist.text = CheckLocal() ? "提交订单" : "注册 ";
        if (CheckLocal())
        {
            NetCommand.Instance.LoginUser(mUserid, (res) =>
            {
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
