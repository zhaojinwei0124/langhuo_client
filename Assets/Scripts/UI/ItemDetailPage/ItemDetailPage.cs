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
    ItemNode m_item;
    int m_cnt;

    public override void Refresh(object data)
    {
        base.Refresh(data);
        m_item = data as ItemNode;
        m_cnt = GetCnt();
        UIEventListener.Get(m_sprAdd.gameObject).onClick = OnAdd;
        UIEventListener.Get(m_sprReduce.gameObject).onClick = OnReduce;
        UIEventListener.Get(m_objBuy).onClick = GoBuy;
        RefreshUI();
    }

    private int GetCnt()
    {
        GameBaseInfo.BuyNode node = GameBaseInfo.Instance.buy_list.Find(x => x.id == m_item.n_item.id);
        if (node.cnt == 0)
            return 1;
        return node.cnt;
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
        m_lblName.text = m_item.t_item.name;
    }

    private void OnAdd(GameObject go)
    {
        m_cnt++;
        m_lblcnt.text = m_cnt.ToString();
    }

    private void OnReduce(GameObject go)
    {
        if (m_cnt > 0)
            m_cnt--;
        m_lblcnt.text = m_cnt.ToString();
    }

    private void GoBuy(GameObject go)
    {
//        Debug.Log("add card list");
        GameBaseInfo.Instance.AddBuyNode(m_item.n_item.id, m_cnt);
        Close();
        // UIManager.Instance.ShowHomeView(1);
    }

  
}
