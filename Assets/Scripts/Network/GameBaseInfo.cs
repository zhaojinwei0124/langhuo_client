using UnityEngine;
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

        public List<ItemNode> items = new List<ItemNode>();

        public string encrypt{ get; set; }

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
        }

        public void ClearBuy()
        {
            buy_list.Clear();
        }


        public string GetItems()
        {
            return Util.Instance.SerializeArray(buy_list.ConvertAll(x => x.id));
        }

        public string GetCnt()
        {
            return Util.Instance.SerializeArray(buy_list.ConvertAll(x => x.cnt));
        }
    }

}