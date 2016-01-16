using System;
using UnityEngine;
using System.Runtime.InteropServices;


public class SDKIOS  
{

//    [DllImport("__Internal")]
//    private static extern void iapPay(string message);

    public static void IapPay(string message)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
           // iapPay(message);
        }
    }


//    [DllImport("__Internal")]
//    private static extern void WeiXinShare(string message);

    public static void WeiXinShare(string message)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
           // WeiXinShare(message);
        }
    }


//    [DllImport("__Internal")]
//    private static extern void Contacts(string message);

    public static string Contacts(string message)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
        {
            // Contacts(message);
        }
        return string.Empty;
    }
	


}
