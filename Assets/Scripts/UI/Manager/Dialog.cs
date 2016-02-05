using UnityEngine;
using GameCore;

public class Dialog : MonoSingle<Dialog>
{
    public GameObject okBtn, cancelBtn;
    public UILabel contentLabel;
    public GameObject offsetObj;

    public void Show(int local)
    {
        Show(Localization.Get(local));
    }


    public void Show(string str)
    {
        Show(str, null);
    }

    public void Show(int local,DelManager.VoidDelegate okDel)
    {
        Show(Localization.Get(local), okDel, null);
    }

    public void Show(string str, DelManager.VoidDelegate okDel)
    {
        Show(str, okDel, null);
    }


    public void Show(int local, DelManager.VoidDelegate okDel, DelManager.VoidDelegate cancelDel)
    {
        Show(Localization.Get(local),okDel,cancelDel);
    }

    public void Show(string str, DelManager.VoidDelegate okDel, DelManager.VoidDelegate cancelDel)
    {
        offsetObj.SetActive(true);
        contentLabel.text = str;

        UIEventListener.Get(okBtn).onClick = (g) =>
        {
            if (okDel != null)
                okDel();
            offsetObj.SetActive(false);
        };

        UIEventListener.Get(cancelBtn).onClick = (g) =>
        {
            if (cancelDel != null)
                cancelDel();
            offsetObj.SetActive(false);
        };
    }
   

}

