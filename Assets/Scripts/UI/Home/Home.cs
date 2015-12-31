using UnityEngine;
using System.Collections;
using Network;


public class Home : Single<Home>
{

    private NItem[] n_items;


    public void Set(NItem[] _items)
    {
        if(_items==null)
            Debug.LogError("items is null");
        n_items=_items;
    }



}
