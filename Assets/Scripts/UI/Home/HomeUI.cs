using UnityEngine;
using System.Collections;
using Network;
using GameCore;

public class HomeUI : View
{
    public UIPoolList m_pool;
    public UITexture m_txtBanner;
    private NItem[] n_items;

    public override void RefreshView()
    {
        base.RefreshView();
        RefreshBanner();
        NetCommand.Instance.GetItems((w) => 
        {
            // Debug.Log("w text: " + w.text);
            n_items = Util.Instance.Get<NItem[]>(w.text);
            Debug.Log("items length: " + n_items.Length);
            if (n_items != null)
            {
//                foreach (var item in n_items)
//                {
//                    Debug.Log("item id: " + item.id + " cnt:" + item.cnt + " nprice: " + item.nprice + " pprice: " + item.pprice);
//                }

                RefreshList();
            } else
            {
                Debug.LogError("net work is error!");
            }
        });
    }

    private void RefreshList()
    {
    }

    int i, seq = -1;

    private void RefreshBanner()
    {
        Debug.Log("refresh nann");
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
