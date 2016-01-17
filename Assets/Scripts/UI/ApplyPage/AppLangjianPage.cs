using UnityEngine;
using System.Collections;

public class AppLangjianPage : View {

    public UIInput m_id;
    public UIInput m_name;
    public UIInput m_pos;
    public GameObject m_objApply;


    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_objApply).onClick=OnApply;
    }



    private void OnApply(GameObject go)
    {
        Toast.Instance.Show(10050);
        Close();
    }


}
