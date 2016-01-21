using UnityEngine;
using System.Collections;

namespace GameCore
{
    public struct PageID
    {

        public static string HOME        =  "HomePage";
        public static string BASKET      =  "BasketPage";
        public static string MYPAGE      =  "MyPage";
        public static string ITEMSDETAIL =  "ItemDetailPage";
        public static string ITEMSMODIFY =  "ItemModifyPage";
        public static string SETTING     =  "SettingPage";
        public static string ACCOUNT     =  "AccountPage";
        public static string ORDER       =  "OrderPage";
        public static string FRIEND      =  "FriendPage";
        public static string BALANCE     =  "BalancePage";
        public static string Regist      =  "RegistPage";
        public static string Login       =  "LoginPage";
        public static string HEADICON    =  "HeadIconPage";
        public static string LANGTI      =  "AppLangtiPage";
        public static string LANGJIAN    =  "AppLangjianPage";
        public static string INVITE      =  "InvitePage";
        public static string TEXT        =  "TextPage";
        public static string TYPE        =  "TypePage";
        //add other page id here....

    
        private string V;
    
        public PageID(string aa)
        {
            V = aa;
        }
    
        public static implicit operator string(PageID id)
        {
            return (id.ToString());
        }
    
        public static implicit operator PageID(string id)
        {
            return (new PageID(id));
        }
    
        public override string ToString()
        {
            return (V);
        }
    }
}