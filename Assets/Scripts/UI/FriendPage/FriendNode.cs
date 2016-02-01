using UnityEngine;
using System.Collections.Generic;
using Network;
using Config;

public class FriendItem
{
    public string name;
    public string phone;
    public string orderid;
    public string statecode;
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
    int orderId;
    
    public override void Refresh()
    {
        base.Refresh();
        m_lblName.text = Data.name;
        if(Data.statecode=="1")
        {
            m_lblTel.text=Localization.Get(10078);
            m_lblGo.text = Localization.Get(10071);
            RefreshBase();
        }
        else if(Data.statecode=="2")
        {
            m_lblTel.text=Localization.Get(10077);
            m_lblGo.text = Localization.Get(10039);
        }
        else
        {
            m_lblTel.text = Localization.Get(10079);
            m_lblGo.text = string.Empty;
        }

        List<NOrder> orders = GameBaseInfo.Instance.myOrders.FindAll(x => x.state == 0);
        hasOrder = orders != null && orders.Count > 0;
        if (hasOrder)
            orderId = orders [0].id;
     
        UIEventListener.Get(m_lblGo.gameObject).onClick = Show;
    }

    private void RefreshBase()
    {
        if(Data.statecode=="1")
        {
            NetCommand.Instance.SearchFriendBase(Data.phone, (str) =>
            {
                if(str.Trim()=="20000") return;
                TBases tbase=Tables.Instance.GetTable<List<TBases>>(TableID.BASE).Find(x=>x.id.ToString()==str.Trim());
                 m_lblTel.text=string.Format(Localization.Get(10078),tbase.district+tbase.name);
            });
        }
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
        Debug.Log("has: "+hasOrder+" orderid: "+(Data.orderid != "0"));
        if (Data.statecode=="2")
        {
            // 替好友接单
            NetCommand.Instance.ReceiveFriend(Data.phone, (str) =>
            {
                Toast.Instance.Show(10072);
                m_lblGo.text= string.Empty;
                NGUITools.FindInParents<FriendPage>(gameObject).RefreshView();
            }, (err) => Toast.Instance.Show(10073));
        }
        else if (Data.statecode=="1")
        {
            if(!hasOrder)
            {
                Toast.Instance.Show(10081);
            }
            else
            {
                //发送代提请求
                NetCommand.Instance.SendFriend(Data.phone, (str) => 
                {
                    m_lblGo.text=string.Empty;
                    Toast.Instance.Show(10051);
                });
            }
        } 
    }
}
