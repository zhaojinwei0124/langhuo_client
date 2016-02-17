using System;
using UnityEngine;


namespace Network
{
    public class NOrder
    {
        public int id;

        public int state;

        public int val;

        public string items;

        public string cnt;

        public int type;

        public string name;

        public Int64 tel;

        public string addr;

        public Int64 accept;

        public string timestap;

        public DateTime time
        {
            get
            {
                return Convert.ToDateTime(timestap);
            }
        }

        public DateTime rcvTime
        {
            get
            {
                return time+new TimeSpan(24,0,0);
            }
        }

        public int[] GetItems()
        {
            Debug.Log("items: "+items);
            return GameCore.Util.Instance.Get<int[]>(items);
        }


        public int[] GetCnts()
        {
            return GameCore.Util.Instance.Get<int[]>(cnt);
        }
    }
}
