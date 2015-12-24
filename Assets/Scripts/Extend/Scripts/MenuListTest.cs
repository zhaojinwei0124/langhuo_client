using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuListTest : MonoBehaviour
{
    [UIComponent("PanelScroll")]
    private UIPoolList m_poolList;
    [UIComponent("btnSelectAll")]
    public UIButton m_btnSelectAll;
    [UIComponent("btnReverse")]
    public UIButton m_btnReverse;
    [UIComponent("btnDelete")]
    public UIButton m_btnDelete;

    private bool m_isInitialized = false;

    void Awake()
    {
        Debug.Log("MenuListTest.Initialize()");
        m_isInitialized = UIComponentAttribute.InitComponents(this.gameObject, this);
        if (!m_isInitialized)
        {
            Debug.LogError("Initialize failed!");
            return;
        }

        UIIntegerListNode node = m_poolList.m_prototype.gameObject.AddComponent<UIIntegerListNode>();
        node.m_onClick.Add(new EventDelegate(OnNodeClicked));
        UIComponentAttribute.InitComponents(m_poolList.m_prototype.gameObject, node);
        m_poolList.m_prototype.gameObject.SetActive(false);

        EventDelegate.Set(m_btnSelectAll.onClick, OnSelectAll);
        EventDelegate.Set(m_btnReverse.onClick, OnReverse);
        EventDelegate.Set(m_btnDelete.onClick, OnDelete);
    }

    void OnEnable()
    {
        int size = 1000;
        IntegerData[] dataArray = new IntegerData[size];
        for (int i = 0; i < size; i++)
        {
            dataArray[i] = new IntegerData() { m_id = i + 1 };
        }
        m_poolList.Initialize(dataArray, UIPoolList.E_ScrollOption.KeepCurrentPosition);
    }

    void OnNodeClicked()
    {
        UIIntegerListNode node = UIPoolListNode.s_current as UIIntegerListNode;
        if (node == null) return;
        node.IntData.m_isSelected = !node.IntData.m_isSelected;
        node.Refresh();
        node.PlayHighlight();
    }

    void OnSelectAll()
    {
        foreach (IntegerData data in m_poolList.m_dataArray)
        {
            data.m_isSelected = true;
        }
        m_poolList.Refresh();
    }

    void OnReverse()
    {
        foreach (IntegerData data in m_poolList.m_dataArray)
        {
            data.m_isSelected = !data.m_isSelected;
        }
        m_poolList.Refresh();
    }

    void OnDelete()
    {
        List<IntegerData> list = new List<IntegerData>(m_poolList.m_dataArray.Select(x => x as IntegerData));
        list.RemoveAll(x => x.m_isSelected);
        m_poolList.Initialize(list.ToArray(), UIPoolList.E_ScrollOption.KeepCurrentPosition);
    }
}
