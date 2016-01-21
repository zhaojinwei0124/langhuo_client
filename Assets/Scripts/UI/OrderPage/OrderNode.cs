﻿using UnityEngine;
using System.Collections;
using Network;
using GameCore;

public class OrderItem
{
    public int[] itemids;
    public int[] cnts;

    //0代表未付款 >0表示已付款是orderid
    public int orderid;  
}

public class OrderNode : UIPoolListNode
{
    public UILabel m_lblDesc;
    public UILabel m_lblCnt;
    public UILabel m_lblOK;
    public UIPopupList m_poplist;
    private PickType pickType = PickType.SELF;

    public OrderItem Data
    {
        get
        {
            return m_data as OrderItem;
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        UIEventListener.Get(m_lblOK.gameObject).onClick = OnCertainClick;

        m_lblDesc.text = GetDesc();

        m_lblOK.text = Data.orderid <= 0 ? Localization.Get(10033) +"      ": Localization.Get(10034) +"      ";
    }

    private string GetDesc()
    {
        string str = null;
        for (int i=0; i<Data.itemids.Length; i++)
        {
            ItemNode item = GameBaseInfo.Instance.items.Find(x => x.n_item.id == Data.itemids [i]);
            string name = item.t_item.name;
            str += name + " X" + Data.cnts [i];
            if (i < Data.itemids.Length - 1)
                str += "\n";
        }
        return str;
    }

    public void OnPopChange()
    {
        if (Data.orderid > 0)
        {
            m_lblOK.text = m_poplist.GetSelect();
            pickType = (PickType)m_poplist.GetIndex();
        }
    }

    private void OnCertainClick(GameObject go)
    {
        if (Data.orderid == 0)
        {
            UIHandler.Instance.Push(PageID.ACCOUNT);
        } else
        {
            m_poplist.SendMessage("OnClick");
        }
    }
}
