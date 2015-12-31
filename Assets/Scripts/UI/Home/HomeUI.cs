using UnityEngine;
using System.Collections;
using Network;
using GameCore;

public class HomeUI : MonoBehaviour
{


    [SerializeField]
    private UIPoolList m_pool;
    private NItem[] n_items;

    void Awake()
    {
    }

    void Start()
    {
        NetCommand.Instance.GetItems((w) => 
        {
            Debug.Log("w text: " + w.text);
            n_items = Util.Instance.Get<NItem[]>(w.text);
            Debug.Log("items length: " + n_items.Length);
            if (n_items != null)
            {
                foreach (var item in n_items)
                {
                    Debug.Log("item id: " + item.id + " cnt:" + item.cnt + " nprice: " + item.nprice + " pprice: " + item.pprice);
                }
            } else
            {
                Debug.LogError("net work is error!");
            }
        });
    }

    private void Refresh()
    {
    }
    
    
}
