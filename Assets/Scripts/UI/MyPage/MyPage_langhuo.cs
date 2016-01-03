using UnityEngine;
using System.Collections;
using GameCore;


public class MyPage_langhuo : MonoBehaviour 
{

    public GameObject m_objOrder;

    public GameObject m_objFriend;

    public GameObject m_objBalance;


	// Use this for initialization
	void Start () 
    {
        UIEventListener.Get(m_objOrder).onClick=OnOrder;
        UIEventListener.Get(m_objFriend).onClick=OnFriend;
        UIEventListener.Get(m_objBalance).onClick=OnBalance;
	}
	
	

    private void OnOrder(GameObject go)
    {
        UIHandler.Instance.Push(PageID.ORDER);
    }


    private void OnFriend(GameObject go)
    {
        UIHandler.Instance.Push(PageID.FRIEND);
    }

    private void OnBalance(GameObject go)
    {
        UIHandler.Instance.Push(PageID.BALANCE);
    }

}
