using UnityEngine;
using System.Collections;
using GameCore;

public class BannerToggle : MonoBehaviour
{


    public UITexture m_txtBanner;
    public UISprite[] tabs;
    private float lastTime;
    private int OFFSET = 0;

    public void Refresh()
    {
        lastTime = Time.time;
        // UIEventListener.Get(m_txtBanner.gameObject).onDrag = Drag;
        UIEventListener.Get(m_txtBanner.gameObject).onPress = Press;
    }

    float x = 0;

    public void Press(GameObject go,bool press)
    {
        if (press)
        {
            x = Input.mousePosition.x;
        } else
        {
            if (Input.mousePosition.x - x > 1f)
            {
                i++;
                OFFSET = 2;
                RefreshUI();
            } else if (Input.mousePosition.x - x < -1f)
            {
                i--;
                if (i < 0)i = 2 ;
                OFFSET = 2;
                RefreshUI();
            }
        }
    }

    void Update()
    {
        if (Time.time - lastTime >= 2 + OFFSET)
        {
            lastTime = Time.time;
            RefreshUI();
            OFFSET = 0;
        }
    }

    int i = 0;

    private void RefreshUI()
    {
        ResourceLoad.TextureHandler.Instance.LoadTexture("banner/banner" + i % 3, (txt) =>
        {
            m_txtBanner.mainTexture = (txt as Texture);
            AutoToggle();
            i++;
        });
    }

    private void AutoToggle()
    {
        for (int j=0; j<3; j++)
        {
            tabs [j].enabled = j == i % 3;
        }
    }
}
