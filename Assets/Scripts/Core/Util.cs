using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Ionic.Zlib;
using Config;

namespace GameCore
{
    class Util:Single<Util>
    {

        public string UnzipString (byte[] compbytes )
        {
            return 	ZlibStream.UncompressString(compbytes);
        }

        public T Get<T>(string str) //where T : struct
        {
             return Tables.Instance.deserial.Deserialize<T>(str);
        }
    }
}
