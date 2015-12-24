//using UnityEngine;
//using System.Collections;
//using System;
//
//public class SDKManager
//{
//    public static SDKManager _s = null;
//
//    public static SDKManager S
//    {
//        get
//        {
//            if (_s == null) _s = new SDKManager();
//            return _s;
//        }
//    }
//
//    public void Init()
//    {
//        CleanAlert();
//    }
//
//
//    public void WeixinMessageShare(string msg)
//    {
//#if UNITY_ANDROID
//        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//        {
//            using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
//            {
//                curActivity.Call("WeixinMessageShare", msg);
//            }
//        }
//#elif UNITY_IPHONE
//         SDKIOS.ActivityWeiXinShare(msg);
//#endif
//    }
//
//
//    public void WeixinShareMessageToFriend(string msg)
//    {
//#if UNITY_ANDROID
//        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//        {
//            using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
//            {
//                curActivity.Call("WeixinShareMessageToFriend", msg);
//            }
//        }
//#elif UNITY_IPHONE
//         SDKIOS.ActivityWeiXinShareToFriend(msg);
//#endif
//    }
//
//
//    public void Quit()
//    {
//#if UNITY_ANDROID
//        RegNotification();
//        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//        {
//            using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
//            {
//                curActivity.Call("Quit", "");
//            }
//        }
//#endif
//    }
//
//    public void CleanAlert()
//    {
//#if UNITY_ANDROID
//        AndroidJavaClass notiClass = new AndroidJavaClass("com.example.notitest.NotificationMainActivity");
//        notiClass.CallStatic("alarmCancel", 10);
//#endif
//    }
//
//
//    private static int notiIndex = 0;
//
//
//    public void RegNotification()
//    {
//        AlertMessage("birds", "Let us happy together, join us1!", 4);
//        AlertMessage("birds", "Let us happy together, join us2!", 64);
//        AlertSpecialMessage("birds", "Let us happy together, join us at 11 hour!", 11);
//    }
//
//    /// <summary>
//    /// 剩余时间推送
//    /// </summary>
//    public void AlertMessage(string title, string message, int leftTime)
//    {
//#if UNITY_ANDROID
//        AndroidJavaClass notiClass = new AndroidJavaClass("com.example.notitest.NotificationMainActivity");
//        notiClass.CallStatic("noti", title, message, leftTime, notiIndex++);
//#endif
//    }
//
//    /// <summary>
//    /// 指定时间推送
//    /// </summary>
//    public static void AlertSpecialMessage(string title, string message, int hour)
//    {
//        DateTime date = DateTime.Now;
//        DateTime fireDate;
//        if (date.Hour < hour)
//        {
//            fireDate = new DateTime(date.Year, date.Month, date.Day);
//        }
//        else
//        {
//            fireDate = new DateTime(date.Year, date.Month, date.Day + 1);
//        }
//
//        fireDate = fireDate.AddHours(hour);
//        int timeInterval = (int)(fireDate - date).TotalSeconds;
//        Debug.Log("addNotification: timeInterval = " + timeInterval);
//#if UNITY_ANDROID
//        AndroidJavaClass notiClass = new AndroidJavaClass("com.example.notitest.NotificationMainActivity");
//        notiClass.CallStatic("noti", title, message, timeInterval, notiIndex++);
//#endif
//
//    }
//
//
//}
