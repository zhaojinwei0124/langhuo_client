using UnityEngine;
using System.Collections;
using ResourceLoad;
using Network;

public class ItemDetailPage : View
{

    public UITexture m_texture;
    public UILabel m_lblName;
    public UILabel m_lbldesc;
    public UILabel m_lblpprice;
    public UILabel m_lblnprice;
    public UILabel m_lblcnt;
    public UISprite m_sprAdd;
    public UISprite m_sprReduce;
    public GameObject m_objBuy;
    public GameObject m_objClose;

    ItemNode m_item;
    int m_cnt;

    public override void Refresh(object data)
    {
        base.Refresh(data);
        m_item = data as ItemNode;
        m_cnt=1;
        UIEventListener.Get(m_sprAdd.gameObject).onClick = OnAdd;
        UIEventListener.Get(m_sprReduce.gameObject).onClick = OnReduce;
        UIEventListener.Get(m_objBuy).onClick = AddBuy;
        UIEventListener.Get(m_objClose).onClick=Close;
        RefreshUI();
    }

    private void RefreshUI()
    {
        TextureHandler.Instance.LoadTexture("Item/" + m_item.t_item.img, (obj) =>
        {
            m_texture.mainTexture = obj as Texture;
        });
        m_lbldesc.text = "[ff0000]" + m_item.t_item.name + "[-] " + m_item.t_item.description;
        m_lblpprice.text = "￥" + m_item.n_item.pprice;
        m_lblnprice.text = "￥" + m_item.n_item.nprice + "元/份";
        m_lblcnt.text = m_cnt.ToString();
    }

    private void OnAdd(GameObject go)
    {
        m_cnt++;
        m_lblcnt.text=m_cnt.ToString();
    }

    private void OnReduce(GameObject go)
    {
        m_cnt--;
        m_lblcnt.text=m_cnt.ToString();
    }

    private void AddBuy(GameObject go)
    {
        Debug.Log("add card list");
        GameBaseInfo.Instance.AddBuyNode(new GameBaseInfo.BuyNode(m_item.n_item.id,m_cnt));
    }

    private void Close(GameObject go)
    {
        Debug.Log("close page");
        UIManager.Instance.ShowFront(false);
    }

}
