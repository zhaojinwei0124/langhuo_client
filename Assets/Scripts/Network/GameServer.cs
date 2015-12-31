using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{

    public delegate void OnResponse(WWW www);



    public class GameServer : Single<GameServer>
    {

        public const string BASE_URL = "http://localhost/langhuo/";

        public const string NET_URL = BASE_URL+ "Netframework/";

        public enum connectStatusType
        {
            offline = 0,
            carrier,
            wifi
        }

        public connectStatusType connectStatus = connectStatusType.offline;

        public struct RequestResult
        {
            public int code;
            public string message;
            public bool isValid() { return code == 200; }
        }


        public static bool CheckConnection()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Instance.connectStatus = connectStatusType.offline;
                return false;
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                Instance.connectStatus = connectStatusType.carrier;
                return true;
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Instance.connectStatus = connectStatusType.wifi;
                return true;
            }
            return false;
        }


        public void HttpPost(string url, Dictionary<string, string> param, OnResponse response = null)
        {
            if (url != null)
            {
                WWWForm form = new WWWForm();
                string orign = string.Empty;
                foreach (KeyValuePair<string, string> post_arg in param)
                {
                    form.AddField(post_arg.Key, post_arg.Value);
                    orign += post_arg.Value;
                }
                GameEngine.Instance.StartCoroutine(postConnect(NET_URL + url+".php", form, response));
            }
        }


        public void HttpGet(string url, OnResponse response = null)
        {
            if (url != null)
            {
               GameEngine.Instance.StartCoroutine(connect(NET_URL + url, response));
            }
        }

        IEnumerator postConnect(string url, WWWForm param, OnResponse response = null)
        {
            Debug.Log("post=>" + url);
            WWW www = new WWW(url, param);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("WWW Error: " + www.error + " url: " + url);
            }
            else
            {
                Debug.Log("WWW Ok!: " + www.text);
            }
            if (response != null) response(www);
            www.Dispose();
        }

        IEnumerator connect(string url, OnResponse response = null)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                Debug.Log("WWW Error: " + www.error);
            }
            else
            {
                Debug.Log("WWW Ok!: " + www.text);
            }
            if (response != null) response(www);
            www.Dispose();
        }

        static public RequestResult ParseResult(string response_json)
        {
            Dictionary<string, object> result = MiniJSON.Json.Deserialize(response_json) as Dictionary<string, object>;
            return ParseResult(result);
        }

        static public RequestResult ParseResult(Dictionary<string, object> result_dic)
        {
            RequestResult res = new RequestResult();
            res.code = JsonUtil.ParseInt(result_dic["code"]);
            res.message = result_dic["msg"].ToString();
            return res;
        }

    }

}
