using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;


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
            GameBaseInfo.Instance.othOrders=Util.Instance.Get<List<NOrder>>(res);
            List<LangtiItem> langtis=new List<LangtiItem>();
            foreach(var item in GameBaseInfo.Instance.othOrders)
            {
                if(item.state==0 || item.accept==GameBaseInfo.Instance.user.tel)
                {
                    LangtiItem it=new LangtiItem();
                    it.orderid=item.id;
                    it.addr=item.addr;
                    it.name=item.name;
                    it.state=item.state;
                    langtis.Add(it);
                }
            }
            if(langtis.Count>0) 
            {
                langtis.Sort((x, y) => y.state-x.state);
                m_pool.Initialize(langtis.ToArray());
            }
            else Toast.Instance.Show(10038);
        });
    }
    
    private void OnAccnt(GameObject go)
    {
        Toast.Instance.Show(10010);
    }
    
    private void OnConfirm(GameObject go)
    {
        Toast.Instance.Show(10010);
    }
}
