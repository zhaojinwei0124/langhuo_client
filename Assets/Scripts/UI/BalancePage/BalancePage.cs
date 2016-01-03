using UnityEngine;
using System.Collections;
using Network;

public class BalancePage : View
{


    public UILabel m_lblBalance;
    public GameObject m_objRecharge;
    public GameObject m_objTake;

    public override void RefreshView()
    {
        base.RefreshView();

        UIEventListener.Get(m_objTake).onClick = OnTake;

        UIEventListener.Get(m_objRecharge).onClick = OnRecharge;

        RefreshUI();
    }

    private void RefreshUI()
    {
        m_lblBalance.text = string.Format("{0:F}", GameBaseInfo.Instance.user.balance) + "元";
    }

    private void OnRecharge(GameObject go)
    {
        Debug.Log("Onrecharge");
    }

    private void OnTake(GameObject go)
    {
        Debug.Log("Ontake");
    }
}
