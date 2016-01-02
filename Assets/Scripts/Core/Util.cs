using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Ionic.Zlib;
using Config;
using System.Text;

namespace GameCore
{
    class Util:Single<Util>
    {

        public string UnzipString (byte[] compbytes )
        {
            return 	ZlibStream.UncompressString(compbytes);
        }

        /// <summary>
        /// make string to object
        /// </summary>
        public T Get<T>(string str) //where T : struct
        {
             return Tables.Instance.deserial.Deserialize<T>(str);
        }

        /// <summary>
        /// make array or list to json string
        /// </summary>
        public string SerializeArray(IList anArray)
        {
            StringBuilder builder=new StringBuilder();
            builder.Append('[');
            bool first = true;
            foreach (object obj in anArray) 
            {
                if (!first) 
                {
                    builder.Append(',');
                }
                first = false;
            }
            builder.Append(']');
            return builder.ToString();
         }



    }
}
