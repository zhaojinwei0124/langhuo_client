using UnityEngine;
using System.Collections;
using Network;

public class LangtiItem
{
    public int orderid;
    public string name;
    public string time;
    public string addr;
    public int state;
};

public class LangtiNode : UIPoolListNode
{
    public UILabel m_lblName;
    public UILabel m_lblTime;
    public UILabel m_lblGo;

    public LangtiItem Data
    {
        get
        {
            return m_data as LangtiItem;
        }
    }

    public override void Refresh()
    {
        base.Refresh();

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
            NetCommand.Instance.UpdateOder(Data.orderid, GameBaseInfo.Instance.user.tel, (res) =>
            {
                Toast.Instance.Show(10043);
                NGUITools.FindInParents<MyPage_langti>(gameObject).Refresh();
            });
        }
    }
}
