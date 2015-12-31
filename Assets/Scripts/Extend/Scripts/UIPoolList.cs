using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIPanel))]
[RequireComponent(typeof(UIScrollView))]
public class UIPoolList : MonoBehaviour
{
    public enum E_DragDirection
    {
        Horizontal = 0,
        Vertical,
    }

    public enum E_ScrollOption
    {
        ResetToZero = 0,
        KeepCurrentPosition,
    }

    public E_DragDirection m_direction = E_DragDirection.Vertical;
    public int m_columnCount = 1;
    public Transform m_transNodeRoot;
    public Transform m_prototype;
    public UIWidget m_sprHead;
    public UIWidget m_sprTail;
    public System.Action<int> m_onPullRefresh;

    public IList<System.Object> m_dataArray = new System.Object[0];
    [HideInInspector]
    public List<UIPoolListNode> m_dynamicNodes = new List<UIPoolListNode>();

    protected Transform m_trans;
    public Transform CachedTrans { get { if (m_trans == null) m_trans = transform; return m_trans; } }
    protected UIPanel m_panel;
    public UIPanel CachedPanel { get { if (m_panel == null) m_panel = GetComponent<UIPanel>(); return m_panel; } }
    protected UIScrollView m_scrollView;
    public UIScrollView CachedScrollView { get { if (m_scrollView == null) m_scrollView = GetComponent<UIScrollView>(); return m_scrollView; } }
    protected UIPoolListNode m_prototypeNode;
    public UIPoolListNode PrototypeNode { get { if (m_prototypeNode == null) m_prototypeNode = m_prototype.gameObject.GetComponent<UIPoolListNode>(); return m_prototypeNode; } }

    protected bool m_initialized = false;
    public bool IsInitialized { get { return m_initialized; } }
    protected int m_poolSize = 0;
    protected int m_maxLine = 0;
    protected int m_firstIndexMax = 0;
    protected int m_firstIndexLastFrame = -1;
    protected int m_lastPull = 0;

    public void Initialize(IList<System.Object> dataArray, E_ScrollOption scrollOption = E_ScrollOption.ResetToZero)
    {
        m_initialized = true;
        m_dataArray = dataArray;

        m_transNodeRoot.localPosition = Vector3.zero;
        if (m_dataArray == null || m_dataArray.Count < 1)
        {
            m_transNodeRoot.SetChildCount(0);
            m_poolSize = 0;
            m_maxLine = 0;
            m_firstIndexMax = 0;
            SetHeadAndTailSpritePosition();
            CachedScrollView.ResetPosition();
            return;
        }

        int visibleLines = GetVisibleLines();
        m_poolSize = Mathf.Min(m_dataArray.Count, visibleLines * m_columnCount);
        m_maxLine = Mathf.Max(0, (m_dataArray.Count - 1) / m_columnCount);
        m_firstIndexMax = Mathf.Max(0, m_dataArray.Count - m_poolSize);

        // calculate the position of the head and the tail
        SetHeadAndTailSpritePosition();
        m_prototype.gameObject.SetActive(true);
        // initialize prototypes
        m_transNodeRoot.SetChildCount(m_poolSize, m_prototype);
        m_prototype.gameObject.SetActive(false);
        // add prototypes into dynamic item pool
        m_dynamicNodes.Clear();
        for (int i = 0; i < m_poolSize; i++)
        {
            Transform childTrans = m_transNodeRoot.GetChild(i);
            childTrans.gameObject.SetActive(true);
            UIPoolListNode uiItem = childTrans.GetComponent<UIPoolListNode>();
            m_dynamicNodes.Add(uiItem);
        }

        m_firstIndexLastFrame = -1;
        if (scrollOption == E_ScrollOption.ResetToZero) {
            CachedScrollView.ResetPosition ();
            UpdateItems(0);
        } else {
            if (gameObject.activeInHierarchy)
                StartCoroutine (RestrictWithInBounds ());
            UpdateItems(GetFirstVisibleIndex());
        }
        //CachedScrollView.onDragFinished = OnPanelDragFinished;
        
        m_lastPull = 0;
    }

    IEnumerator RestrictWithInBounds()
    {
        yield return null;
        if (CachedScrollView.restrictWithinPanel && CachedPanel.clipping != UIDrawCall.Clipping.None)
            CachedScrollView.RestrictWithinBounds(CachedScrollView.dragEffect == UIScrollView.DragEffect.None, CachedScrollView.canMoveHorizontally, CachedScrollView.canMoveVertically);
    }

    public void Refresh()
    {
        foreach (UIPoolListNode node in m_dynamicNodes)
        {
            if (node != null)
                node.Refresh();
        }
    }

    protected void Update()
    {
        if (!m_initialized) return;

        int firstVisibleIndex = GetFirstVisibleIndex();
        if (firstVisibleIndex != m_firstIndexLastFrame)
        {
            UpdateItems(firstVisibleIndex);
            return;
        }
    }

