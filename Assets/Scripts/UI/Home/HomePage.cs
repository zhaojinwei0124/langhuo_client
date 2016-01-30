using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;
using Platform;
using Config;

public class HomePage : View
{
    public UIPoolList m_pool;
    public UITexture m_txtBanner;
    public UILabel m_lblCity;
    public UILabel m_lbllDistric;
    public GameObject m_objType;
    private List<HomeScrollData> last_data;
    const int BASECODE = 20000;

    public override void RefreshView()
    {
        base.RefreshView();
        RefreshBanner();
        LocationManager.Instance.locationHandler += RefreshGPS;
        UIEventListener.Get(m_lblCity.gameObject).onClick=OnLocalClick;
        UIEventListener.Get(m_objType).onClick = OnTypeClick;
        RefreshTitle();
        NetCommand.Instance.GetItems((res) => 
        {
            Home.Instance.Set(Util.Instance.Get<NItem[]>(res));
            RefreshList();
        });
    }

    protected override void Close()
    {
        LocationManager.Instance.locationHandler -= RefreshGPS;
        base.Close();
    }

    private void RefreshGPS()
    {
        if (!PlayerPrefs.HasKey(PlayerprefID.BASE))
        {
            m_lblCity.text=GameBaseInfo.Instance.address.city;
            m_lbllDistric.text = string.Format(Localization.Get(10024),GameBaseInfo.Instance.address.district);
        }
    }

    public void RefreshTitle()
    {
        int m_base = BASECODE;
        if (GameBaseInfo.Instance.user == null)
        {
            m_base = PlayerPrefs.GetInt(PlayerprefID.BASE, BASECODE);
        } else
        {
            m_base = GameBaseInfo.Instance.user.bases;
            PlayerPrefs.SetInt(PlayerprefID.BASE,m_base);
        }
        if(m_base != BASECODE)
        {
            TBases _base = Tables.Instance.GetTable<List<TBases>>(TableID.BASE).Find(x => x.id == m_base);
            m_lbllDistric.text = _base.district + _base.name;
            m_lblCity.text=_base.city;
        }
    }

    private void RefreshList()
    {
        List<ItemNode> items = Home.Instance.items;
        List<HomeScrollData> datas = new List<HomeScrollData>();

        int filter = Home.Instance.type;
        if (filter > 0)
            items = items.FindAll(x => x.t_item.type == filter);

        for (int i=0; i<(items.Count+1)/2; i++)
        {
            HomeScrollData data = new HomeScrollData(items [2 * i], items.Count - 1 >= 2 * i + 1 ? items [2 * i + 1] : null);
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
            Toast.Instance.Show(10065);
            Debug.LogError("datas is null!");
        }
    }


    private void OnLocalClick(GameObject go)
    {
        UIHandler.Instance.Push(PageID.BASE);
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
