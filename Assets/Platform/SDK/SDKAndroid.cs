using UnityEngine;
using System.Collections;

public class SDKAndroid 
{
    #if UNITY_ANDROID
    public static void WeiXinShare(string message)
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                curActivity.Call("WeixinMessageShare", message);
            }
        }
    }

    public static void WeiXinWebShare(string message)
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                curActivity.Call("WeixinShareWebPageToFriend", message);
            }
        }
    }


    public static string Contacts(string msg)
    {
      
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
               return curActivity.Call<string>("GetConnacts", msg);
            }
        }
      
    }
    #endif
}
