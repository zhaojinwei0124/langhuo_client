using UnityEngine;
using System.Collections;
using Network;
using GameCore;

public class HomeUI : View
{
    public UIPoolList m_pool;


    private NItem[] n_items;

    void Awake(){}

    public override void RefreshView()
    {
        base.RefreshView();
        NetCommand.Instance.GetItems((w) => 
        {
           // Debug.Log("w text: " + w.text);
            n_items = Util.Instance.Get<NItem[]>(w.text);
            Debug.Log("items length: " + n_items.Length);
            if (n_items != null)
            {
                foreach (var item in n_items)
                {
                    Debug.Log("item id: " + item.id + " cnt:" + item.cnt + " nprice: " + item.nprice + " pprice: " + item.pprice);
                }

                RefreshList();
            } else
            {
                Debug.LogError("net work is error!");
            }
        });
    }

    void Refresh()
    {

    }


    private void RefreshList()
    {
    }
    
    
}
