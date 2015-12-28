using UnityEngine;
using System.Collections.Generic;
using MiniJSON;


namespace GameCore
{
    public class ReadTables : Single<ReadTables> 
    {

        string[] tables=new string[]{"items","activity"};

        Dictionary<string,object> dicTables;


        public void InitAll()
        {
            if(dicTables==null) 
                dicTables=new Dictionary<string, object>();
            dicTables.Clear();
            for(int i=0;i<tables.Length;i++)
            {
                TextAsset txt=Resources.Load<TextAsset>("Tables/"+tables[i]);
                Debug.Log("add: "+txt.text);
                dicTables.Add(tables[i], Json.Deserialize(txt.text));
            }
        }
    	

        public object GetTable(string name)
        {
            if(dicTables==null) 
            {
                Debug.LogError("not load table end!");
                return null;
            }
            else if(!dicTables.ContainsKey(name))
            {
                Debug.LogError("table "+name+" is null");
                return null;
            }
            else
            {
                return dicTables[name];
            }
        }
    }
}
