using UnityEngine;
using System.Collections.Generic;
using Config;

public class TypePage : View
{

    public TypeNode[] m_types;

    public override void RefreshView()
    {
        base.RefreshView();
        TType[] types = Tables.Instance.GetTable<TType[]>(TableID.TYPE);
        for(int i=0;i<m_types.Length;i++)
        {
            m_types[i].Refresh(i,types[i]);
        }
    }

    public void OnItemClick(int index)
    {
        Debug.Log("index: " + index);
        Close();
    }

}
