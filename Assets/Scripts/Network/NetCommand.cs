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
            param.Add("ID", GameBaseInfo.Instance.userid.ToString());
            HttpPost("user/search", param, msgCallback, errCallback);
        }

        public void RegistUser(string name, string tel, int paymode, string addr, MsgCallback msgCallback, ErrCallback errCallback=null)
        {
            Dictionary<string,string> param = new Dictionary<string, string>();
            param.Add("name", name);
            param.Add("tel", tel);
            param.Add("paymode", paymode.ToString());
            param.Add("addr", addr);
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
            param.Add("ID", GameBaseInfo.Instance.userid.ToString());
            HttpPost("item/search", param, msgCallback, errCallback);
        }

        public void GetUserWorldRandList(string userId, int scope, MsgCallback msgCallback, ErrCallback errCallback=null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID", GameBaseInfo.Instance.userid.ToString());
            param.Add("scope", scope.ToString());
            HttpPost("score/getScopeRedis", param, msgCallback, errCallback);
        }

    }
}