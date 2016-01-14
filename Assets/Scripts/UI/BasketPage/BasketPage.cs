using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;

public class BasketPage : View
{
    public UIPoolList mPool;

    public UILabel m_lblAllPrice;

    public UILabel m_lblGO;

    public UILabel m_lblLocal;

    public UILabel m_lblChange;

    public GameObject m_objNotify;

    public GameObject m_objGo;

    public override void RefreshView()
    {
        base.RefreshView();
        RefreshTitle();
        RefreshList();
        RefreshAccount();
    }

    private void RefreshTitle()
    {
        UIEventListener.Get(m_objNotify).onClick=OnNotify;
        UIEventListener.Get(m_lblChange.gameObject).onClick=OnChange;
        m_lblLocal.text=GameBaseInfo.Instance.address.city+"  "+GameBaseInfo.Instance.address.district;
    }

    private void OnChange(GameObject go)
    {
        Toast.Instance.Show(Localization.Get(10010));
    }

    private void OnNotify(GameObject go)
    {
        Toast.Instance.Show(Localization.Get(10010));
    }

    private void RefreshList()
    {
        if(GameBaseInfo.Instance.buy_list!=null)
        {
            List<BasketItem> list=new List<BasketItem>();
            foreach(var it in GameBaseInfo.Instance.buy_list)
            {
                BasketItem item =new BasketItem();
                item.id=it.id;
                item.cnt=it.cnt;
                list.Add(item);
            }
            mPool.Initialize(list.ToArray());
        }
    }


    private void RefreshAccount()
    {
        int price=0;
        foreach(var item in GameBaseInfo.Instance.buy_list)
        {
            ItemNode node=Home.Instance.items.Find(x=>x.n_item.id==item.id);
            price+=node.n_item.nprice*item.cnt;
        }
        m_lblAllPrice.text="总计：￥"+price;
        UIEventListener.Get(m_objGo).onClick=GoBuy;
    }


    private void GoBuy(GameObject go)
    { 
        if(GameBaseInfo.Instance.buy_list.Count<=0)
        {
            Toast.Instance.Show(Localization.Get(10011));
        }
        else
        {
            UIHandler.Instance.Push(PageID.ACCOUNT);
        }
    }
}
