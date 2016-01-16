using UnityEngine;
using System.Collections;
using Network;
using GameCore;
using ResourceLoad;

public class AccountPage : View
{
    public UITexture m_head;
    public UIInput m_user;
    public UILabel m_type;
    public UIPopupList m_poplist;
    public UIInput m_receive;
    public UIInput m_address;
    public UIInput m_tel;
    public UILabel m_balance;
    public UILabel m_paycnt;
    public GameObject m_objRegist;
    public UILabel m_lblRegist;
    private string mUserid;
    private PayType payType = PayType.LANGJIAN;

    public override void RefreshView()
    {
        base.RefreshView();
        Debug.Log("refresh..");
        UIEventListener.Get(m_objRegist).onClick = OnCommit;
        UIEventListener.Get(m_head.gameObject).onClick = OnTextClick;
        RefreshUI();
    }

    public void OnPopChange()
    {
        m_type.text = m_poplist.GetSelect();
        payType = (PayType)(m_poplist.GetIndex() + 1);
        //  Debug.Log("index: "+m_poplist.GetIndex()+" type: "+payType);
    }

    private bool CheckLocal()
    {
        mUserid = PlayerPrefs.GetString(PlayerprefID.USERID, null);
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
        UIHandler.Instance.Push(PageID.HEADICON);
    }

    private void OnCommit(GameObject go)
    {
        if (!CheckValid())
            return;

        if (!CheckLocal())
        {
            Toast.Instance.Show(10018);
          
        } else
        {
            int price = 0;
            foreach (var item in GameBaseInfo.Instance.buy_list)
            {
                ItemNode node = Home.Instance.items.Find(x => x.n_item.id == item.id);
                price += node.n_item.nprice * item.cnt;
            }
            if (GameBaseInfo.Instance.buy_list.Count <= 0)
            {
                Toast.Instance.Show(Localization.Get(10002));
            } else if (GameBaseInfo.Instance.user.balance < price)
            {
                Toast.Instance.Show(Localization.Get(10003));
            } else
            {

                Dialog.Instance.Show(Localization.Get(10004), (g) =>
                {
                    NetCommand.Instance.SysnOrder(price, (msg) =>
                    {
                        Debug.Log("commit order success!");
                        Toast.Instance.Show(Localization.Get(10005));
                        GameBaseInfo.Instance.ClearBuy();
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
        Debug.Log("refresh ui");
        m_lblRegist.text = Localization.Get(10006);
        if (CheckLocal())
        {
            NetCommand.Instance.LoginUser(mUserid, (res) =>
            {
                NUser nuser = GameCore.Util.Instance.Get<NUser>(res);
                GameBaseInfo.Instance.user = nuser;
                m_user.label.text = m_receive.label.text = nuser.name;
                m_type.text = nuser.type == 1 ? Localization.Get(10008) : Localization.Get(10009);
                m_address.label.text = nuser.addr;
                m_tel.label.text = nuser.tel.ToString();
                m_balance.text = Localization.Get(10031) + string.Format("{0:f}", GameBaseInfo.Instance.user.balance);
                m_paycnt.text = Localization.Get(10031) + string.Format("{0:f}", GameBaseInfo.Instance.GetPaycnt());
                TextureHandler.Instance.LoadHeadTexture(PlayerPrefs.GetString(PlayerprefID.HEADICO, "010"),
                (txt) =>
                {
                    m_head.mainTexture = txt as Texture2D;
                });
            });
        } else
        {
            UIHandler.Instance.Push(PageID.LOGIN);
        }

    }

}
