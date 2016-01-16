using UnityEngine;
using System.Collections;
using Network;
using GameCore;

public class OrderItem
{
    public int itemid;

    //0代表未付款 >0表示已付款是orderid
    public int orderid;  
}

public class OrderNode : UIPoolListNode
{
    public UILabel m_lblDesc;
    public UILabel m_lblCnt;
    public GameObject m_objCertain;

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

        UIEventListener.Get(m_objCertain).onClick = OnCertainClick;

        ItemNode item = GameBaseInfo.Instance.items.Find(x => x.n_item.id == Data.itemid);

        m_lblDesc.text = item.t_item.description;

        m_lblCnt.text = string.Format(Localization.Get(10032), item.n_item.cnt);

    }

    private void OnCertainClick(GameObject go)
    {
        if (Data.orderid == 0)
        {
            UIHandler.Instance.Push(PageID.ACCOUNT);
        } else
        {
            NetCommand.Instance.UpdateOder(Data.orderid, (sr) => 
            {
                Toast.Instance.Show(10035);
            },
            (err) => 
            {
                Toast.Instance.Show(err);
            });
        }
    }
}
