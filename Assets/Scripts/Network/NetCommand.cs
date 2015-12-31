using UnityEngine;
using System.Collections.Generic;


namespace Network
{

    public class NetCommand :Single<NetCommand>
    {

        //------------------ API -------------------------
        public void HttpPost(string url, Dictionary<string, string> param,  OnResponse response)
        {
            GameServer.Instance.HttpPost(url, param, response);
        }

        public void SearchUser(string weiboId, OnResponse response)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID", GameBaseInfo.Instance.userid.ToString());
            HttpPost("user/search", param, response);
        }

  
        public void GetItems(OnResponse response)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID",GameBaseInfo.Instance.userid.ToString());
            HttpPost("item/search", param,response);
        }

        public void GetUserWorldRandList(string userId, int scope, OnResponse response)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID", GameBaseInfo.Instance.userid.ToString());
            param.Add("scope", scope.ToString());
            HttpPost("score/getScopeRedis", param, response);
        }

    }
}