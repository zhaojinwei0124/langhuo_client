using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;

public class OrderItem
{
    public int[] itemids;
    public int[] cnts;
    public NOrder noder;//null代表未付款  
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

        m_lblOK.text = Data.noder ==null ? Localization.Get(10033) + "      " : m_poplist.items[Data.noder.type] + "      ";

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
        TimerManager.Instance.AddTimer(100, (seq) =>{
            if (Data.noder != null)
            {
                m_lblOK.text = m_poplist.GetSelect();
                pickType = (PickType)m_poplist.GetIndex();
               
                Debug.Log("pickType: " + pickType);

                Dialog.Instance.Show(10085, () =>
                {
                    NetCommand.Instance.StateOder(Data.noder.id.ToString(), (int)pickType, (str) =>
                    {
                        if (pickType == PickType.CANCEL)
                            Toast.Instance.Show(10086);
                        else
                            Toast.Instance.Show(10087);
                        GameBaseInfo.Instance.UpdateOrders((success) =>
                                                               NGUITools.FindInParents<OrderPage>(gameObject).RefreshList());
                    });
                }, () =>
                {
                    m_lblOK.text = m_poplist.items [0];
                    pickType = PickType.SELF;
                });
            }
        });
    }

    private void OnCertainClick(GameObject go)
    {
        if (Data.noder == null)
        {
            AccountPage.showOrderBtn = true;
            UIHandler.Instance.Push(PageID.ACCOUNT);
        } else
        {
            m_poplist.SendMessage("OnClick");
        }
    }
}
