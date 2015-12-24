using UnityEngine;
using System.Collections;

public class IntegerData
{
    public int m_id;
    public bool m_isSelected;
}

public class UIIntegerListNode : UIPoolListNode
{
    [UIComponent("Label")]
    public UILabel m_label;
    [UIComponent("lbSelected")]
    public UIWidget m_selected;
    [UIComponent("Sprite")]
    public UISprite m_sprBG;

    public IntegerData IntData { get { return m_data as IntegerData; } }

    private bool m_isPlayingHighlight = false;

    public override void Refresh()
    {
        m_label.text = IntData.m_id.ToString();
        m_selected.enabled = IntData.m_isSelected;
    }

    public void PlayHighlight()
    {
        if (m_isPlayingHighlight) return;
        m_isPlayingHighlight = true;
        Color bgColor = m_sprBG.color;
        m_sprBG.color = Color.gray;
        TweenColor.Begin(m_sprBG.cachedGameObject, 0.3f, bgColor).SetOnFinished(() => m_isPlayingHighlight = false);
    }
}
