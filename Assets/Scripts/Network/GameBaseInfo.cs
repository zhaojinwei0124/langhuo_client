﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore;

/// <summary>
/// all local data for app
/// </summary>
namespace Network
{
    public enum PayMode
    {
        WXPay = 0,
        AliPay =1,
    };

    public enum PayType
    {
        LANGJIAN=1,
        LANGTI=2,
    };

    public class GameBaseInfo:Single<GameBaseInfo>
    {
        public NUser user{ get; set; }

        //home
        public List<ItemNode> items = new List<ItemNode>();

        //self
        public List<NOrder> myOrders = new List<NOrder>();

        //langjian langti
        public List<NOrder> othOrders = new List<NOrder>();

        public string encrypt{ get; set; }

        //gps localization
        public Address address;


        public struct BuyNode
        {
            public int id;
            public int cnt;

            public BuyNode(int _id, int _cnt)
            {
                id = _id;
                cnt = _cnt;
            }
        };

        public PayMode payMode = PayMode.AliPay;
        public List<BuyNode> buy_list = new List<BuyNode>();

        public void Init()
        {
            base.Init();
            string buys = PlayerPrefs.GetString(PlayerprefID.BUYLIST);
            if (!string.IsNullOrEmpty(buys))
            {
                buy_list = Util.Instance.Get<List<BuyNode>>(buys);
            }
        }

        public void AddBuyNode(int id, int cnt)
        {
            if (buy_list.ConvertAll(x => x.id).Contains(id))
            {
                if (cnt == 0)
                {
                    BuyNode node = buy_list.Find(x => x.id == id);
                    buy_list.Remove(node);
                } else
                {
                    for (int i=0; i<buy_list.Count; i++)
                    {
                        if (buy_list [i].id == id)
                        {
                            buy_list [i] = new BuyNode(id, cnt);
                            break;
                        }
                    }
                }
            } else
            {
                buy_list.Add(new BuyNode(id, cnt));
            }
            PlayerPrefs.SetString(PlayerprefID.BUYLIST, DeJson.Serializer.Serialize(buy_list.ToArray()));
        }

        public int GetPaycnt()
        {
            int price = 0;
            foreach (var item in GameBaseInfo.Instance.buy_list)
            {
                ItemNode node = Home.Instance.items.Find(x => x.n_item.id == item.id);
                price += node.n_item.nprice * item.cnt;
            }
            return price;
        }

        public void ClearBuy()
        {
            buy_list.Clear();
            PlayerPrefs.SetString(PlayerprefID.BUYLIST, DeJson.Serializer.Serialize(buy_list.ToArray()));
        }

        public void UpdateOrders()
        {
            UpdateOrders(null);
        }

        public void UpdateOrders(DelManager.BoolDelegate del)
        {
            NetCommand.Instance.SearchOders(GameBaseInfo.Instance.user.tel, (string res) =>
            {
                GameBaseInfo.Instance.myOrders = Util.Instance.Get<List<NOrder>>(res);
                Debug.Log("cnt: "+GameBaseInfo.Instance.myOrders.Count);
                if (del != null)
                    del(true);
            }, (string err) =>
            {
                if (del != null)
                    del(false);
                Toast.Instance.Show(10026);
            });
        }

        public void UpdateAccount()
        {
            UpdateAccount(null);
        }

        public void UpdateAccount(DelManager.BoolDelegate del)
        {
            NetCommand.Instance.LoginUser(PlayerPrefs.GetString(PlayerprefID.USERID), (string res) =>
            {
                NUser nuser = Util.Instance.Get<NUser>(res);
                GameBaseInfo.Instance.user = nuser;
                if (del != null)
                    del(true);
            }, (string err) => {
                if (del != null)
                    del(false);});
        }

        public string GetBuyItems()
        {
            return Util.Instance.SerializeArray(buy_list.ConvertAll(x => x.id));
        }

        public string GetBuyCnt()
        {
            return Util.Instance.SerializeArray(buy_list.ConvertAll(x => x.cnt));
        }


        public bool CanCancel(NOrder order)
        {
            return order.state==0 || (order.state==1 && order.accept==0);
        }

    }

}