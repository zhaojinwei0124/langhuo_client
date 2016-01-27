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
        TableID.Init(); 
        GameBaseInfo.Instance.Init();
        UIHandler.Instance.Init();
        Tables.Instance.InitAll(() =>
        {
            UIManager.Instance.Init();
            LocationManager.Instance.Init();
        });
       
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
        if(Input.GetKeyUp(KeyCode.S))
        {
           Platform.SimApi.Instance.Sms(1234,"15216768456",(res)=>
            {
                Debug.Log("sms repd: "+res);
            },(err)=>
            {
                Debug.LogError("sms fail");
            });
        }
#elif UNITY_ANDROID
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Dialog.Instance.Show("确定要退出吗",(ga)=>{ Application.Quit();});
        }
#endif

    }

 


}
