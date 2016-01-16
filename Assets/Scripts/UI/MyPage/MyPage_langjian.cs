using UnityEngine;
using System.Collections;

public class MyPage_langjian : MonoBehaviour {


    public UIPoolList m_pool;

    public GameObject m_objAccnt;

    public GameObject m_objOK;


	public void Refresh()
    {

        UIEventListener.Get(m_objOK).onClick=OnConfirm;

        UIEventListener.Get(m_objAccnt).onClick=OnAccnt;
    }



    private void OnAccnt(GameObject go)
    {

    }


    private void OnConfirm(GameObject go)
    {
        
    }
}
