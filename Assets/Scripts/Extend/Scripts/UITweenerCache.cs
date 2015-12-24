using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UITweenerCache : MonoBehaviour
{
    [SerializeField]
    UITweener[] m_tweeners;
    [SerializeField]
    bool m_refreshNow;

    public UITweener[] Tweeners
    {
        get { return m_tweeners; }
    }

    void Update()
    {
        if (m_refreshNow)
        {
            m_refreshNow = false;

            if (Application.isPlaying)
                return;

            m_tweeners = GetComponentsInChildren<UITweener>();
        }
    }

    public void FlyTo(bool forward)
    {
        foreach (UITweener tw in Tweeners)
        {
            tw.tweenFactor = forward ? 1f : 0f;
            tw.Sample(tw.tweenFactor, false);
        }
    }

    public void Play(bool forward)
    {
        foreach (UITweener tw in Tweeners)
            tw.Play(forward);
    }
}
