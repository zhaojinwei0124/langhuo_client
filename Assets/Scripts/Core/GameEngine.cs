using UnityEngine;
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

        UIManager.Instance.Init();

        Tables.Instance.InitAll();
//
//        Tables.Instance.GetTable<List<TItem>>(TableID.ITEMS, (mlist) =>
//        {
//                Debug.Log("length: " + mlist.Count);
//                
//                foreach (var item in mlist)
//                {
//                    Debug.LogError("item: " + item.id + " type: " + item.type+" name:"+item.description);
//                }
//
//        });

    }

    void Update()
    {
        TimerManager.Instance.Update();
    }

 


}
