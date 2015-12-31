using UnityEngine;
using System.Collections.Generic;
using Network;
using GameCore;

public class HomeUI : View
{
    public UIPoolList m_pool;
    public UITexture m_txtBanner;


    public override void RefreshView()
    {
        base.RefreshView();
        RefreshBanner();
        NetCommand.Instance.GetItems((w) => 
        {
            // Debug.Log("w text: " + w.text);
            Home.Instance.Set(Util.Instance.Get<NItem[]>(w.text));
            RefreshList();
        });
    }

    private void RefreshList()
    {
        List<ItemNode> items=Home.Instance.items;
    }

    int i, seq = -1;

    private void RefreshBanner()
    {
        if (seq == -1)
        {
            TimerManager.Instance.AddTimer(2000, 0, (_seq) =>
            {
                seq = _seq;
                ResourceLoad.TextureHandler.Instance.LoadTexture("Texture/banner/banner" + i % 3, (txt) =>
                {
                    m_txtBanner.mainTexture = (txt as Texture);
                    i++;
                });
            });
        }
    }

}
