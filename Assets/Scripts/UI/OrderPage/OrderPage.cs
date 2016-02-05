using UnityEngine;
using System.Collections;
using Network;
using System.Collections.Generic;
using GameCore;

public class OrderPage : View
{
    public UIPoolList m_pool;

    public GameObject m_objConfirm;

    public GameObject m_objCount;

    private List<OrderItem> items=new List<OrderItem>();

    public override void RefreshView()
    {
        base.RefreshView();

        UIEventListener.Get(m_objConfirm).onClick=OnConfirm;

        UIEventListener.Get(m_objCount).onClick=OnAccount;

        RefreshList();
    }


    public void RefreshList()
    {
        if(items!=null) items.Clear();
        List<NOrder> orders=GameBaseInfo.Instance.myOrders.FindAll(x=>x.state==0||x.state==1);
        for(int i=0;i<orders.Count;i++)
        {
            OrderItem item =new OrderItem();
            item.cnts=orders[i].GetCnts();
            item.itemids=orders[i].GetItems();
            item.orderid=orders[i].id;
            item.state=orders[i].state;
            items.Add(item);
        }

        List<GameBaseInfo.BuyNode> buys=GameBaseInfo.Instance.buy_list;
        if(buys.Count>0)
        {
            OrderItem item =new OrderItem();
            item.orderid=0;
            item.itemids=buys.ConvertAll<int>(x=>x.id).ToArray();
            item.cnts=buys.ConvertAll<int>(x=>x.cnt).ToArray();
            items.Add(item);
        }
        m_pool.Initialize(items.ToArray());
    }



    private void OnConfirm(GameObject go)
    {
        Toast.Instance.Show(10010);
    }


    private void OnAccount(GameObject go)
    {
        List<NItem> nitems=new List<NItem>();
        for(int i=0;i<items.Count;i++)
        {
            for(int j=0;j<items[i].cnts.Length;j++)
            {
                NItem n=new NItem();
                n.id=items[i].itemids[j];
                n.cnt=items[i].cnts[j];
                NItem y=nitems.Find(x=>x.id==n.id);
                if(y!=null)
                {
                    y.cnt+=n.cnt;
                }
                else
                {
                    nitems.Add(n);
                }
            }
        }
        string content=string.Empty;
        for(int i=0;i<nitems.Count;i++)
        {
            content+=GameBaseInfo.Instance.items.Find(x=>x.n_item.id==nitems[i].id).t_item.name+" X"+nitems[i].cnt+"\n";
        }
        UIHandler.Instance.Push(PageID.TEXT,new StrText(10057,string.Format(Localization.Get(10058),content)));
    }
}
