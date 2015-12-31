using UnityEngine;
using System.Collections.Generic;
using DeJson;

namespace GameCore
{
    public class ReadTables : Single<ReadTables>
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

        string[] tables = new string[]{"items","activity"};
        Dictionary<string,string> dicTables;

        public void InitAll()
        {
            if (dicTables == null) 
                dicTables = new Dictionary<string, string>();
            dicTables.Clear();
            for (int i=0; i<tables.Length; i++)
            {
                TextAsset txt = Resources.Load<TextAsset>("Tables/" + tables [i]);
                //  Debug.Log("add: "+txt.text);
                dicTables.Add(tables [i], txt.text);
            }
        }

        public T GetTable<T>(string name) //where T : struct
        {
            if (dicTables.ContainsKey(name))
            {
                return deserial.Deserialize<T>(dicTables [name]);
            } else
            {
                return default(T);
            }
        }


    }
}
