using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class UIDummySprite : MonoBehaviour
{
    const string ATLAS_RESOURCE_ROOT = "Atlases/";

    public string m_atlasName;

    void Awake()
    {
        if (Application.isPlaying)
        {
            ToSprite();
        }
    }

    void FromSprite(UISprite uiSprite)
    {
        if (uiSprite == null || uiSprite.atlas == null) return;
        m_atlasName = uiSprite.atlas.name;
    }

    UISprite ToSprite()
    {
        UISprite uiSprite = GetComponent<UISprite>();

        // TODO: use resource manager
        GameObject atlasObj = Resources.Load(ATLAS_RESOURCE_ROOT + m_atlasName, typeof(GameObject)) as GameObject;
        if (atlasObj == null) return uiSprite;
        UIAtlas uiAtlas = atlasObj.GetComponent<UIAtlas>();
        if (uiAtlas == null) return uiSprite;

        uiSprite.atlas = uiAtlas;

        return uiSprite;
    }

    public void RemoveDummy(bool addWidget)
    {
        if (addWidget)
        {
            ToSprite();
        }
        NGUITools.Destroy(this);
    }

    public static UIDummySprite AttachDummy(UISprite uiSprite, bool doStrip)
    {
        if (uiSprite == null) return null;
        GameObject go = uiSprite.cachedGameObject;
        UIDummySprite dummySprite = go.GetComponent<UIDummySprite>();
        if (dummySprite == null) dummySprite = go.AddComponent<UIDummySprite>();
        dummySprite.FromSprite(uiSprite);
        if (doStrip)
        {
            uiSprite.atlas = null;
        }
        return dummySprite;
    }
}
