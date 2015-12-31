using UnityEngine;
using System;

/// <summary>
/// 基类继承树中有MonoBehavrour类的单件实现，这种单件实现有利于减少对场景树的查询操作
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

                    //挂接到GameEngine下
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


    /// <summary>
    /// 删除单件实例,这种继承关系的单件生命周期应该由模块显示管理
    /// </summary>
    public static void DestroyInstance()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = null;
    }

    /// <summary>
    ///  Awake消息，确保单件实例的唯一性
    /// </summary>
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

    /// <summary>
    /// OnDestroy消息，确保单件的静态实例会随着GameObject销毁
    /// </summary>
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