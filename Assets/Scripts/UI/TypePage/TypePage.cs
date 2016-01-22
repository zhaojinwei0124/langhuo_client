using UnityEngine;
using System.Collections.Generic;
using Config;

public class TypePage : View
{
	public Transform m_child;

	public Transform m_parent;

	public UIGrid m_grid;


    public override void RefreshView()
    {
        base.RefreshView();
        TType[] types = Tables.Instance.GetTable<TType[]>(TableID.TYPE);

		m_parent.SetChild(types.Length,m_child);

		m_parent.SetChild<TypeNode> (types.Length, m_child, (index,node) =>
		{
			node.Refresh(index,types[index]);
		});

		m_grid.repositionNow = true;
    }

    public void OnItemClick(int index)
    {
        Debug.Log("index: " + index);
        Close();
    }

}
