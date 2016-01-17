using System;

namespace Network
{
    public class NOrder
    {
        public int id;

        public int state;

        public string items;

        public string cnt;

        public int type;

       // public string addr;


        public int[] GetItems()
        {
            return GameCore.Util.Instance.Get<int[]>(items);
        }


        public int[] GetCnts()
        {
            return GameCore.Util.Instance.Get<int[]>(cnt);
        }
    }
}
