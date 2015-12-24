using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;

public class View : MonoBehaviour
{

	[NonSerialized]private UIComponentDict components;
	[NonSerialized]protected int Depth;
	private List<int> panelsDepth;

    public object Param { get; set; }

	public virtual bool HideBack
	{
		get
		{
			return false;
		}
	}

	public virtual bool FullScreen
	{
		get
		{
			return true;
		}
	}

	public virtual bool History
	{
		get
		{
			return true;
		}
	}

	public virtual bool SpaceClose
	{
		get 
		{ 
			return FullScreen?false:true; 
		}
	}
	public UIComponentDict Components
	{
		get
		{
			if(components == null)
			{
				components = GetComponent<UIComponentDict>();
			}
			return components;
		}
	}

	public UIComponentDict PanelParent
	{
		set
		{
			for (int i=0; i< Components.m_panelList.Count; i++)
			value.m_panelList.Add (Components.m_panelList [i]);
		}
	}

	public void SetDepth(int depth)
	{
		if(panelsDepth == null || Components.m_panelList.Count != panelsDepth.Count)
		{
			panelsDepth = new List<int>();
			for(int i = 0; i < Components.m_panelList.Count; i++)
			{
				panelsDepth.Add( Components.m_panelList[i].depth);
			}
		}
		for(int i = 0; i < Components.m_panelList.Count; i++)
		{
			Components.m_panelList[i].depth = panelsDepth[i]%10 + depth;
		}
		Depth = depth;
	}

	public int GetDepth ()
	{
		return Depth;
	}
	
	public void SetAlpha(float alpha)
	{
		foreach(UIPanel panel in Components.m_panelList)
		{
			panel.alpha = alpha;
			break;
		}
	}


	private bool m_isDirty = false;
    public void DoRefreshView()
    {
		if (!m_isDirty && gameObject.activeSelf)
		{
            StartCoroutine(SetDirty());
		}
        RefreshView(); 
    }

  
    IEnumerator SetDirty()
    {
        m_isDirty = true;
        yield return null;
        if (Components != null)
        {
            foreach (UIPanel panel in Components.m_panelList)
            {
                if (panel.cachedGameObject.activeInHierarchy && panel.enabled)
                    panel.Refresh();
            }
        }
        m_isDirty = false;
    }

	public virtual void RefreshView()
	{

	}

    public virtual void Refresh(object data) { }

	public virtual void CloseView()
	{
		
	}

	public virtual void UnFocusView()
	{
		
	}

    UIPanel CreateFullScreenCollider()
    {
        Transform parentTrans = transform;
        Transform panelTrans = new GameObject("PanelBlock").transform;
        panelTrans.parent = parentTrans;
        panelTrans.localPosition = Vector3.zero;
        panelTrans.localRotation = Quaternion.identity;
        panelTrans.localScale = Vector3.one;
        panelTrans.gameObject.layer = parentTrans.gameObject.layer;
        UIPanel uiPanel = panelTrans.gameObject.AddComponent<UIPanel>();
        Transform trans = new GameObject("ModalCollider").transform;
        trans.parent = panelTrans;
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
        trans.gameObject.layer = panelTrans.gameObject.layer;
        UIWidget widget = trans.gameObject.AddComponent<UIWidget>();
        widget.autoResizeBoxCollider = true;
        BoxCollider2D bc = trans.gameObject.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        bc.center = Vector3.zero;
        bc.size = new Vector3(1, 1);
        UIStretch stretch = trans.gameObject.AddComponent<UIStretch>();
        stretch.uiCamera = UICamera.FindCameraForLayer(LayerMask.NameToLayer("UI")).camera;
        stretch.style = UIStretch.Style.Both;
        return uiPanel;
    }

    private UIPanel m_panelBlock;

    public void BlockInput()
    {
        if (m_panelBlock == null)
        {
            m_panelBlock = CreateFullScreenCollider();
            SetColliderDepth();
        }
        m_panelBlock.cachedGameObject.SetActive(true);
    }

    public void OpenInput()
    {
        if (m_panelBlock != null)
            m_panelBlock.cachedGameObject.SetActive(false);
    }

    public bool IsInputBlocked
    {
        get
        {
            return (m_panelBlock != null) ? m_panelBlock.cachedGameObject.activeSelf : false;
        }
    }

    void SetColliderDepth()
    {
        if (Components != null)
        {
            Components.m_panelList.Add(m_panelBlock);
            m_panelBlock.depth = GetMaxDepth() + 1;
        }
    }

    public int GetMaxDepth()
    {
        int maxDepth = 0;
        foreach (UIPanel panel in Components.m_panelList)
        {
            int curDepth = panel.depth;
            if (curDepth > maxDepth) maxDepth = curDepth;
        }
        return maxDepth;
    }
}
