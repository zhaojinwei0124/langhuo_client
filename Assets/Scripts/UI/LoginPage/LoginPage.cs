using UnityEngine;
using System.Collections;
using GameCore;
using Platform;
using Network;

public class LoginPage : View
{
    public UIInput m_tel;
    public UIInput m_code;
    public GameObject m_objLogin;
    public UILabel m_lblMsg;
    const int TIMER = 60;
    int timerCnt = 60;
    int timerSeq = -1;
    int randCode = 1008;

    public override void RefreshView()
    {
        base.RefreshView();
        UIEventListener.Get(m_objLogin).onClick = OnLogin;
        UIEventListener.Get(m_lblMsg.gameObject).onClick = OnMsgClick;
    }

    protected override void Close()
    {
        m_lblMsg.text = Localization.Get(10015);
        TimerManager.Instance.RemoveTimer(timerSeq);
        base.Close();
    }

    private void OnLogin(GameObject go)
    { 
        if (!Util.Instance.CheckPhoneValid(m_tel.label.text.Trim()))
        {
            Toast.Instance.Show(10017);
            return;
        }
        if (timerCnt <= 0)
        {
            Toast.Instance.Show(10014);
            return;
        } 
        int code = 1000;
        if (!int.TryParse(m_code.label.text.Trim(), out code))
        {
            Debug.LogError("msg: " + m_code.label.text + " code: " + code);
        }
#if !SMS
        if (randCode != code)
        {
            Toast.Instance.Show(10016);
            return;
        }
#endif
        NetCommand.Instance.LoginUser(m_tel.label.text, (res) =>
        {
            PlayerPrefs.SetString(PlayerprefID.USERID, m_tel.label.text);
            NUser nuser = Util.Instance.Get<NUser>(res);
            GameBaseInfo.Instance.user = nuser;
            Debug.Log("regist use success!");
            Close();
        });
    }

    private void OnMsgClick(GameObject go)
    {
        if (timerCnt < TIMER - 1 && timerCnt > 0)
        {
            Debug.LogError("timer cnt: " + timerCnt);
            return;
        }
        if (Util.Instance.CheckPhoneValid(m_tel.label.text.Trim()))
        {
            m_lblMsg.text = timerCnt.ToString();
            timerSeq = TimerManager.Instance.AddTimer(1000, TIMER, (seq) =>
            {
                m_lblMsg.text = timerCnt.ToString();
                if (timerCnt >= 0)
                {
                    timerCnt--;
                } else
                {
                    TimerManager.Instance.RemoveTimer(timerSeq);
                    m_lblMsg.text = Localization.Get(10015);
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


}
