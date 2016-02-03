using UnityEngine;
using System.Collections.Generic;
using GameCore;
using Config;
using Network;
using Platform;

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
        if (!Check())
            return;
        TableID.Init(); 
        GameBaseInfo.Instance.Init();
        UIHandler.Instance.Init();
        Tables.Instance.InitAll();
        UIManager.Instance.Init();
        LocationManager.Instance.Init();
    }

    bool Check()
    {
        if (!GameServer.CheckConnection())
        {
            Toast.Instance.Show(10067);
            return false;
        }
        return true;
    }

    void Update()
    {
        TimerManager.Instance.Update();

#if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("clean cache");
            Caching.CleanCache();
            PlayerPrefs.DeleteAll();
        }
#elif UNITY_ANDROID
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Dialog.Instance.Show("确定要退出吗",(ga)=>{ Application.Quit();});
        }
#endif

    }

 


}
