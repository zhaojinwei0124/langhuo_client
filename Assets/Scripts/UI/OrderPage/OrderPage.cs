using UnityEngine;
using System.Collections;

public class OrderPage : View
{
    public UIPoolList m_pool;

    public GameObject m_objConfirm;

    public GameObject m_objCount;

    public override void RefreshView()
    {
        base.RefreshView();

        UIEventListener.Get(m_objConfirm).onClick=OnConfirm;

        UIEventListener.Get(m_objCount).onClick=OnAccount;

    }




    private void OnConfirm(GameObject go)
    {

        Toast.Instance.Show(10010);
    }



    private void OnAccount(GameObject go)
    {
        Toast.Instance.Show(10010);
    }
}
