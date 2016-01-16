using UnityEngine;
using System.Collections;
using ResourceLoad;
using GameCore;


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
        UIEventListener.Get(m_goModify).onClick = OnModify;
    }
   
    protected BasketItem Data
    {
        get{ return m_data as BasketItem;}
    }


    private ItemNode mItemNode;

    public override void Refresh()
    {
        base.Refresh();
        mItemNode = Network.GameBaseInfo.Instance.items.Find(x => x.n_item.id == Data.id);

        m_lbCnt.text = Data.cnt.ToString();
        m_lbPrice.text = "价格： ￥" + mItemNode.n_item.nprice;
        m_lbDesc.text = "[ff0000]" + mItemNode.t_item.name + "[-] " + mItemNode.t_item.description;
        TextureHandler.Instance.LoadTexture("Item/" + mItemNode.t_item.img, (obj) =>
        {
            m_txtIcon.mainTexture = obj as Texture;
        });

    }


    private void OnModify(GameObject go)
    {
        UIHandler.Instance.Push(PageID.ITEMSMODIFY,mItemNode);
    }
}
