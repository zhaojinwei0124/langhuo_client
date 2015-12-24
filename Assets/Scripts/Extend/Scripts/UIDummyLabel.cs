using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class UIDummyLabel : MonoBehaviour
{
    const string FONT_RESOURCE_ROOT = "Fonts/";

    public string m_trueTypeFontName;
    public string m_uiFontName;

    void Awake()
    {
        if (Application.isPlaying)
        {
            ToLabel();
        }
    }

    void FromLabel(UILabel uiLabel)
    {
        if (uiLabel == null) return;
        if (uiLabel.bitmapFont != null)
        {
            m_uiFontName = uiLabel.bitmapFont.name;
            m_trueTypeFontName = "";
        }
        else if (uiLabel.trueTypeFont != null)
        {
            m_trueTypeFontName = uiLabel.trueTypeFont.name;
            m_uiFontName = "";
        }
    }

    UILabel ToLabel()
    {
        UILabel uiLabel = GetComponent<UILabel>();
        if (!string.IsNullOrEmpty(m_uiFontName))
        {
            // TODO: use resource manager
            GameObject fontObj = Resources.Load(FONT_RESOURCE_ROOT + m_uiFontName, typeof(GameObject)) as GameObject;
            if (fontObj == null) return uiLabel;

            UIFont uiFont = fontObj.GetComponent<UIFont>();
            if (uiFont == null) return uiLabel;

            uiLabel.bitmapFont = uiFont;
        }
        else if (!string.IsNullOrEmpty(m_trueTypeFontName))
        {
            // TODO: use resource manager
            Font trueTypeFont = Resources.Load(FONT_RESOURCE_ROOT + m_trueTypeFontName, typeof(Font)) as Font;
            if (trueTypeFont == null) return uiLabel;

            uiLabel.trueTypeFont = trueTypeFont;
        }
        return uiLabel;
    }

    public void RemoveDummy(bool addWidget)
    {
        if (addWidget)
        {
            ToLabel();
        }
        NGUITools.Destroy(this);
    }

    public static UIDummyLabel AttachDummy(UILabel uiLabel, bool doStrip)
    {
        if (uiLabel == null) return null;
        GameObject go = uiLabel.cachedGameObject;
        UIDummyLabel dummyLabel = go.GetComponent<UIDummyLabel>();
        if (dummyLabel == null) dummyLabel = go.AddComponent<UIDummyLabel>();
        dummyLabel.FromLabel(uiLabel);
        if (doStrip)
        {
            uiLabel.bitmapFont = null;
            uiLabel.trueTypeFont = null;
        }
        return dummyLabel;
    }
}
