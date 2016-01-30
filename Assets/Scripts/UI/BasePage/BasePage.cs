using UnityEngine;
using System.Collections.Generic;
using Network;
using Config;
using GameCore;

public class BasePage : View
{

    public UIPopupList m_poplist;
    public UILabel m_lblBase;
    public UILabel m_lblName;
    public GameObject m_objOK;
    private int m_Select = -1;
    private List<TBases> m_bases;

    public override void RefreshView()
    {
        base.RefreshView();

        UIEventListener.Get(m_objOK).onClick = OnClickBtn;

        UIEventListener.Get(m_poplist.gameObject).onClick = OnPopClick;

        if (GameBaseInfo.Instance.user != null)
        {
            RefreshUI();
        } 
        else if(!PlayerPrefs.HasKey(PlayerprefID.USERID))
        {
            Close();
            UIHandler.Instance.Push(PageID.Regist);
        }
        else
        {
            NetCommand.Instance.LoginUser(PlayerPrefs.GetString(PlayerprefID.USERID), (string res) =>
            {
                NUser nuser = Util.Instance.Get<NUser>(res);
                GameBaseInfo.Instance.user = nuser;
                RefreshUI();
            });
        }
    }

    private void OnPopClick(GameObject go)
    {
        Debug.Log("poplist click");
    }

    private void RefreshUI()
    {
        m_lblName.text = GameBaseInfo.Instance.user.name;
        
        RefreshPopList();
    }

    private void RefreshPopList()
    {
        string city = GameBaseInfo.Instance.address.city;
        List<TBases> bases = Tables.Instance.GetTable<List<TBases>>(TableID.BASE);
        m_bases = bases.FindAll(x => x.city == city);
        if (m_bases == null || m_bases.Count <= 0)
        {
            Toast.Instance.Show(10070);
            Debug.LogError("not such base");
            return;
        }

        m_poplist.items = m_bases.ConvertAll<string>(x => x.district + " " + x.name);
     
    }

    public void OnPopChange()
    {
        m_lblBase.text = m_poplist.GetSelect();
        m_Select = m_poplist.GetIndex();
    }

    private void OnClickBtn(GameObject go)
    {
        if (m_Select < 0 || m_bases==null || m_bases.Count<=0)
        {
            Toast.Instance.Show(10068);
        } else
        {
            PlayerPrefs.SetInt(PlayerprefID.BASE, m_bases [m_Select].id);
            Debug.Log("select: "+m_bases[m_Select].name);
            GameBaseInfo.Instance.user.bases=m_bases[m_Select].id;
            NetCommand.Instance.UpdateBase(m_bases [m_Select].id, (res) =>
            {
                Toast.Instance.Show(10069);
                Close();
            });
        }
    }

}
