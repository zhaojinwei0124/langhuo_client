using UnityEngine;

public class Loadding : MonoSingle<Loadding>
{

    public UISpriteAnimation loadAnim;

    public void Show(bool show)
    {
        loadAnim.gameObject.SetActive(show);

        if (show)
        {
            loadAnim.ResetToBeginning();
        }
    }
}

