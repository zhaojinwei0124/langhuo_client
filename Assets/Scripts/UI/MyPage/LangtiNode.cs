using UnityEngine;
using System.Collections;

public class LangtiItem
{
    public string name;
    public string time;
}


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

        UIEventListener.Get(m_lblGo.gameObject).onClick=GoOrder;
        m_lblName.text=Data.name;
        m_lblTime.text=string.Format(Localization.Get(10037),System.DateTime.Now+new System.TimeSpan(3600));
    }



    private void GoOrder(GameObject go)
    {

    }
}
