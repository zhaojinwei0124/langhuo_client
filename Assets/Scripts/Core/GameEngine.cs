using UnityEngine;
using System.Collections;
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
        ResourceLoad.Downloader.Instance.LoadAsyncTextasset("test",(obj)=>
        {
            Debug.Log("load txt is: "+(obj as TextAsset).text);
        });
    }

    void Update()
    {
        TimerManager.Instance.Update();
    }




}
