using UnityEngine;
using System.Collections;
using Config;

public class TypeNode : MonoBehaviour
{
    private int mIndex;
	private int mType;

	public UILabel m_label;


    public void Refresh(int index,TType type)
    {
        TType[] types = Tables.Instance.GetTable<TType[]>(TableID.TYPE);
        mIndex = index;
		mType = type.type;
        if (m_label != null)
        {
            m_label.text = type.name;
        } else
        {
            Debug.LogError("mlabel is null, index: " + index);
        }
    }

    public void OnClick()
    {
        Debug.Log("index: " + mIndex);
        TypePage page = NGUITools.FindInParents<TypePage>(gameObject);
        page.OnItemClick(mIndex);
    }
    
}
