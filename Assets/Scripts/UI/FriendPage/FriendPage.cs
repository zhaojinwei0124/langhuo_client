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

    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_lblleft.gameObject).onClick = OnLeft;
        UIEventListener.Get(m_lblright.gameObject).onClick = OnRight;
        if (string.IsNullOrEmpty(msg))
        {
            msg = SDKManager.Instance.Contacts("");
            Debug.Log("unity RefreshView msg:  " + msg);
            if (!string.IsNullOrEmpty(msg))
            {
                List<FriendItem> items = Util.Instance.ParseContacts(msg);
                foreach (var item in items)
                {
                    Debug.Log("x: " + item.phone + " length: " + item.phone.Length);
                }
                items.RemoveAll(x => x.phone.Length != 11 || !x.phone.StartsWith("1"));
                items.RemoveAll(x => x.phone == GameBaseInfo.Instance.user.tel.ToString());

                Debug.Log("length: " + items.Count);
                CulPriends(items);
            }
        }
    }


    private bool Check()
    {
        if(GameBaseInfo.Instance.user.bases==20000)
        {
            Toast.Instance.Show(10080);
            UIHandler.Instance.Push(PageID.BASE);
            return false;
        }
        return true;
    }


    private void OnLeft(GameObject go)
    {
        if(Check())
        {
            NetCommand.Instance.UpdateUserCode(1, (str) => Toast.Instance.Show(10076));
        }   
    }



    private void OnRight(GameObject go)
    {
        if(Check())
        {
            NetCommand.Instance.UpdateUserCode(2, (str) => Toast.Instance.Show(10076));
        }
    }



    private void CulPriends(List<FriendItem> items)
    {
        List<string> friends = items.ConvertAll<string>(x => x.phone);
        NetCommand.Instance.SelectFriends(friends, (str) =>
        {
            items = Util.Instance.ParseContacts(str);
            List<string> friends2 = items.ConvertAll<string>(x => x.phone);
            NetCommand.Instance.FriendState(friends2, (nmsg) =>
            {
                if (!string.IsNullOrEmpty(nmsg))
                {
                    List<CallBackItem> cbs = Util.Instance.ParseCallback(nmsg);
                    foreach (var item in cbs)
                    {
                        FriendItem fi = items.Find(x => x.phone == item.key);
                        if (fi != null)
                        {
                            fi.statecode = item.value;
                        }
                    }
                }
                items.Sort((x,y)=>int.Parse(y.statecode)-int.Parse(x.statecode));
                m_pool.Initialize(items.ToArray());
            });
        });
    }

}
