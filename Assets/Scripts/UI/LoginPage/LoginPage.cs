using UnityEngine;
using System.Collections;
using Network;
using GameCore;
using Platform;

public class LoginPage : View
{
    public UIInput m_user;
    public UILabel m_type;
    public UIPopupList m_poplist;
    public UIInput m_address;
    public UIInput m_tel;
    public UIInput m_code;
    public GameObject m_objRegist;
    public UILabel m_lblMsg;
    private PayType payType = PayType.LANGJIAN;

    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_objRegist).onClick = OnRegist;
        UIEventListener.Get(m_lblMsg.gameObject).onClick = OnMsgClick;
    }

    const int TIMER = 60;
    int timerCnt = 60;
    int timerSeq = -1;
    int randCode = 1008;

    protected override void Close()
    {
        if (timerSeq < 0)
        {
            m_lblMsg.text=Localization.Get(10015);
            TimerManager.Instance.RemoveTimer(timerSeq);
        }
        base.Close();
    }

    private void OnMsgClick(GameObject go)
    {
        if (timerCnt < TIMER -1 && timerCnt > 0)
        {
            Debug.LogError("timer cnt: "+timerCnt);
            return;
        }
        if (CheckValid())
        {
            TimerManager.Instance.AddTimer(1000, TIMER, (seq) =>
            {
                m_lblMsg.text = timerCnt + "s";
                if (timerCnt >= 0)
                {
                    timerCnt--;
                } else
                {
                    if (timerSeq < 0)
                    {
                        TimerManager.Instance.RemoveTimer(timerSeq);
                        m_lblMsg.text=Localization.Get(10015);
                    }
                }
            });


            randCode = Random.Range(1001, 9999);
            SimApi.Instance.Sms(randCode, m_tel.label.text, (string msg) =>
            {
                Debug.Log(10012);
            }, (string err) =>
            {
                Toast.Instance.Show(10013);
            });

        } else
        {
            Toast.Instance.Show(Localization.Get(100002));
        }
    }

    private bool CheckValid()
    {
        if (m_tel.label.text.Length != 11)
        {
            Debug.LogError("tel is not valid!");
            return false;
        }
        return true;
    }

    private void OnRegist(GameObject go)
    {
        if (!CheckValid())
        {
            Toast.Instance.Show(10017);
            return;
        }
        if (timerCnt <= 0)
        {
            Toast.Instance.Show(10014);
            return;
        }
        int code =1000;
        if(!int.TryParse(m_code.label.text.Trim(),out code))
        {
            Debug.LogError("msg: "+m_code.label.text+" code: "+code);
        }

        if(randCode != code)
        {
            Toast.Instance.Show(10016);
            return;
        }
        Debug.Log("regist msg success");
        NetCommand.Instance.RegistUser(m_user.label.text, m_tel.label.text, (int)GameBaseInfo.Instance.payMode, 
                                           m_address.label.text, payType, (res) =>
        {
            Debug.Log("res: " + res);
            if (res.Equals("true"))
            {
                PlayerPrefs.SetString(PlayerprefID.USERID, m_tel.label.text);
                Debug.Log("regist use success!");
                Close();
            }
            else
            {
                Debug.LogError("show msg: "+res);
            }
        });
    }
}
