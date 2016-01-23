using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;

public class MyPage_langjian : MonoSingle<MyPage_langjian>
{
   
    public UIPoolList m_pool;
    public GameObject m_objAccnt;
    public GameObject m_objOK;

    private  List<LangtiItem> langjis = new List<LangtiItem>();

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
            GameBaseInfo.Instance.othOrders = Util.Instance.Get<List<NOrder>>(res);
            langjis.Clear();
            foreach (var item in GameBaseInfo.Instance.othOrders)
            {
                if (item.state < 3)
                {
                    LangtiItem it = new LangtiItem();
                    it.orderid = item.id;
                    it.addr = item.addr;
                    it.name = item.name;
                    it.state = item.state;
                    it.val=item.val;
                    it.select = false;
                    langjis.Add(it);
                }
            }
            if (langjis.Count > 0)
            {
                langjis.Sort((x, y) => y.val - x.val);
                m_pool.Initialize(langjis.ToArray());
            } else
                Toast.Instance.Show(10038);
        });
    }

    private void OnAccnt(GameObject go)
    {
        if (GameBaseInfo.Instance.othOrders != null && GameBaseInfo.Instance.othOrders.Count>0)
        {
            List<NItem> list = new List<NItem>();
            foreach(var it in GameBaseInfo.Instance.othOrders)
            {
                int[] items=it.GetItems();
                int[] cnts=it.GetCnts();
                for(int i=0;i<items.Length;i++)
                {
                    NItem ni=new NItem();
                    ni.id=items[i];
                    ni.cnt=cnts[i];
                    NItem y=list.Find(x=>x.id==ni.id);
                    if(y!=null)
                    {
                        y.cnt+=ni.cnt;
                    }
                    else
                    {
                        list.Add(ni);
                    }
                }
            }
            string content=string.Empty;
            for(int i=0;i<list.Count;i++)
            {
                content+=GameBaseInfo.Instance.items.Find(x=>x.n_item.id==list[i].id).t_item.name+" X"+list[i].cnt+"\n";
            }
            UIHandler.Instance.Push(PageID.TEXT,new StrText(10057,string.Format(Localization.Get(10058),content)));
        }
        else
        {
            Toast.Instance.Show(10059);
        }
    }

    private void OnConfirm(GameObject go)
    {
        Toast.Instance.Show(10010);
    }
}
