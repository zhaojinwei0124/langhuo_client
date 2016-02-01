using UnityEngine;
using System.Collections.Generic;
using Network;
using Config;
using GameCore;


public class LangtiItem
{
    public int orderid;
    public string name;
    public string time;
    public string addr;
    public int state;
    public bool select;
    public int val;
};

public class LangtiNode : UIPoolListNode
{
    public UILabel m_lblName;
    public UILabel m_lblTime;
    public UILabel m_lblGo;

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
        m_lblGo.color = select ? Color.red : Color.black;
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
        UIEventListener.Get(m_lblGo.gameObject).onClick = GoOrder;
        m_lblName.text = string.Format(Localization.Get(10041), Data.name);
        m_lblTime.text = string.Format(Localization.Get(10037), System.DateTime.Now + new System.TimeSpan(Random.Range(20, 3600)), Data.addr);
        m_lblGo.text = Data.state == 0 ? Localization.Get(10039) : Localization.Get(10040);
    }

    private void GoOrder(GameObject go)
    {
        if (Data.state == 1)
            Toast.Instance.Show(10042);
        else
        {
            NetCommand.Instance.UpdateOder(new int[]{ Data.orderid}, GameBaseInfo.Instance.user.tel, (res) =>
            {
                Toast.Instance.Show(10043);
                NGUITools.FindInParents<MyPage_langti>(gameObject).Refresh();
            });
        }
    }
}
