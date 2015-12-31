using UnityEngine;
using System;

/// <summary>
/// ����̳�������MonoBehavrour��ĵ���ʵ�֣����ֵ���ʵ�������ڼ��ٶԳ������Ĳ�ѯ����
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

                    //�ҽӵ�GameEngine��
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
    /// ɾ������ʵ��,���ּ̳й�ϵ�ĵ�����������Ӧ����ģ����ʾ����
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
    ///  Awake��Ϣ��ȷ������ʵ����Ψһ��
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
    /// OnDestroy��Ϣ��ȷ�������ľ�̬ʵ��������GameObject����
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