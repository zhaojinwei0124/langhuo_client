using UnityEngine;
using System.Collections.Generic;
using Network;

public class FriendItem
{
    public string name;
    public string phone;
    public string orderid;
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
        m_lblTel.text = Data.phone;
        List<NOrder> orders = GameBaseInfo.Instance.myOrders.FindAll(x => x.state == 0);
        hasOrder = orders != null && orders.Count > 0;
        if (hasOrder)
            orderId = orders [0].id;
        if (Data.orderid != "0")
        {
            m_lblGo.text = Localization.Get(10039);
        } else if (hasOrder)
        {
            m_lblGo.text = Localization.Get(10071);
        } else
        {
            m_lblGo.text = string.Empty;
        }
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
        Debug.Log("has: "+hasOrder+" orderid: "+(Data.orderid != "0"));
        if (Data.orderid != "0")
        {
            NetCommand.Instance.ReceiveFriend(Data.orderid, (str) =>
            {
                Toast.Instance.Show(10072);
                m_lblGo.text= hasOrder?Localization.Get(10071): string.Empty;
                NGUITools.FindInParents<FriendPage>(gameObject).RefreshView();
            }, (err) => Toast.Instance.Show(10073));
        }
        else if (hasOrder)
        {
            NetCommand.Instance.SendFriend(orderId, Data.phone, (str) => 
            {
                m_lblGo.text=string.Empty;
                Toast.Instance.Show(10051);
            });
        } 
    }
}
