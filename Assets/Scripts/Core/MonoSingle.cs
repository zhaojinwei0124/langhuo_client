using UnityEngine;
using System;

/// <summary>
/// @huailiang.peng
/// @2015.11.30
/// </summary>
public class MonoSingle<T> : MonoBehaviour where T : MonoSingle<T>
{
    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Type theType = typeof(T);

                _instance = (T)FindObjectOfType(theType);

                if (_instance == null)
                {

                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();

                    //¹Ò½Óµ½GameEngineÏÂ
                    GameObject bootObj = GameObject.Find("GameEngine");
                    if (bootObj != null)
                    {
                        go.transform.parent = bootObj.transform;
                    }
                }
            }
            return _instance;
        }
    }


    public static void DestroyInstance()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = null;
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance.gameObject != gameObject)
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            } else
            {
                DestroyImmediate(gameObject);
            }
        } else if (_instance == null)
        {
            _instance = GetComponent<T>();
        }

        Init();
    }

    protected virtual void OnDestroy()
    {
        if (_instance != null && _instance.gameObject == gameObject)
        {
            _instance = null;
        }
    }

    public virtual void DestroySelf()
    {
        _instance = null;
        Destroy(gameObject);
    }

    public static bool HasInstance()
    {
        return _instance != null;
    }

    protected virtual void Init()
    {

    }


}