using UnityEngine;
using System.Collections;

public class LangtiItem
{

}


public class LangtiNode : UIPoolListNode 
{

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
    }
}
