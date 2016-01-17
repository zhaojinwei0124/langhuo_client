using UnityEngine;
using System.Collections;
using Network;
using System.Collections.Generic;


public class OrderPage : View
{
    public UIPoolList m_pool;

    public GameObject m_objConfirm;

    public GameObject m_objCount;


    public override void RefreshView()
    {
        base.RefreshView();

        UIEventListener.Get(m_objConfirm).onClick=OnConfirm;

        UIEventListener.Get(m_objCount).onClick=OnAccount;

        RefreshList();
    }


    private void RefreshList()
    {
        List<OrderItem> items=new List<OrderItem>();
        List<NOrder> orders=GameBaseInfo.Instance.myOrders.FindAll(x=>x.state==0);
        for(int i=0;i<orders.Count;i++)
        {
            OrderItem item =new OrderItem();
            item.cnts=orders[i].GetCnts();
            item.itemids=orders[i].GetItems();
            item.orderid=orders[i].id;
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
        Toast.Instance.Show(10010);
    }
}
