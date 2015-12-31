using UnityEngine;
using System.Collections;
using ResourceLoad;

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
        TextureHandler.Instance.LoadTexture("Item/" + Data.item1.t_item.img, (obj) =>
        {
            m_textures [0].mainTexture = obj as Texture;
        });
        TextureHandler.Instance.LoadTexture("Item/" + data.item2.t_item.img, (obj) =>
        {
            m_textures[1].mainTexture= obj as Texture;
        });

        m_lbldesc[0].text=data.item1.t_item.description;
        m_lbldesc[1].text=data.item2.t_item.description;

        m_lblpprice[0].text="￥"+data.item1.n_item.pprice;
        m_lblpprice[1].text="￥"+data.item2.n_item.pprice;

        m_lblnprice[0].text="￥"+data.item1.n_item.nprice+"元/份";
        m_lblnprice[1].text="￥"+data.item2.n_item.nprice+"元/份";


    }
}
