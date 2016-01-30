using System;
using UnityEngine;
using System.Runtime.InteropServices;


public class SDKIOS  
{

    [DllImport("__Internal")]
    public static extern void iapPay(string message);

    [DllImport("__Internal")]
    public static extern void WeiXinShare(string message);

    [DllImport("__Internal")]
    public static extern void WeiXinShareToFriend(string message);


    [DllImport("__Internal")]
    public static extern string Contacts(string message);


	


}
