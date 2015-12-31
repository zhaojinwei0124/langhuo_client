using UnityEngine;
using System.Collections.Generic;
using GameCore;
using DeJson;


public sealed class GameEngine : MonoSingle<GameEngine>
{

    Deserializer m_json;

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

        //TextAsset txt = Resources.Load<TextAsset>("Tables/activity");

        //Debug.Log(txt.text);

        //m_json = new DeJson.Deserializer();

        //ActivityNode[] mlist = m_json.Deserialize<ActivityNode[]>(txt.text);

        Debug.LogError("length: " + mlist.Length);

        foreach (var item in mlist)
        {
            Debug.LogError("item: " + item.description + " name: " + item.name);
        }
    }


    void Update()
    {
        TimerManager.Instance.Update();
    }

    public Deserializer getJson()
    {
        if (m_json == null) m_json = new Deserializer();
        return m_json;
    }


}
