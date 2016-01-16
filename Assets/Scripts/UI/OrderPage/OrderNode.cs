using UnityEngine;
using System.Collections;
using Network;

public class OrderItem
{
    public int itemid;
    public int state;
    public PayType type;
    public string price;
}

public class OrderNode : UIPoolListNode
{
    public UITexture m_texture;
    public UILabel m_lblDesc;
    public UILabel m_lblTime;
    public UILabel m_lblType;
    public GameObject m_objCertain;

    public OrderItem Data
    {
        get
        {
            return m_data as OrderItem;
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        UIEventListener.Get(m_objCertain).onClick = OnCertainClick;

        ItemNode item = GameBaseInfo.Instance.items.Find(x => x.n_item.id == Data.itemid);

        m_lblType.text = Data.type == PayType.LANGJIAN ? Localization.Get(10027) : Localization.Get(10030);
        m_lblDesc.text = item.t_item.description;
        ResourceLoad.TextureHandler.Instance.LoadItemTexture(item.t_item.img, (ob) =>
        {
            m_texture.mainTexture = ob as Texture;
        });
    }

    private void OnCertainClick(GameObject go)
    {
    }
}
