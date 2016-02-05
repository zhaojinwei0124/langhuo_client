using UnityEngine;
using System.Collections.Generic;
using Network;
using Config;

public class FriendItem
{
    public string name;
    public string phone;
    public int code;
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
    int orderId;
    
    public override void Refresh()
    {
        base.Refresh();
        m_lblName.text = Data.name;
        Debug.Log("phone: "+Data.phone+" code: "+Data.code);
        if(Data.code==2) //好友发状态 求代提 
        {
            m_lblTel.text = Localization.Get(10077); //求带领
            m_lblGo.text = Localization.Get(10039);  //接单
        }
        else if(Data.code==1) //好友要去浪尖
        {
            TBases tbase=Tables.Instance.GetTable<List<TBases>>(TableID.BASE).Find(x=>x.id==Data.bases);
            m_lblTel.text= string.Format(Localization.Get(10078),tbase.district+tbase.name);   //我要去浪尖
            m_lblGo.text = Localization.Get(10071);  //发送代提请求
        }
        else
        {
            //如果好友取消代提请求 或者没有状态
            m_lblTel.text =  Localization.Get(10079); //该好友暂时没有状态更新
            m_lblGo.text =  Localization.Get(10071); //发送代提请求
        }

        List<NOrder> orders = GameBaseInfo.Instance.myOrders.FindAll(x => x.state == 0);
        hasOrder = orders != null && orders.Count > 0;
        if (hasOrder)
            orderId = orders [0].id;
     
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
        if (Data.code==2)
        {
            // 替好友接单
            NetCommand.Instance.ReceiveFriend(Data.phone, (str) =>
            {
                if(str.Trim().Equals("no_order")) Toast.Instance.Show(10092);
                else
                {
                    Toast.Instance.Show(10072);
                    NGUITools.FindInParents<FriendPage>(gameObject).SyncFriends();
                }
            }, (err) => Toast.Instance.Show(10073));
        }
        else if (Data.code==1)
        {
            if(!hasOrder)
            {
                Toast.Instance.Show(10081);
            }
            else
            {
                //发送代提请求
                NGUITools.FindInParents<FriendPage>(gameObject).OnRight(gameObject);
            }
        } 
    }
}
