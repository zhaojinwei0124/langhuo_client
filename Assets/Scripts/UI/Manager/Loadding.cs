using UnityEngine;

public class Loadding : MonoSingle<Loadding>
{

    public UISpriteAnimation loadAnim;

    public GameObject offsetObj;

    public void Show(bool show)
    {
        offsetObj.gameObject.SetActive(show);

        if (show)
        {
            loadAnim.ResetToBeginning();
        }
    }
}

