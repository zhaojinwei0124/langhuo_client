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

    }

    void Update()
    {
        TimerManager.Instance.Update();

#if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("clean cache");
            Caching.CleanCache();
        }
#endif

    }

 


}
