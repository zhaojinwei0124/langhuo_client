using UnityEngine;
using System.Collections;

public class HeadIconPage : View
{

    public UITexture[] icons;
   
    public override void RefreshView()
    {
        base.RefreshView();

        for (int i=0; i<icons.Length; i++)
        {
            icons[i].gameObject.name=icons[i].mainTexture.name;
            UIEventListener.Get(icons [i].gameObject).onClick = (g) => SaveIcon(g.name);
        }
    }

    private void SaveIcon(string name)
    {
        Debug.Log("name: "+name);
        PlayerPrefs.SetString(GameCore.PlayerprefID.HEADICO, name);
        Close();
    }

}
