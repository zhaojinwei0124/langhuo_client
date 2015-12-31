using UnityEngine;
using System.Collections;

public class MyPage : View 
{

    [SerializeField]
    private GameObject[] m_tabpages;

    [SerializeField]
    private UITab[] m_tabs;

    [SerializeField]

	void Start () 
    {
        Debug.Log("MyPage Start");
        UIEventListener.Get(m_tabs[0].gameObject).onClick=(go)=>Show(0);
        UIEventListener.Get(m_tabs[1].gameObject).onClick=(go)=>Show(1);
        UIEventListener.Get(m_tabs[2].gameObject).onClick=(go)=>Show(2);
	}
	


    private void HideAll()
    {
        foreach(var ite in m_tabpages)
        {
            ite.gameObject.SetActive(false);
        }
    }
    
    
    private void Show(int index)
    {
        Debug.Log("show index: "+index);
        HideAll();
        m_tabpages[index].gameObject.SetActive(true);
    }
}
