using UnityEngine;
using System.Collections.Generic;
using GameCore;

public class InvitePage : View
{

    public UIPoolList m_pool;
    string msg = string.Empty;
    
    public override void RefreshView()
    {
        base.RefreshView();
        if (string.IsNullOrEmpty(msg))
        {
            msg = SDKManager.Instance.Contacts(null);
            Debug.Log("RefreshView msg:  " + msg);
            if (!string.IsNullOrEmpty(msg))
            {
                List<FriendItem> items = Util.Instance.ParseContacts(msg);
                m_pool.Initialize(items.ToArray());
            }
        }
    }
}
