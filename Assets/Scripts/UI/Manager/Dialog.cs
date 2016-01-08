using UnityEngine;

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

    public void Show(int local,UIEventListener.VoidDelegate okDel)
    {
        Show(Localization.Get(local), okDel, null);
    }

    public void Show(string str, UIEventListener.VoidDelegate okDel)
    {
        Show(str, okDel, null);
    }


    public void Show(int local, UIEventListener.VoidDelegate okDel, UIEventListener.VoidDelegate cancelDel)
    {
        Show(Localization.Get(local),okDel,cancelDel);
    }

    public void Show(string str, UIEventListener.VoidDelegate okDel, UIEventListener.VoidDelegate cancelDel)
    {
        offsetObj.SetActive(true);
        contentLabel.text = str;

        UIEventListener.Get(okBtn).onClick = (go) =>
        {
            if (okDel != null)
                okDel(go);
            offsetObj.SetActive(false);
        };

        UIEventListener.Get(cancelBtn).onClick = (go) =>
        {
            if (cancelDel != null)
                cancelDel(go);
            offsetObj.SetActive(false);
        };
    }
   

}

