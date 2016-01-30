using UnityEngine;
using System.Collections;

namespace GameCore
{
    public struct PlayerprefID
    {
        public static string USERID      =  "userid";
        public static string BUYLIST     =  "buylist";
        public static string HEADICO     =  "headicon";
        public static string BASE        =  "basecode";
        //add other page id here....
        
        
        private string V;
        
        public PlayerprefID(string aa)
        {
            V = aa;
        }
        
        public static implicit operator string(PlayerprefID id)
        {
            return (id.ToString());
        }
        
        public static implicit operator PlayerprefID(string id)
        {
            return (new PlayerprefID(id));
        }
        
        public override string ToString()
        {
            return (V);
        }
    }
}