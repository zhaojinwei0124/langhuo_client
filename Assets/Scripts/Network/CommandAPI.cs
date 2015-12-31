using UnityEngine;
using System.Collections.Generic;


namespace Network
{

    public class CommandAPI :Single<CommandAPI>
    {

        //------------------ API -------------------------
        public void HttpPost(string url, Dictionary<string, string> param,  OnResponse response)
        {
            GameServer.Instance.HttpPost(url, param, response);
        }

        public void SearchUser(string userId, string weiboId, OnResponse response)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ID", userId);
            HttpPost("user/search", param, response);
        }

  
        public void GetItems(OnResponse response)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            HttpPost("item/search", param, response);
        }

        public void GetUserWorldRandList(string userId, int scope, OnResponse response)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("date", "");
            param.Add("scope", scope.ToString());
            HttpPost("score/getScopeRedis", param, response);
        }

    }
}