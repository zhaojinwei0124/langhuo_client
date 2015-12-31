using UnityEngine;
using System.Collections.Generic;
using GameCore;


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

        ReadTables.Instance.InitAll();

        ActivityNode[] mlist = ReadTables.Instance.GetTable<ActivityNode[]>("activity");

//        Debug.Log("length: " + mlist.Length);
//
//        foreach (var item in mlist)
//        {
//            Debug.LogError("item: " + item.description + " name: " + item.name);
//        }
    }

    void Update()
    {
        TimerManager.Instance.Update();
    }

 


}
