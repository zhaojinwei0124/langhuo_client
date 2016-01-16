using UnityEngine;
using System.Collections;

public class LangjianItem
{

}



public class LangjianNode : UIPoolListNode
{

    public UILabel m_lblName;

    public UILabel m_lblTime;

    public GameObject m_objOK;



    public LangjianItem Data
    {
        get
        {
            return m_data as LangjianItem;
        }
    }


    public override void Refresh()
    {
        base.Refresh();


    }
}
