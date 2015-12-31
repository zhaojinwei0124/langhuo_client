//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using NbaLitJson;
//using System.Text;
//using System.Security.Cryptography;
//using System;
//using System.IO;
//using Ionic.Zlib;

//namespace Network
//{
//    class NetworkUtil
//    {

//        public static string UnzipString (byte[] compbytes )
//        {
//            return 	ZlibStream.UncompressString(compbytes);
//        }

//        static NbaLitJson.JsonMapper _main_json_mapper;
//        public static NbaLitJson.JsonMapper  MainJM()
//        {
//            if(_main_json_mapper==null)
//            {
//                _main_json_mapper=new NbaLitJson.JsonMapper();
//            }
//            return _main_json_mapper;
//        }
		
//        static NbaLitJson.JsonMapper _sub_json_mapper;
//        public static NbaLitJson.JsonMapper SubJM()
//        {
//            if(_sub_json_mapper==null)
//            {
//                _sub_json_mapper=new NbaLitJson.JsonMapper();
//            }
//            return _sub_json_mapper;
//        }
//    }
//}
