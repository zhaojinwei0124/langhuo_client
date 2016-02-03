using UnityEngine;
using System.Collections;
using GameCore;
using Network;


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
        if(!Util.Instance.CheckIDValid(m_id.label.text.Trim()))
        {
            Toast.Instance.Show(10052);
            return;
        }
       
        NetCommand.Instance.Apply(1,(str)=> {Toast.Instance.Show(10050); Close();});
       
    }


}
