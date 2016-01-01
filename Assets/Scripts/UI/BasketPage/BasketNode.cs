using UnityEngine;
using System.Collections;
using ResourceLoad;

public class BasketItem
{
    public int cnt;
    public int id;
};

public class BasketNode : UIPoolListNode
{

    public UITexture m_txtIcon;
    public UILabel m_lbPrice;
    public UILabel m_lbDesc;
    public UILabel m_lbCnt;
    public GameObject m_goModify;
        
    void Start()
    {
        UIEventListener.Get(m_goModify).onClick = (go) =>
        {
            Debug.Log("modify count");
        };
    }
   
    protected BasketItem Data
    {
        get{ return m_data as BasketItem;}
    }

    public override void Refresh()
    {
        base.Refresh();
        ItemNode item = Home.Instance.items.Find(x => x.n_item.id == Data.id);

        m_lbCnt.text = Data.cnt.ToString();
        m_lbPrice.text = "价格： ￥" + item.n_item.nprice;
        m_lbDesc.text = "[ff0000]" + item.t_item.name + "[-] " + item.t_item.description;
        TextureHandler.Instance.LoadTexture("Item/" + item.t_item.img, (obj) =>
        {
            m_txtIcon.mainTexture = obj as Texture;
        });

    }
}
