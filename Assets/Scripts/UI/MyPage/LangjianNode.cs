using UnityEngine;
using System.Collections.Generic;
using Network;
using System;
using Config;
using GameCore;

public class LangjianNode : UIPoolListNode
{

    public UILabel m_lblName;

    public UILabel m_lblTime;

    public UILabel m_lblOK;

    private bool m_selected=false;

    public LangtiItem Data
    {
        get
        {
            return m_data as LangtiItem;
        }
    }

    public void SetSelect(bool select)
    {
        m_selected=select;
        m_lblName.color = select ? Color.red : Color.black;
        m_lblOK.color = select ? Color.red : Color.black;
        m_lblTime.color = select ? Color.red : Color.black;
    }

    void OnClick()
    {
        Data.select=!m_selected;
        SetSelect(!m_selected);
    }


    void OnDoubleClick()
    {

        NOrder order=GameBaseInfo.Instance.othOrders.Find(x=>x.id==Data.orderid);
        if(order!=null)
        {
            string str="";
            int[] cnts =order.GetCnts();
            int[] ids =order.GetItems();
            for(int i=0;i<ids.Length;i++)
            {
                TItem item=Tables.Instance.GetTable<List<TItem>>(TableID.ITEMS).Find(x=>x.id==ids[i]);
                str+=item.name +"   X"+cnts[i]+"\n";
            }
            if(!string.IsNullOrEmpty(str))
            {
                StrText txt=new StrText(10074,str);
                UIHandler.Instance.Push(PageID.TEXT,txt);
            }
        }
        else
        {
            Debug.Log("order is null!");
        }
    }


    public override void Refresh()
    {
        base.Refresh();

        SetSelect(Data.select);

        UIEventListener.Get(m_lblOK.gameObject).onClick=OnCertain;

        m_lblName.text=Data.name;

        m_lblTime.text=string.Format(Localization.Get(10044),(DateTime.Now-new TimeSpan(UnityEngine.Random.Range(20,36000))).ToString());
    }


    private void OnCertain(GameObject go)
    {
        if (Data.state == 3)
        {
            Debug.LogError(Localization.Get(10045));
            Toast.Instance.Show(10045);
        }
        else
        {
            NetCommand.Instance.CompleteOrder(Data.orderid,  (res) =>
            {
                Toast.Instance.Show(10046);
                NGUITools.FindInParents<MyPage_langjian>(gameObject).Refresh();
            });
        }
    }
}
