using UnityEngine;
using System.Collections.Generic;
using GameCore;
using Network;

public class FriendPage : View
{

    public UIPoolList m_pool;
    public UILabel m_lblleft;
    public UILabel m_lblright;
    string msg = string.Empty;
    List<FriendItem> items = new List<FriendItem>();

    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_lblleft.gameObject).onClick = OnLeft;
        UIEventListener.Get(m_lblright.gameObject).onClick = OnRight;
        m_lblright.text = GameBaseInfo.Instance.user.code == 2 ? Localization.Get(10093):Localization.Get(10094);
        if (string.IsNullOrEmpty(msg))
        {
            msg = SDKManager.Instance.Contacts("");
            Debug.Log("unity RefreshView msg:  " + msg);
            if (!string.IsNullOrEmpty(msg))
            {
                items.Clear();
                items = Util.Instance.ParseContacts(msg);
                items.RemoveAll(x => x.phone.Length != 11 || !x.phone.StartsWith("1"));
                items.RemoveAll(x => x.phone == GameBaseInfo.Instance.user.tel.ToString());
                SyncFriends(items);
            }
        }
    }

    private bool Check()
    {
        if (GameBaseInfo.Instance.user.bases == 20000)
        {
            Toast.Instance.Show(10080);
            UIHandler.Instance.Push(PageID.BASE);
            return false;
        }
        return true;
    }

    private bool CheckOrder()
    {
        foreach (var item in GameBaseInfo.Instance.myOrders)
        {
            Debug.Log("item code: " + item.state);
        }
        return GameBaseInfo.Instance.myOrders.Exists(x => x.state <= 1);
    }

    //goto langjian
    private void OnLeft(GameObject go)
    {
        if (Check())
        {
            NetCommand.Instance.UpdateUserCode(1, (str) =>
            {
                Toast.Instance.Show(10076);
                GameBaseInfo.Instance.UpdateAccount((succ)=>{RefreshView();});
            });
        }
    }


    //qiudaiti or cancel
    public void OnRight(GameObject go)
    {
        if (GameBaseInfo.Instance.user.code == 1)
        {
            Dialog.Instance.Show(10091, () =>
            {
                NetCommand.Instance.UpdateUserCode(0, (str) => 
                {
                    Toast.Instance.Show(10076);
                    GameBaseInfo.Instance.UpdateAccount();
                    SyncFriends();
                });
            });
            return;
        }
        if (!CheckOrder())
        {
            Toast.Instance.Show(10090);
            return;
        }
        if (Check())
        {
            int code = GameBaseInfo.Instance.user.code == 0 ? 2 : 0;
            NetCommand.Instance.UpdateUserCode(code, (str) => 
            {
                Toast.Instance.Show(10076);
                GameBaseInfo.Instance.UpdateAccount((succ)=>{RefreshView();});
            });
        }
    }

    public void SyncFriends()
    {
        if (items != null)
        {
            m_pool.Initialize(null);
            SyncFriends(items);
        }
    }

    private void SyncFriends(List<FriendItem> items)
    {
        List<string> friends = items.ConvertAll<string>(x => x.phone);
        NetCommand.Instance.UserFriends(friends, (str) =>
        {
            List<NFriend> fds = Util.Instance.Get<List<NFriend>>(str);
            foreach (var item in items)
            {
                NFriend fi = fds.Find(x => x.tel.ToString() == item.phone);
                if (fi != null)
                {
                    item.bases = fi.bases;
                    item.code = fi.code;
                } else
                {
                    Debug.LogError("phone: " + item.phone);
                }
            }
            items.Sort((x,y) => y.code - x.code);
            m_pool.Initialize(items.ToArray());
        });
    }

}
