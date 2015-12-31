using UnityEngine;
using System.Collections.Generic;
using Network;

public class MenuBar : MonoBehaviour
{

    public UITab[] tabs;
    public GameObject[] panels;

    void Start()
    {
        UIEventListener.Get(tabs [0].gameObject).onClick = (go) => Show(0);
        UIEventListener.Get(tabs [1].gameObject).onClick = (go) => Show(1);
        UIEventListener.Get(tabs [2].gameObject).onClick = (go) => Show(2);
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            NetCommand.Instance.SearchUser("abc321", (w) => {
                Debug.LogError("respond: " + w.text);});
        } else if (Input.GetKeyUp(KeyCode.B))
        {
            NetCommand.Instance.GetItems((w) => 
            {
                Debug.Log("w text: " + w.text);
            });
        }
    }

    private void HideAll()
    {
        foreach (var ite in panels)
        {
            ite.gameObject.SetActive(false);
        }
    }

    private void Show(int index)
    {
        HideAll();
        panels [index].gameObject.SetActive(true);
    }

}
