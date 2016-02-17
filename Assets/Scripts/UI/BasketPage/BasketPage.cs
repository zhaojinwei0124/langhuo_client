using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;
using Platform;
using Config;

public class BasketPage : View
{
    public UIPoolList mPool;

    public UILabel m_lblAllPrice;

    public UILabel m_lblGO;

    public UILabel m_lblLocal;

    public UILabel m_lblChange;

    public GameObject m_objNotify;

    public GameObject m_objGo;

    const int BASECODE = 200000;


    public override void RefreshView()
    {
        base.RefreshView();
        RefreshTitle();
        RefreshList();
        RefreshAccount();
        RefreshLocal();
    }

    protected override void Close()
    {
        LocationManager.Instance.locationHandler-=RefreshGPS;
        base.Close();
    }

    private void RefreshTitle()
    {
        LocationManager.Instance.locationHandler+=RefreshGPS;
        UIEventListener.Get(m_objNotify).onClick=OnNotify;
        UIEventListener.Get(m_lblChange.gameObject).onClick=OnChange;
        RefreshLocal();
        RefreshGPS();
    }

    private void RefreshGPS()
    {
        if (!PlayerPrefs.HasKey(PlayerprefID.BASE) || PlayerPrefs.GetInt(PlayerprefID.BASE,BASECODE) == BASECODE)
        {
            m_lblLocal.text =GameBaseInfo.Instance.address.city+" "+ GameBaseInfo.Instance.address.district;
        }
    }

    private void RefreshLocal()
    {
        int m_base =BASECODE;
        if (GameBaseInfo.Instance.user == null)
        {
            m_base = PlayerPrefs.GetInt(PlayerprefID.BASE, BASECODE);
        } else
        {
            m_base = GameBaseInfo.Instance.user.bases;
            PlayerPrefs.SetInt(PlayerprefID.BASE,m_base);
        }
        if(m_base!=BASECODE)
        {
            TBases _base = Tables.Instance.GetTable<List<TBases>>(TableID.BASE).Find(x => x.id == m_base);
            m_lblLocal.text = _base.district + _base.name;
        }
        else 
        {
            m_lblLocal.text =GameBaseInfo.Instance.address.city+" "+ GameBaseInfo.Instance.address.district;
        }
    }

    private void OnChange(GameObject go)
    {
       // Toast.Instance.Show(Localization.Get(10010));
        UIHandler.Instance.Push(PageID.BASE);
    }

    private void OnNotify(GameObject go)
    {
        Toast.Instance.Show(Localization.Get(10053));
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
        m_lblAllPrice.text="总计：￥"+GameBaseInfo.Instance.GetPaycnt();
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
            AccountPage.showOrderBtn=true;
            UIHandler.Instance.Push(PageID.ACCOUNT);
        }
    }
}
