using UnityEngine;
using System.Collections;
using Network;
using GameCore;

public class AppLangtiPage : View {

    public UIInput m_id;
    public UIInput m_name;
    public UIInput m_nick;
    public GameObject m_objApply;
    
    
    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_objApply).onClick=OnApply;
        m_nick.label.text=GameBaseInfo.Instance.user.name;
    }

    
    private void OnApply(GameObject go)
    {
        if(!Util.Instance.CheckIDValid(m_id.label.text.Trim()))
        {
            Toast.Instance.Show(10052);
            return;
        }

        Toast.Instance.Show(10050);
        Close();
    }
}
