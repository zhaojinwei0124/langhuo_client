using UnityEngine;
using System.Collections.Generic;
using GameCore;

public class InvitePage : View
{

    public UILabel m_lblInvit;
    public UILabel m_lblShare;

    string msg = string.Empty;
    
    public override void RefreshView()
    {
        base.RefreshView();
        if (string.IsNullOrEmpty(msg))
        {
            msg = SDKManager.Instance.Contacts("");
            Debug.Log("RefreshView msg:  " + msg);

        }

        UIEventListener.Get(m_lblInvit.gameObject).onClick=OnInvite;
        UIEventListener.Get(m_lblShare.gameObject).onClick=OnShare;
    }




    private void OnInvite(GameObject go)
    {
        SDKManager.Instance.WeixinMessageShare("");
    }


    private void OnShare(GameObject go)
    {
        SDKManager.Instance.WeixinWebShare("");
    }
}
