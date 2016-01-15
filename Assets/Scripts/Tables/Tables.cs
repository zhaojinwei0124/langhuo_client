using UnityEngine;
using System.Collections.Generic;
using DeJson;
using ResourceLoad;
using System;
using System.Reflection;

/// <summary>
/// author: huailiang.peng
/// data:   2015.12.31
/// func:   read or get config tables from remote and local
/// </summary>

namespace Config
{
    public class Tables : Single<Tables>
    {

        Deserializer _deserial;

        public Deserializer deserial
        {
            get
            {
                if (_deserial == null)
                    _deserial = new Deserializer();
                return _deserial;
            }
        }
       

        string[] tables;

        Dictionary<string,string> dicTables;
        public void InitAll()
        {
            if(tables==null) tables=TableID.ids;
            if (dicTables == null) 
                dicTables = new Dictionary<string, string>();
            dicTables.Clear();
            for (int i=0; i<tables.Length; i++)
            {
                TextAsset txt = Resources.Load<TextAsset>("Tables/" + tables [i]);
//                Debug.Log("id:"+tables[i]+" txt: "+txt.text);
                dicTables.Add(tables[i], txt.text);
            }
        }

        /// <summary>
        /// just load local Asset
        /// </summary>
        public T GetTable<T>(string name) //where T : struct
        {
            if (dicTables.ContainsKey(name))
            {
                return deserial.Deserialize<T>(dicTables [name]);
            }
            return default(T);
        }

        /// <summary>
        /// Load Asset check path local first and remote then
        /// </summary>
        public void GetTable<T>(string name, Action<T> cb)
        {
            if (dicTables.ContainsKey(name))
            {
                Debug.Log(name+" : "+dicTables[name]);
                cb(deserial.Deserialize<T>(dicTables [name]));
            } else
            {
                Downloader.Instance.LoadAsyncTextasset(name, (obj) =>
                {
                    string txt = (obj as TextAsset).text;
                    cb(deserial.Deserialize<T>(txt));
                });
            }
        }

    }
}
