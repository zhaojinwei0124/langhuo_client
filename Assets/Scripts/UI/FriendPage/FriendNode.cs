using UnityEngine;
using System.Collections.Generic;
using Network;
using Config;

public class FriendItem
{
    public string name;
    public string phone;
    public int code;//-1通讯录0没有1去浪尖2全局求代提
    public int bases;
}

public class FriendNode : UIPoolListNode
{

    public UILabel m_lblName;
    public UILabel m_lblTel;
    public UILabel m_lblGo;

    public FriendItem Data
    {
        get
        {
            return m_data as FriendItem;
        }
    }
    
    bool hasOrder;
    public override void Refresh()
    {
        base.Refresh();
        m_lblName.text = Data.name;
        if (Data.code == 2) //好友发状态 求代提
        {
            m_lblTel.text = Localization.Get(10077); //求带领
            m_lblGo.text = Localization.Get(10039);  //接单
        } else if (Data.code == 1) //好友要去浪尖
        {
            TBases tbase = Tables.Instance.GetTable<List<TBases>>(TableID.BASE).Find(x => x.id == Data.bases);
            m_lblTel.text = string.Format(Localization.Get(10078), tbase.district + tbase.name);   //我要去浪尖
            m_lblGo.text = Localization.Get(10071);  //发送代提请求
        } else if (Data.code == 0)
        {
            //如果好友取消代提请求 或者没有状态
            m_lblTel.text = Localization.Get(10079); //该好友暂时没有状态更新
            m_lblGo.text = Localization.Get(10071); //发送代提请求
        } else if (Data.code == -1)
        {
            m_lblTel.text = Localization.Get(10096); //通讯录中存在的好友
            m_lblGo.text = Localization.Get(10095); //发送好友邀请
        } else if (Data.code == -2)
        {
            m_lblTel.text = Localization.Get(10098); //通讯录中存在的好友
            m_lblGo.text = Localization.Get(10097); //同意加为好友
        }

        List<NOrder> orders = GameBaseInfo.Instance.myOrders.FindAll(x => x.state == 0);
        hasOrder = orders != null && orders.Count > 0 && orders.Exists(x=>x.type==1);
     
        UIEventListener.Get(m_lblGo.gameObject).onClick = Show;
    }

    private void SetSelect(bool select)
    {
        m_lblGo.color = select ? Color.red : Color.black;
        m_lblName.color = select ? Color.red : Color.black;
        m_lblTel.color = select ? Color.red : Color.black;
    }

    private void Show(GameObject go)
    {
        // Toast.Instance.Show(10036);
        if (Data.code == 2)
        {
            // 替好友接单
            NetCommand.Instance.ReceiveFriend(Data.phone, (str) =>
            {
                if (str.Trim().Equals("no_order"))
                    Toast.Instance.Show(10092);
                else
                {
                    Toast.Instance.Show(10072);
                    NGUITools.FindInParents<FriendPage>(gameObject).SyncFriends();
                }
            }, (err) => Toast.Instance.Show(10073));
        } else if (Data.code == -1)
        {
            NetCommand.Instance.RequestFriend(Data.phone,(str) =>
            {
                Toast.Instance.Show(10099);
                GameBaseInfo.Instance.UpdateAccount();
                NGUITools.FindInParents<FriendPage>(gameObject).SyncFriends();
            });
        } else if (Data.code == -2)
        {
            NetCommand.Instance.ConfirmFriend(Data.phone,(str) =>
            {
                Toast.Instance.Show(10100);
                GameBaseInfo.Instance.UpdateAccount();
                NGUITools.FindInParents<FriendPage>(gameObject).SyncFriends();
            });
        } else if (Data.code <= 1)
        {
            if (!hasOrder)
            {
                Toast.Instance.Show(10081);
            } else
            {
                //发送代提请求
                NGUITools.FindInParents<FriendPage>(gameObject).OnRight(gameObject);
            }
        } else
        {
            Toast.Instance.Show("logic error");
        }
    }
}
