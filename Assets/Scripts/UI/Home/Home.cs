using UnityEngine;
using System.Collections.Generic;
using Network;
using Config;

/// <summary>
/// Homeui parse class
/// </summary>
/// 
public class ItemNode
{
    public NItem n_item;
    public TItem t_item;
    
    public ItemNode(NItem _n, TItem _t)
    {
        n_item = _n;
        t_item = _t;
    }
};

public class Home : Single<Home>
{

    public List<ItemNode> items;

    public void Set(NItem[] _items)
    {
        if (_items == null)
            Debug.LogError("items is null");
       
        NItem[] n_items = _items;

        if (items == null)
            items = new List<ItemNode>();
        items.Clear();
        foreach (var item in _items)
        {
            TItem titem = Tables.Instance.GetTable<List<TItem>>(TableID.ITEMS).Find(x => x.id == item.id);
            ItemNode node = new ItemNode(item, titem);
            items.Add(node);
        }

    }


}
