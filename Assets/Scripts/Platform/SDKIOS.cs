//using System;
//using UnityEngine;
//using System.Runtime.InteropServices;
//
//
//public class SDKIOS  
//{
//
//    [DllImport("__Internal")]
//    private static extern void iapPay(string message);
//
//    public static void ActivityIapPay(string message)
//    {
//        if (Application.platform != RuntimePlatform.OSXEditor)
//        {
//            iapPay(message);
//        }
//    }
//
//
//    [DllImport("__Internal")]
//    private static extern void WeiXinShare(string message);
//
//    public static void ActivityWeiXinShare(string message)
//    {
//        if (Application.platform != RuntimePlatform.OSXEditor)
//        {
//            WeiXinShare(message);
//        }
//    }
//
//
//    [DllImport("__Internal")]
//    private static extern void WeiXinShareToFriend(string message);
//
//    public static void ActivityWeiXinShareToFriend(string message)
//    {
//        if (Application.platform != RuntimePlatform.OSXEditor)
//        {
//            WeiXinShareToFriend(message);
//        }
//    }
//	
//
//
//}
