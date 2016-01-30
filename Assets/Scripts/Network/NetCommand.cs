using UnityEngine;
using System.Collections.Generic;

namespace Network
{
    public class NetCommand :Single<NetCommand>
    {

        //------------------ API -------------------------
        public void HttpPost(string url, Dictionary<string, string> param, MsgCallback msgCallback, ErrCallback errCallback=null)
        {
            GameServer.Instance.HttpPost(url, param, msgCallback, errCallback);
        }

        public void SearchUser(string weiboId, MsgCallback msgCallback, ErrCallback errCallback)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID", GameBaseInfo.Instance.user.tel.ToString());
            HttpPost("user/search", param, msgCallback, errCallback);
        }

        public void RegistUser(string name, string tel, int paymode, string addr,PayType type, MsgCallback msgCallback, ErrCallback errCallback=null)
        {
            Dictionary<string,string> param = new Dictionary<string, string>();
            param.Add("name", name);
            param.Add("tel", tel);
            param.Add("paymode", paymode.ToString());
            param.Add("addr", addr);
            param.Add("type",((int)type).ToString());
            HttpPost("user/regist", param, msgCallback, errCallback);
        }

        public void LoginUser(string userid, MsgCallback msgCallback, ErrCallback errCallback=null)
        {
            Dictionary<string,string> param = new Dictionary<string, string>();
            param.Add("tel", userid);
            HttpPost("user/login", param, msgCallback, errCallback);
        }

        
        public void SelectFriends(List<string> friends, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("friends", GameCore.Util.Instance.SerializeArray(friends));
            HttpPost("user/friend", param, msgCallback, errCallback);
        }

        public void UpdateBase(int bases, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tel", GameBaseInfo.Instance.user.tel.ToString());
            param.Add("ba", bases.ToString());
            HttpPost("user/base", param, msgCallback, errCallback);
        }
  
        //get all items
        public void GetItems(MsgCallback msgCallback, ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID", "0");//GameBaseInfo.Instance.user.tel.ToString());
            HttpPost("item/search", param, msgCallback, errCallback);
        }

        //add new order
        public void SysnOrder(int price,string name,int type, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tel", GameBaseInfo.Instance.user.tel.ToString());
            param.Add("addr",GameBaseInfo.Instance.user.addr);
            param.Add("name",name);
            param.Add("type",type.ToString());
            param.Add("items",GameBaseInfo.Instance.GetItems());
            param.Add("cnt",GameBaseInfo.Instance.GetCnt());
            param.Add("pri",price.ToString());
            HttpPost("order/add", param, msgCallback, errCallback);
        }

        //confirm order state
        public void UpdateOder(int[] orderids, long accept, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("orderids", GameCore.Util.Instance.SerializeArray(orderids));
            param.Add("accept", accept.ToString());
            HttpPost("order/confirm", param, msgCallback, errCallback);
        }


        public void CompleteOrder(int orderid, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("orderid", orderid.ToString());
            HttpPost("order/complete", param, msgCallback, errCallback);
        }

        public void CompleteOrders(int[] orderids, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            string ids=GameCore.Util.Instance.SerializeArray(orderids);
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("orderids", ids);
            HttpPost("order/completes", param, msgCallback, errCallback);
        }

        //get all orders for self
        public void SearchOders(long tel, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tel", tel.ToString());
            HttpPost("order/search", param, msgCallback, errCallback);
        }

        //get all orders for langjian langti
        public void GetOders(long tel, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tel", tel.ToString());
            HttpPost("order/get", param, msgCallback, errCallback);
        }

        public void SortOders(string orderid, MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("orderid", orderid.ToString());
            HttpPost("order/sort", param, msgCallback, errCallback);
        }



    }
}