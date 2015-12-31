﻿using UnityEngine;
using System.Collections.Generic;
using GameCore;
using Config;

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

        Tables.Instance.InitAll();

//        Tables.Instance.GetTable<ActivityNode[]>("activity", (mlist) =>
//        {
//            Debug.Log("length: " + mlist.Length);
//            
//            foreach (var item in mlist)
//            {
//                Debug.LogError("item: " + item.description + " name: " + item.name);
//            }
//        });
//
//        Tables.Instance.GetTable<ItemNode[]>("items", (mlist) =>
//        {
//                        Debug.Log("length: " + mlist.Length);
//                        
//                        foreach (var item in mlist)
//                        {
//                            Debug.LogError("item: " + item.id + " score: " + item.score+" name:"+item.price);
//                        }
//
//        });

    }

    void Update()
    {
        TimerManager.Instance.Update();
    }

 


}
