using UnityEngine;
using System.Collections.Generic;
using GameCore;
using Network;

public class FriendPage : View
{

    public UIPoolList m_pool;
    string msg = string.Empty;

    public override void RefreshView()
    {
        base.RefreshView();
        if (string.IsNullOrEmpty(msg))
        {
            msg = SDKManager.Instance.Contacts("");
            Debug.Log("unity RefreshView msg:  " + msg);
            if (!string.IsNullOrEmpty(msg))
            {
                List<FriendItem> items = Util.Instance.ParseContacts(msg);
                CulPriends(items);
            }
        }
    }

    private void CulPriends(List<FriendItem> items)
    {
        List<string> friends = items.ConvertAll<string>(x => x.phone);
        NetCommand.Instance.SelectFriends(friends, (str) =>
        {
            if (!string.IsNullOrEmpty(str))
            {
                List<FriendItem> rps = Util.Instance.ParseContacts(str);
                m_pool.Initialize(rps.ToArray());
            }
            else
            {
                Toast.Instance.Show(10066);
            }
        });
    }

}
