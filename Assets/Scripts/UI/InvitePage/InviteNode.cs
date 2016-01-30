using UnityEngine;
using System.Collections;

public class InviteNode : UIPoolListNode
{

    public UILabel m_lblName;
    public UILabel m_lblTel;
    public GameObject m_objGo;
    
    
    public FriendItem Data
    {
        get
        {
            return m_data as FriendItem;
        }
    }
    
    
    public override void Refresh()
    {
        base.Refresh();
        m_lblName.text=Data.name;
        m_lblTel.text=Data.phone;
        
        UIEventListener.Get(m_objGo).onClick=Show;
    }
    
    
    private void Show(GameObject go)
    {
       // Toast.Instance.Show(10051);
        SDKManager.Instance.WeixinMessageShare("");
    }
}
