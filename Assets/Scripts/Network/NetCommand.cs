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
  
        public void GetItems(MsgCallback msgCallback, ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID", "0");//GameBaseInfo.Instance.user.tel.ToString());
            HttpPost("item/search", param, msgCallback, errCallback);
        }

        public void SysnOrder(MsgCallback msgCallback,ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tel", GameBaseInfo.Instance.user.tel.ToString());
            param.Add("addr",GameBaseInfo.Instance.user.addr);
            param.Add("type",((int)GameBaseInfo.Instance.user.type).ToString());
            param.Add("items",GameBaseInfo.Instance.GetItems());
            param.Add("cnt",GameBaseInfo.Instance.GetCnt());
            HttpPost("order/add", param, msgCallback, errCallback);
        }

    }
}