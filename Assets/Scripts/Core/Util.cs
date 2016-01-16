using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Ionic.Zlib;
using Config;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;

namespace GameCore
{
    class Util:Single<Util>
    {
        /// <summary>
        /// Unzips the file bytes
        /// </summary>
        public string UnzipString(byte[] compbytes)
        {
            return  ZlibStream.UncompressString(compbytes);
        }

        /// <summary>
        /// make string to object
        /// </summary>
        public T Get<T>(string str) //where T : struct
        {
            return Tables.Instance.deserial.Deserialize<T>(str);
        }

        public List<FriendItem> ParseContacts(string msg)
        {
            List<FriendItem> lt = new List<FriendItem>();
            string[] nodes = msg.Split(',');
            foreach (var item in nodes)
            {
                string[] n = item.Split(':');
                FriendItem node=new FriendItem();
                node.phone = n [1];
                node.name = n [0];
                lt.Add(node);
            }
            return lt;
        }


        /// <summary>
        /// make array or list to json string
        /// </summary>
        public string SerializeArray(IList array)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            int index = 0;
            foreach (object obj in array)
            {
                index++;
                if (index < array.Count)
                {
                    builder.Append(obj);
                    builder.Append(',');
                } else
                    builder.Append(obj);
            }
            builder.Append(']');
            return builder.ToString();
        }

        public int GetLength(string s)
        {
            int length = 0;
            foreach (char c in s)
            {
                if (c != '\n')
                {
                    if (c >= '\u4e00' && c <= '\u9fa5')
                        length += 2;
                    else
                        length += 1;
                }
            }
            
            return UnityEngine.Mathf.CeilToInt(length / 2);
        }

        ///   <summary>
        ///   给一个字符串进行MD5加密
        ///   </summary>
        ///   <param   name="strText">待加密字符串</param>
        ///   <returns>加密后的字符串</returns>
        public string MD5Encrypt(string  strText)
        {   
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(strText);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData [i].ToString("x2");
            }
            return byte2String;
        }

        public Color GetColorFrom255(int _r, int _g, int _b, int _a)
        {
            Color temp_color;
            temp_color.r = _r / 255.0f;
            temp_color.g = _g / 255.0f;
            temp_color.b = _b / 255.0f;
            temp_color.a = _a / 255.0f;
            return temp_color;
        }
        
        public void SetLayer(Transform trans, string layerName)
        {
            foreach (Transform child in trans)
            {
                child.gameObject.layer = LayerMask.NameToLayer(layerName);
                SetLayer(child, layerName);
            }
        }

    }
}
