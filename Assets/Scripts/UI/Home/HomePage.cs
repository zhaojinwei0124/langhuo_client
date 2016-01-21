using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;
using Platform;

public class HomePage : View
{
    public UIPoolList m_pool;
    public UITexture m_txtBanner;
    public UILabel m_lblCity;
    public UILabel m_lbllDistric;
    public GameObject m_objType;
    private List<HomeScrollData> last_data;

    public override void RefreshView()
    {
        base.RefreshView();
        RefreshBanner();
        LocationManager.Instance.locationHandler += RefreshTitle;

        UIEventListener.Get(m_objType).onClick=OnTypeClick;

        NetCommand.Instance.GetItems((res) => 
        {
            Home.Instance.Set(Util.Instance.Get<NItem[]>(res));
            RefreshList();
        });
    }

    protected override void Close()
    {
        LocationManager.Instance.locationHandler -= RefreshTitle;
        base.Close();
    }

    public void RefreshTitle()
    {
        m_lblCity.text = GameBaseInfo.Instance.address.city;
        m_lbllDistric.text = GameBaseInfo.Instance.address.district + Localization.Get(10024);
    }

    private void RefreshList()
    {
        List<ItemNode> items = Home.Instance.items;
        List<HomeScrollData> datas = new List<HomeScrollData>();
        for (int i=0; i<items.Count/2; i++)
        {
            HomeScrollData data = new HomeScrollData(items [2 * i], items.Count - 1 >= 2 * i + 1 ? items [2 * i + 1] : null);
            datas.Add(data);
        }
        for (int i=0; i<2; i++)
        {
            HomeScrollData data = new HomeScrollData(items [0], items [1]);
            datas.Add(data);
        }
        if (datas != null && datas.Count > 0)
        {
            if (last_data == null || !Home.Instance.Compare(last_data, datas))
            {
                last_data = datas;
                m_pool.Initialize(datas.ToArray());
            }
        } else
        {
            Debug.LogError("datas is null!");
        }
    }

   
    private void OnTypeClick(GameObject go)
    {
        UIHandler.Instance.Push(PageID.TYPE);
    }


    int i, seq = -1;

    private void RefreshBanner()
    {
        if (seq == -1)
        {
            TimerManager.Instance.AddTimer(2000, 0, (_seq) =>
            {
                seq = _seq;
                ResourceLoad.TextureHandler.Instance.LoadTexture("banner/banner" + i % 3, (txt) =>
                {
                    m_txtBanner.mainTexture = (txt as Texture);
                    i++;
                });
            });
        }
    }


}
