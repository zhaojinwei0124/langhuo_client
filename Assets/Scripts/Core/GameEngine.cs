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
       // TimerManager.Instance.AddTimer(1000, 10, (x) => Debug.Log("req: "+x));
    }

    void Update()
    {
        TimerManager.Instance.Update();
    }




}
