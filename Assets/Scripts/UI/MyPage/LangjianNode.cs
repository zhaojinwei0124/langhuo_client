using UnityEngine;
using System.Collections.Generic;
using Network;
using System;

public class LangjianNode : UIPoolListNode
{

    public UILabel m_lblName;

    public UILabel m_lblTime;

    public GameObject m_objOK;


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

        UIEventListener.Get(m_objOK).onClick=OnCertain;

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
