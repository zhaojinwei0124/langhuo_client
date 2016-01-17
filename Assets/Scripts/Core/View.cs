using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;

public abstract class View : MonoBehaviour
{

    private List<int> panelsDepth;

    public UIPanel panel
    {
        get
        {
            return gameObject.GetComponent<UIPanel>();
        }
    }

    public virtual bool HideBack
    {
        get
        {
            return false;
        }
    }

    public virtual void RefreshView()
    {
    }

    public virtual void Refresh(object data)
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

    protected virtual void Close()
    {
        UIHandler.Instance.Pop();
    }

    public void SetDepth(int index)
    {
       // Debug.Log("index:" +index);
        panel.depth=10*index;
    }

    public void BlockInput()
    {
        if (m_panelBlock == null)
        {
            m_panelBlock = CreateFullScreenCollider();
        }
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

}