    protected void UpdateItems(int firstVisibleIndex)
    {
        int lastVisibleIndex = firstVisibleIndex + m_poolSize;
        // find out nodes to be updated
        List<UIPoolListNode> nodesToUpdate = new List<UIPoolListNode>(m_dynamicNodes);
        List<System.Object> dataToUpdate = new List<System.Object>();
        List<int> indexes = new List<int>();
        for (int i = firstVisibleIndex; i < lastVisibleIndex; i++)
        {
            System.Object data = m_dataArray[i];
            UIPoolListNode existingNode = null;
            if (data != null)
            {
                existingNode = nodesToUpdate.Find(x => data.Equals(x.m_data));
            }
            if (existingNode != null && existingNode.Initialized)
            {
                nodesToUpdate.Remove(existingNode);
                existingNode.CachedTrans.localPosition = GetPositionByIndex(i) + NodeOffset;
            }
            else
            {
                dataToUpdate.Add(data);
                indexes.Add(i);
            }
        }
        for (int i = 0; i < dataToUpdate.Count; i++)
        {
            UIPoolListNode node = nodesToUpdate[i];
            System.Object data = dataToUpdate[i];
            node.Initialize(data);
            node.name = "item" + i;
            node.CachedTrans.localPosition = GetPositionByIndex(indexes[i]) + NodeOffset;
        }
        m_firstIndexLastFrame = firstVisibleIndex;
    }

    protected int GetVisibleLines()
    {
        return (m_direction == E_DragDirection.Vertical)
            ? (Mathf.FloorToInt(CachedPanel.baseClipRegion.w / PrototypeNode.SizeY) + 2)
            : (Mathf.FloorToInt(CachedPanel.baseClipRegion.z / PrototypeNode.SizeX) + 2);
    }

    protected void SetHeadAndTailSpritePosition()
    {
        if (m_direction == E_DragDirection.Vertical)
        {
            Vector3 pos = GetPositionByIndex(0);
            pos.x = 0;
            m_sprHead.pivot = UIWidget.Pivot.Top;
            m_sprHead.width = (int)PrototypeNode.SizeX;
            m_sprHead.height = (int)PrototypeNode.SizeY;
            m_sprHead.cachedTransform.localPosition = pos;

            pos = GetPositionByIndex(Mathf.Max(0, m_dataArray != null ? (m_dataArray.Count - 1) : 0));
            pos.x = 0;
            pos.y -= PrototypeNode.SizeY;
            m_sprTail.pivot = UIWidget.Pivot.Bottom;
            m_sprTail.width = (int)PrototypeNode.SizeX;
            m_sprTail.height = (int)PrototypeNode.SizeY;
            m_sprTail.cachedTransform.localPosition = pos;
        }
        else
        {
            Vector3 pos = GetPositionByIndex(0);
            pos.y = 0;
            m_sprHead.pivot = UIWidget.Pivot.Left;
            m_sprHead.width = (int)PrototypeNode.SizeX;
            m_sprHead.height = (int)PrototypeNode.SizeY;
            m_sprHead.cachedTransform.localPosition = pos;

            pos = GetPositionByIndex(Mathf.Max(0, m_dataArray != null ? (m_dataArray.Count - 1) : 0));
            pos.y = 0;
            pos.x += PrototypeNode.SizeX;
            m_sprTail.pivot = UIWidget.Pivot.Right;
            m_sprTail.width = (int)PrototypeNode.SizeX;
            m_sprTail.height = (int)PrototypeNode.SizeY;
            m_sprTail.cachedTransform.localPosition = pos;
        }
    }

    protected Vector3 GetPositionByIndex(int index)
    {
        float width = PrototypeNode.SizeX;
        float height = PrototypeNode.SizeY;
        if (m_direction == E_DragDirection.Vertical)
        {
            float x = (index % m_columnCount) * width - width * (m_columnCount - 1) * 0.5f;
            float y = (index / m_columnCount) * (-height) + PanelOffset;
            return new Vector3(x, y, 0f);
        }
        else
        {
            float y = (index % m_columnCount) * (-height) + height * (m_columnCount - 1) * 0.5f;
            float x = (index / m_columnCount) * width + PanelOffset;
            return new Vector3(x, y, 0f);
        }
    }

    protected int GetFirstVisibleIndex()
    {
        if (m_direction == E_DragDirection.Vertical)
        {
            float posY = -CachedPanel.clipSoftness.y - CachedPanel.clipOffset.y;
            float height = PrototypeNode.SizeY;
            int curLine = Mathf.Clamp(Mathf.FloorToInt(posY / height), 0, m_maxLine);
            return Mathf.Clamp(curLine * m_columnCount, 0, m_firstIndexMax);
        }
        else
        {
            float posX = -CachedPanel.clipSoftness.x + CachedPanel.clipOffset.x;
            float width = PrototypeNode.SizeX;
            int curLine = Mathf.Clamp(Mathf.FloorToInt(posX / width), 0, m_maxLine);
            return Mathf.Clamp(curLine * m_columnCount, 0, m_firstIndexMax);
        }
    }

    protected Vector3 NodeOffset
    {
        get
        {
            if (m_direction == E_DragDirection.Vertical)
            {
                return new Vector3(0, -PrototypeNode.SizeY * 0.5f - PrototypeNode.CenterY, 0);
            }
            else
            {
                return new Vector3(PrototypeNode.SizeX * 0.5f - PrototypeNode.CenterX, 0, 0);
            }
        }
    }

    protected float PanelOffset
    {
        get
        {
            if (m_direction == E_DragDirection.Vertical)
            {
                return CachedPanel.baseClipRegion.w * 0.5f - CachedPanel.clipSoftness.y + CachedPanel.baseClipRegion.y;
            }
            else
            {
                return -CachedPanel.baseClipRegion.z * 0.5f + CachedPanel.clipSoftness.x - CachedPanel.baseClipRegion.x;
            }
        }
    }
}
