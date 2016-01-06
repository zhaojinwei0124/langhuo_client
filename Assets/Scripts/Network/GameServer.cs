using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{

    //public delegate void OnResponse(WWW www);
    public delegate void MsgCallback(string msg);

    public delegate void ErrCallback(string msg);

    public class GameServer : Single<GameServer>
    {

        public const string BASE_URL = "http://146577.vhost151.cloudvhost.cn/langhuo/";
            //"http://192.168.0.103/langhuo/";
        public const string NET_URL = BASE_URL + "Netframework/";

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

            public bool isValid()
            {
                return code == 200;
            }
        }


        public static bool CheckConnection()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Instance.connectStatus = connectStatusType.offline;
                return false;
            } else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                Instance.connectStatus = connectStatusType.carrier;
                return true;
            } else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Instance.connectStatus = connectStatusType.wifi;
                return true;
            }
            return false;
        }

        public void HttpPost(string url, Dictionary<string, string> param, MsgCallback msgCallback, ErrCallback errCallback)
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
                Print(url, param);
                Loadding.Instance.Show(true);
                GameEngine.Instance.StartCoroutine(postConnect(NET_URL + url + ".php", form, msgCallback, errCallback));
            }
        }

        private void Print(string url, Dictionary<string, string> param)
        {
            string str = NET_URL + url + ".php?";
            foreach (var item in param)
            {
                str += item.Key + ":" + item.Value + "&";
            }
            Debug.Log("request=> " + str.Substring(0, str.Length - 1));
        }

        public void HttpGet(string url, MsgCallback msgCallback, ErrCallback errCallback)
        {
            if (url != null)
            {
                Loadding.Instance.Show(true);
                GameEngine.Instance.StartCoroutine(connect(NET_URL + url, msgCallback, errCallback));
            }
        }

        IEnumerator postConnect(string url, WWWForm param, MsgCallback msgCallback, ErrCallback errCallback)
        {
            WWW www = new WWW(url, param);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("net error=> " + www.error);
                if (errCallback != null)
                    errCallback(www.error);
            } else
            {
                string resp = www.text.Trim();
                Debug.Log("respond=> " + resp);
                if (resp.StartsWith("false"))
                {
                    if (errCallback != null)
                        errCallback(resp);
                } else
                {
                    if (msgCallback != null)
                        msgCallback(resp);
                }
            }
            Loadding.Instance.Show(false);
            www.Dispose();
        }

        IEnumerator connect(string url, MsgCallback msgCallback, ErrCallback errCallback)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                Debug.Log("net error=> " + www.error);
                if (errCallback != null)
                    errCallback(www.error);
            } else
            {
                string resp = www.text.Trim();
                Debug.Log("response=> " + resp);
                if (resp.StartsWith("false"))
                {
                    if (errCallback != null)
                        errCallback(resp);
                } else
                {
                    if (msgCallback != null)
                        msgCallback(resp);
                }
            }
            Loadding.Instance.Show(false);
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
            res.code = JsonUtil.ParseInt(result_dic ["code"]);
            res.message = result_dic ["msg"].ToString();
            return res;
        }

    }

}
