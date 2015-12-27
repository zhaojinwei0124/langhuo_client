using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    [SerializeField]
    private UITexture m_txt;

	// Use this for initialization
	void Start () {
        ResourceLoad.Downloader.Instance.LoadAsyncTexture("Texture/Item/caomei",(obj)=>{m_txt.mainTexture=(obj as Texture);});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
