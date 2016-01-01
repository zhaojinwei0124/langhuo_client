using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{
    public class GameBaseInfo:Single<GameBaseInfo>
    {

        public int userid{ get; set; }

        public string encrypt{ get; set; }

        public string city{ get; set; }

        public string distric{ get; set; }

        public struct BuyNode
        {
            public int id;
            public int cnt;

            public BuyNode (int _id,int _cnt)
            {
                id=_id;
                cnt=_cnt;
            }
        };

        public List<BuyNode> buy_list=new List<BuyNode>();


        public void AddBuyNode(BuyNode node)
        {
            buy_list.Add(node);
        }

        public void ClearBuy()
        {
            buy_list.Clear();
        }


        public void InitLocal()
        {
           city="上海";
           distric="五角场提货点";
        }
    }

}