using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Network;


namespace Platform
{
    public class SimApi :Single<SimApi>
    {


        public void HttpPost(string url, WWWForm param, MsgCallback msgCallback, ErrCallback errCallback)
        {
            if (url != null)
            {
                Loadding.Instance.Show(true);
                GameEngine.Instance.StartCoroutine(postConnect(SimDef.SIM_URL+url, param, msgCallback, errCallback));
            }
        }


        IEnumerator postConnect(string url, WWWForm param, MsgCallback msgCallback, ErrCallback errCallback)
        {
            WWW www = new WWW(url, param);
            Debug.Log("request=> "+(url+"  "+ System.Text.Encoding.Default.GetString(param.data)));
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


        public void Charge(MsgCallback msgDel,ErrCallback errDel)
        {
            string url="20150822/create/client";
            WWWForm form=new WWWForm();
            form.AddField("accountSid",SimDef.ACCOUNT_SID);
            form.AddField("appId",SimDef.APP_ID);
            HttpPost(url,form,msgDel,errDel);
        }


        public void Sms(MsgCallback msgDel,ErrCallback errDel)
        {
            string url="20150822/SMS/emailSMS";
            string timestamp=string.Format("{0:yyyyMMddHHmmss}", System.DateTime.Now);
            WWWForm form=new WWWForm();
            form.AddField("accountSid",SimDef.ACCOUNT_SID);
            form.AddField("appId",SimDef.APP_ID);
            form.AddField("emailTemplateId",SimDef.MSG_ID);
            form.AddField("to",SimDef.CLIENT_NO);
            form.AddField("param","1008");
            form.AddField("timestamp",timestamp);
            form.AddField("sig",GameCore.Util.Instance.MD5Encrypt(SimDef.ACCOUNT_SID+SimDef.AUTH_TOKEN+timestamp));
            HttpPost(url,form,msgDel,errDel);
        }

    }
}
