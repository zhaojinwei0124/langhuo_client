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
                foreach(var item in items)
                {
                    Debug.Log("x: "+item.phone+" length: "+item.phone.Length);
                }
                items.RemoveAll(x=> x.phone.Length!=11 || !x.phone.StartsWith("1"));
                items.RemoveAll(x=>x.phone==GameBaseInfo.Instance.user.tel.ToString());

                Debug.Log("length: "+items.Count);
                CulPriends(items);
            }
        }
    }

    private void CulPriends(List<FriendItem> items)
    {
        List<string> friends = items.ConvertAll<string>(x => x.phone);
        NetCommand.Instance.SelectFriends(friends, (str) =>
        {
            items = Util.Instance.ParseContacts(str);
            List<string> friends2 = items.ConvertAll<string>(x => x.phone);
            NetCommand.Instance.SearchFriends(friends2,(nmsg)=>
            {
                if (!string.IsNullOrEmpty(nmsg))
                {
                    List<CallBackItem> cbs=Util.Instance.ParseCallback(nmsg);
                    List<string> senders=cbs.ConvertAll<string>(x=>x.key);
                    foreach(var item in items)
                    {
                        if(senders.Contains(item.phone)) item.orderid=cbs.Find(x=>x.key==item.phone).value;
                    }
                }
                m_pool.Initialize(items.ToArray());
            });
        });
    }

}
