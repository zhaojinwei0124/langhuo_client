using UnityEngine;
using System.Collections;
using Network;

public class MyPage_langti : MonoSingle<MyPage_langti>
{
    public UIPoolList m_pool;
    public GameObject m_objAccnt;
    public GameObject m_objOK;

    public void Refresh()
    {
        UIEventListener.Get(m_objOK).onClick = OnConfirm;
        UIEventListener.Get(m_objAccnt).onClick = OnAccnt;
        RefreshList();
    }

    private void RefreshList()
    {
        NetCommand.Instance.GetOders(GameBaseInfo.Instance.user.tel, (res) =>
        {
        });
    }
    
    private void OnAccnt(GameObject go)
    {
    }
    
    private void OnConfirm(GameObject go)
    {
        
    }
}
