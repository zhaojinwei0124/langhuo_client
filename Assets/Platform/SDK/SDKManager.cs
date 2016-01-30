using UnityEngine;
using System.Collections;
using System;

public class SDKManager:Single<SDKManager>
{


    public string Contacts(string msg)
    {
#if UNITY_EDITOR
        return "SDK Contacts:123456789";
#endif

#if UNITY_ANDROID
      return  SDKAndroid.Contacts(msg);
#elif UNITY_IPHONE
       return SDKIOS.Contacts(msg);
#endif
    }

    public void WeixinMessageShare(string msg)
    {
#if UNITY_EDITOR
        Debug.Log("SDK WeixinMessageShare Called!");
        return;
#endif

#if UNITY_ANDROID
        SDKAndroid.WeiXinShare(msg);
#elif UNITY_IPHONE
         SDKIOS.WeiXinShare(msg);
#endif
    }


    public void WeixinWebShare(string msg)
    {
        #if UNITY_EDITOR
        Debug.Log("SDK WeixinWebShare Called!");
        return;
        #endif
        
        #if UNITY_ANDROID
        SDKAndroid.WeiXinWebShare(msg);
        #elif UNITY_IPHONE
        SDKIOS.WeiXinShare(msg);
        #endif
    }

}
