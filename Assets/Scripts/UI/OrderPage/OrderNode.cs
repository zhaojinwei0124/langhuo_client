using UnityEngine;
using System.Collections;

public class OrderNode : UIPoolListNode
{
    public UITexture m_texture;

    public UILabel m_lblDesc;

    public UILabel m_lblTime;

    public UILabel m_lblType;

    public GameObject m_objCertain;
	

    public override void Refresh()
    {
        base.Refresh();
        UIEventListener.Get(m_objCertain).onClick=OnCertainClick;
    }



    private void OnCertainClick(GameObject go)
    {
    }
}
