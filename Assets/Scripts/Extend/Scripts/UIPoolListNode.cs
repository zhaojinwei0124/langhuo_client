using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPoolListNode : MonoBehaviour
{
    static public UIPoolListNode s_current;

    static UICamera s_uiCamera;
    static UICamera CachedCamera
    {
        get
        {
            if (s_uiCamera == null)
                s_uiCamera = UICamera.FindCameraForLayer(LayerMask.NameToLayer("UI"));
            return s_uiCamera;
        }
    }

    protected bool m_initialized = false;
    public bool Initialized { get { return m_initialized; } }
    protected Transform m_cachedTrans;
    public Transform CachedTrans { get { if (m_cachedTrans == null) m_cachedTrans = transform; return m_cachedTrans; } }
    protected BoxCollider m_cachedCollider;
    protected BoxCollider CachedCollider { get { if (m_cachedCollider == null) m_cachedCollider = GetComponent<BoxCollider>(); return m_cachedCollider; } }
    protected BoxCollider2D m_cachedCollider2D;
    protected BoxCollider2D CachedCollider2D { get { if (m_cachedCollider2D == null) m_cachedCollider2D = GetComponent<BoxCollider2D>(); return m_cachedCollider2D; } }
    public float SizeX
    {
        get
        {
            if (CachedCamera.eventType == UICamera.EventType.UI_2D) return CachedCollider2D.size.x;
            else if (CachedCamera.eventType == UICamera.EventType.UI_3D) return CachedCollider.size.x;
            else return 0f;
        }
    }
    public float SizeY
    {
        get
        {
            if (CachedCamera.eventType == UICamera.EventType.UI_2D) return CachedCollider2D.size.y;
            else if (CachedCamera.eventType == UICamera.EventType.UI_3D) return CachedCollider.size.y;
            else return 0f;
        }
    }
    public float CenterX
    {
        get
        {
            if (CachedCamera.eventType == UICamera.EventType.UI_2D) return CachedCollider2D.center.x;
            else if (CachedCamera.eventType == UICamera.EventType.UI_3D) return CachedCollider.center.x;
            else return 0f;
        }
    }
    public float CenterY
    {
        get
        {
            if (CachedCamera.eventType == UICamera.EventType.UI_2D) return CachedCollider2D.center.y;
            else if (CachedCamera.eventType == UICamera.EventType.UI_3D) return CachedCollider.center.y;
            else return 0f;
        }
    }
    protected UIPoolList m_poolList;
    public UIPoolList PoolList { get { if (m_poolList == null) m_poolList = NGUITools.FindInParents<UIPoolList>(gameObject); return m_poolList; } }
    protected List<EventDelegate> m_onClick = new List<EventDelegate>();

    public System.Object m_data;

    public void Initialize(System.Object data)
    {
        m_initialized = true;
        m_data = data;
        Refresh();
    }

    public virtual void Refresh()
    {

    }

    protected virtual void OnClick()
    {
        if (s_current == null && Initialized && enabled)
        {
            s_current = this;
            EventDelegate.Execute(m_onClick);
            s_current = null;
        }
    }
}
