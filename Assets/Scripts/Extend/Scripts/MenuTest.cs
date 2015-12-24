using UnityEngine;
using System.Collections;

public class MenuTest : MonoBehaviour
{
    [UIComponent("PanelContents/labTitle", UIComponentAttribute.E_KeyType.Hierarchy)]
    public UILabel m_lbTitle;
    [UIComponent("sprArmor", UIComponentAttribute.E_KeyType.Name)]
    public UISprite m_sprArmor;
    [UIComponent("Button", UIComponentAttribute.E_KeyType.Name)]
    public UIButton m_button;
    [UIComponent("PanelContents/Button/Label", UIComponentAttribute.E_KeyType.Hierarchy)]
    public UILabel m_lbButton;

    private bool m_isInitialized = false;
    private string[] m_spriteNames = new string[]
    {
        "Orc Armor - Shoulders",
        "Orc Armor - Bracers",
        "Orc Armor - Boots"
    };
    private int m_spriteIndex = 0;

    void OnEnable()
    {
        Debug.Log("MenuTest.Initialize()");
        m_isInitialized = UIComponentAttribute.InitComponents(this.gameObject, this);
        if (!m_isInitialized)
        {
            Debug.LogError("Initialize failed!");
            return;
        }
        m_button.onClick.Add(new EventDelegate() { target = this, methodName = "OnButtonClick"});
        m_lbButton.text = "Change!";
    }

    void Update()
    {
        if (!m_isInitialized) return;
        m_lbTitle.text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    void OnButtonClick()
    {
        if (!m_isInitialized) return;
        m_spriteIndex = (m_spriteIndex + 1) % m_spriteNames.Length;
        m_sprArmor.spriteName = m_spriteNames[m_spriteIndex];
        m_sprArmor.MakePixelPerfect();
    }
}
