using UnityEngine;
using System.Collections.Generic;
using GameCore;
using Network;

public class FriendPage : View
{

    public UIPoolList m_pool;
    public UILabel m_lblleft;
    public UILabel m_lblright;
    public UILabel m_lbltel;
    public UILabel m_lbladd;
    string msg = string.Empty;
    List<FriendItem> items = new List<FriendItem>();

    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_lblleft.gameObject).onClick = OnLeft;
        UIEventListener.Get(m_lblright.gameObject).onClick = OnRight;
        UIEventListener.Get(m_lbladd.gameObject).onClick=OnAddFriend;
        m_lblright.text = GameBaseInfo.Instance.user.code == 2 ? Localization.Get(10093) : Localization.Get(10094);
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

    private void OnAddFriend(GameObject go)
    {
        string phone =m_lbltel.text.Trim();
        if(!Util.Instance.CheckPhoneValid(phone))
        {
            Toast.Instance.Show(10017);
            return;
        }
        NetCommand.Instance.RequestFriend(phone,(str) =>
        {
            Toast.Instance.Show(10099);
            GameBaseInfo.Instance.UpdateAccount();
            SyncFriends();
        },(err)=>Toast.Instance.Show(10101));
    }

    //检查浪尖地址
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

    private bool CheckSelfOrder()
    {
        List<NOrder> orders =GameBaseInfo.Instance.myOrders;
        Debug.Log("type: "+orders[0].type+" cnt: "+orders.Count+" state: "+orders[0].state);
        return orders.Count>0 && orders.Exists(x => x.type == 0 && x.state <= 1);
    }


    //go to langjian
    private void OnLeft(GameObject go)
    {
        if (Check())
        {
            NetCommand.Instance.UpdateUserCode(1, (str) =>
            {
                Toast.Instance.Show(10076);
                GameBaseInfo.Instance.UpdateAccount((succ) => {
                    RefreshView();});
            });
        }
    }


    //qiudaiti or cancel
    public void OnRight(GameObject go)
    {
        if (GameBaseInfo.Instance.user.code == 1) //如果去浪尖code =1  点击取消去浪尖code = 0
        {
            Dialog.Instance.Show(10091, () =>UpdateCode(0));
            return;
        }
        if (!CheckOrder()) //如果没有订单 就去浪尖就没有意义
        {
            Toast.Instance.Show(10090);
            return;
        }
        if (Check())    //检查浪尖地址 去浪尖code =1
        {
            //如果没有消息code = 0, 求代提code =2，如果已经去浪尖code =1, 就没有必要去求代提了
            int code = GameBaseInfo.Instance.user.code == 0 ? 2 : 0;
            if(CheckSelfOrder())Dialog.Instance.Show(10034, ()=>UpdateCode(code));
            else UpdateCode(code);
        }
    }


    private void UpdateCode(int code)
    {
        NetCommand.Instance.UpdateUserCode(code, (str) => 
        {
            Toast.Instance.Show(10076);
            GameBaseInfo.Instance.UpdateAccount((succ) => {
                RefreshView();});
        });
    }

    public void SyncFriends()
    {
        if (items != null)
        {
            m_pool.Initialize(null);
            SyncFriends(items);
        }
    }

    private void SyncFriends(List<FriendItem> contacts)
    {
        // List<FriendItem> friends=new List<FriendItem>();
        NetCommand.Instance.UserFriends(contacts.ConvertAll(x => x.phone), (str) =>
        {
            items.Clear();
            List<NFriend> fds = Util.Instance.Get<List<NFriend>>(str);
            foreach (var item in fds)
            {
                if (items.Exists(x => x.phone == item.tel))
                {
                    FriendItem fi = items.Find(x => x.phone == item.tel);
                    fi.code = item.code > fi.code ? item.code : fi.code;
                } else
                {
                    FriendItem fi = new FriendItem();
                    fi.bases = item.bases;
                    fi.code = item.tel.StartsWith("9")?-2: item.code;
                    fi.name = item.name;
                    fi.phone = item.tel;
                    items.Add(fi);
                }
            }
            items.Sort((x,y) => y.code - x.code);
            m_pool.Initialize(items.ToArray());
        });
    }

}
