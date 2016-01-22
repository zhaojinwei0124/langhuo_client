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


    public int type = 0;

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

        GameBaseInfo.Instance.items=items;

    }




    public bool Compare(List<HomeScrollData> item1,List<HomeScrollData> item2)
    {
        if(item1.Count!=item2.Count) return false;
        for(int i=0;i<item1.Count;i++)
        {
            if(item1[i].item1.n_item.id!=item2[i].item1.n_item.id)
            {
                return false;
            }
        }
        return true;
    }


}
