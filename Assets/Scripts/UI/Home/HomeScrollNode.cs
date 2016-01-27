using UnityEngine;
using System.Collections;
using ResourceLoad;
using GameCore;

public class HomeScrollData
{
    public ItemNode item1;
    public ItemNode item2;

    public HomeScrollData(ItemNode _it1, ItemNode _it2)
    {
        item1 = _it1;
        item2 = _it2;
    }
}

public class HomeScrollNode : UIPoolListNode
{

    public UITexture[] m_textures;
    public UILabel[] m_lblpprice;
    public UILabel[] m_lblnprice;
    public UILabel[] m_lbldesc;

    public HomeScrollData Data
    {
        get
        {
            return m_data as HomeScrollData;
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        HomeScrollData data = Data;
        TextureHandler.Instance.LoadItemTexture(Data.item1.t_item.img, (obj) =>
        {
            m_textures [0].mainTexture = obj as Texture;
        });
        m_lbldesc [0].text = "[ff0000]"+data.item1.t_item.name+"[-] "+ data.item1.t_item.description;
        m_lblpprice [0].text = string.Format(Localization.Get(10063), data.item1.n_item.pprice);
        m_lblnprice [0].text =  string.Format(Localization.Get(10064), data.item1.n_item.nprice);
        UIEventListener.Get(m_textures [0].gameObject).onClick = OnItem1Click;

        if (data.item2 != null)
        {
            TextureHandler.Instance.LoadItemTexture(data.item2.t_item.img, (obj) =>
            {
                m_textures [1].mainTexture = obj as Texture;
            });
            m_lbldesc [1].text = "[ff0000]"+data.item2.t_item.name+"[-] "+ data.item2.t_item.description;
            m_lblpprice [1].text = string.Format(Localization.Get(10063), data.item2.n_item.pprice);
            m_lblnprice [1].text = string.Format(Localization.Get(10064), data.item2.n_item.nprice);
            UIEventListener.Get(m_textures [1].gameObject).onClick = OnItem2Click;
        } else
        {
            m_textures[1].mainTexture=null;
            m_lbldesc [1].text = string.Empty;
            m_lblpprice [1].text = string.Empty;
            m_lblnprice [1].text =  string.Empty;
            UIEventListener.Get(m_textures [1].gameObject).onClick = null;
        }
    }

    private void OnItem1Click(GameObject go)
    {
//        Debug.Log("click name: " + Data.item1.t_item.name);
        UIHandler.Instance.Push(PageID.ITEMSDETAIL,Data.item1);
    }

    private void OnItem2Click(GameObject go)
    {
//        Debug.Log("click name: " + Data.item2.t_item.name);
        UIHandler.Instance.Push(PageID.ITEMSDETAIL,Data.item2);
    }
}
