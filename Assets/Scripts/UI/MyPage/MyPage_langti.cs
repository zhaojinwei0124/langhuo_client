using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;

public class MyPage_langti : MonoSingle<MyPage_langti>
{
    public UIPoolList m_pool;
    public GameObject m_objAccnt;
    public GameObject m_objOK;
    public GameObject m_objReady;
    private  List<LangtiItem> langtis = new List<LangtiItem>();

    public void Refresh()
    {
        UIEventListener.Get(m_objOK).onClick = OnConfirm;
        UIEventListener.Get(m_objAccnt).onClick = OnAccnt;
        UIEventListener.Get(m_objReady).onClick = OnReady;
        RefreshList();
    }

    private void RefreshList()
    {
        NetCommand.Instance.GetOders(GameBaseInfo.Instance.user.tel, (res) =>
        {
            langtis.Clear();
            GameBaseInfo.Instance.othOrders = Util.Instance.Get<List<NOrder>>(res);
            foreach (var item in GameBaseInfo.Instance.othOrders)
            {
                if (item.state == 0 || item.accept == GameBaseInfo.Instance.user.tel)
                {
                    LangtiItem it = new LangtiItem();
                    it.orderid = item.id;
                    it.addr = item.addr;
                    it.name = item.name;
                    it.state = item.state;
                    it.val = item.val;
                    it.time=item.rcvTime;
                    it.select = false;
                    langtis.Add(it);
                }
            }
            if (langtis.Count > 0)
            {
                langtis.Sort((x, y) => y.state - x.state);
                m_pool.Initialize(langtis.ToArray());
            } else
                Toast.Instance.Show(10038);
        });
    }
    
    private void OnAccnt(GameObject go)
    {
        List<NItem> nitems = new List<NItem>();
        if (langtis.Count > 0)
        {
            List<LangtiItem> langitems = langtis.FindAll(x => x.state == 1);
            foreach (var item in langtis)
            {
                NOrder order = GameBaseInfo.Instance.othOrders.Find(x => x.id == item.orderid);
                if (order != null)
                {
                    int[] items = order.GetItems();
                    int[] cnts = order.GetCnts();
                    for (int i=0; i<items.Length; i++)
                    {
                        NItem it = nitems.Find(x => x.id == items [i]);
                        if (it != null)
                        {
                            it.cnt += cnts [i];
                        } else
                        {
                            NItem n = new NItem();
                            n.id = items [i];
                            n.cnt = cnts [i];
                            nitems.Add(n);
                        }
                    }
                }
            }

            string content = string.Empty;
            for (int i=0; i<nitems.Count; i++)
            {
                content += GameBaseInfo.Instance.items.Find(x => x.n_item.id == nitems [i].id).t_item.name + " X" + nitems [i].cnt + "\n";
            }
            UIHandler.Instance.Push(PageID.TEXT, new StrText(10057, string.Format(Localization.Get(10058), content)));
        } else
        {
            Toast.Instance.Show(10059);
        }

    }

    private void OnConfirm(GameObject go)
    {
        List<LangtiItem> items = langtis.FindAll(x => x.select == true);
        if (items != null && items.Count > 0)
        {
            NetCommand.Instance.UpdateOder(items.ConvertAll(x => x.orderid).ToArray(), GameBaseInfo.Instance.user.tel,
               (res) =>
            {
                Toast.Instance.Show(10043);
                RefreshList();
            });
        } else
        {
            Debug.Log("items is null: " + (items == null));
            if (items != null)
                Debug.Log("select cnt: " + items.Count);
            Toast.Instance.Show(10062);
        }
    }

    private void OnReady(GameObject go)
    {
        List<LangtiItem> items = langtis.FindAll(x => x.select == true);
        if (items != null && items.Count > 0)
        {
            string orderid = Util.Instance.SerializeArray(items.ConvertAll(x => x.orderid));
            NetCommand.Instance.SortOders(orderid, (res) =>
            {
                Toast.Instance.Show(10061);
                RefreshList();
            });
        } else
        {
            Debug.Log("items is null: " + (items == null));
            if (items != null)
                Debug.Log("select cnt: " + items.Count);
            Toast.Instance.Show(10062);
        }
    }
}
