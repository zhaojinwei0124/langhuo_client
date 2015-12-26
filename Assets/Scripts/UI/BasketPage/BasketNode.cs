using UnityEngine;
using System.Collections;

public class BasketItem
{
    public int cnt;
    public int price;
    public string desc;
    public string texture;
};


public class BasketNode : UIPoolListNode 
{

    [SerializeField]
    private UITexture m_txtIcon;

    [SerializeField]
    private UILabel m_lbPrice;

    [SerializeField]
    private UILabel m_lbDesc;

    [SerializeField]
    private UILabel m_lbCnt;


    [SerializeField]
    private GameObject m_goModify;

        
	void Start () 
    {
        UIEventListener.Get(m_goModify).onClick=(go)=>
        {
            Debug.Log("modify count");
        };
	}
	
   
    protected BasketItem Data
    {
        get{  return m_data as BasketItem;}
    }


    public override void Refresh()
    {
        base.Refresh();
        m_lbCnt.text=Data.cnt.ToString();
        m_lbPrice.text=Data.price.ToString();
        m_lbDesc.text=Data.desc.ToString();

    }
}
