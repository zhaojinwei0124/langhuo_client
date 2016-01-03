using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Ionic.Zlib;
using Config;
using System.Text;
using UnityEngine;

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

        public Color GetColorFrom16(string _str, float _alpha)
        {
            if (_str == null || _str.Length < 4)
            {
                return Color.white;
            }
            Color temp_color;
            temp_color.a = _alpha;
            string _temp = _str.Substring(0, 2);
            temp_color.r = System.Convert.ToInt32(_temp, 16) / 255.0f;
            _temp = _str.Substring(2, 2);
            temp_color.g = System.Convert.ToInt32(_temp, 16) / 255.0f;
            _temp = _str.Substring(4, 2);
            temp_color.b = System.Convert.ToInt32(_temp, 16) / 255.0f;
            return temp_color;
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
