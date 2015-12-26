using UnityEngine;
using System.Collections.Generic;
using Network;


public class MenuBar : MonoBehaviour 
{

    public UITab[] tabs;

    public GameObject[] panels;

	void Start ()
    {
        UIEventListener.Get(tabs[0].gameObject).onClick=(go)=>Show(0);
        UIEventListener.Get(tabs[1].gameObject).onClick=(go)=>Show(1);
        UIEventListener.Get(tabs[2].gameObject).onClick=(go)=>Show(2);
	}
	
	void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            CommandAPI.Instance.SearchUser("abc","abc321",(w)=>{Debug.LogError("respond: "+w.text);});
        }
        else if(Input.GetKeyUp(KeyCode.B))
        {
            TextAsset txt=Resources.Load("Tables/test",typeof(TextAsset)) as TextAsset;
            Debug.LogError("txt is: "+txt.text);
            List<object> list =  MiniJSON.Json.Deserialize(txt.text) as List<object>;
            Debug.Log("list cnt: "+list.Count);
        }
    }

    private void HideAll()
    {
        foreach(var ite in panels)
        {
            ite.gameObject.SetActive(false);
        }
    }


    private void Show(int index)
    {
        HideAll();
        panels[index].gameObject.SetActive(true);
    }

}
