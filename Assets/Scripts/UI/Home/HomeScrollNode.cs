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
        TextureHandler.Instance.LoadTexture("Item/" + Data.item1.t_item.img, (obj) =>
        {
            m_textures [0].mainTexture = obj as Texture;
        });
        m_lbldesc [0].text = "[ff0000]"+data.item1.t_item.name+"[-] "+ data.item1.t_item.description;
        m_lblpprice [0].text = "￥" + data.item1.n_item.pprice;
        m_lblnprice [0].text = "￥" + data.item1.n_item.nprice + "元/份";
        UIEventListener.Get(m_textures [0].gameObject).onClick = OnItem1Click;

        if (data != null)
        {
            m_textures[1].transform.parent.gameObject.SetActive(true);
            TextureHandler.Instance.LoadTexture("Item/" + data.item2.t_item.img, (obj) =>
            {
                m_textures [1].mainTexture = obj as Texture;
            });
            m_lbldesc [1].text = "[ff0000]"+data.item2.t_item.name+"[-] "+ data.item2.t_item.description;
            m_lblpprice [1].text = "￥" + data.item2.n_item.pprice;
            m_lblnprice [1].text = "￥" + data.item2.n_item.nprice + "元/份";
            UIEventListener.Get(m_textures [1].gameObject).onClick = OnItem2Click;
        } else
        {
            m_textures[1].transform.gameObject.SetActive(false);
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
