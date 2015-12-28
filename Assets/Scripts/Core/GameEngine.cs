using UnityEngine;
using System.Collections.Generic;
using GameCore;
using MobageLitJson;

public sealed class GameEngine : MonoSingle<GameEngine>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("Game engine start.");


        TextAsset txt=Resources.Load<TextAsset>("Tables/activity");
        Debug.Log(txt.text);

        JsonData json=JsonMapper.ToObject(txt.text);
        Debug.Log("length: "+json.Count);
        Debug.Log(json[0]["name"]);
//        foreach(var item in list)
//        {
//            Debug.Log("ite: "+item.name);
//        }

//        List<object> list =  MiniJSON.Json.Deserialize(txt.text) as List<object>;
//        Debug.Log("list cnt: "+list.Count);
//        foreach(var item in list)
//        {
//            Debug.Log("obj "+item.ToString());
//           ActivityNode it =  item as ActivityNode;
//            Debug.Log("name: "+it.name);
//        }
       

       // ReadTables.Instance.InitAll();

//        ResourceLoad.Downloader.Instance.LoadAsyncTextasset("test",(obj)=>
//        {
//            Debug.Log("load txt is: "+(obj as TextAsset).text);
//        });

//        List<ItemNode> items = ReadTables.Instance.GetTable("items") as List<ItemNode>;
//
//        if(items!=null)
//        {
//            Debug.Log("item cnt: "+items.Count);
//            foreach(var item in items)
//            {
//                Debug.Log(item.id+" name: "+item.name+" desc: "+item.description);
//            }
//        }
//        else
//        {
//            Debug.Log("items is null");
//        }
    }

    void Update()
    {
        TimerManager.Instance.Update();
    }




}
