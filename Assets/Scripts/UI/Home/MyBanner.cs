using UnityEngine;
using System.Collections;
using GameCore;


public class MyBanner : MonoBehaviour
{

	[SerializeField]
	private UITexture m_txtBanner;
   

	// Use this for initialization
	void Start () {

        int i=0;
		TimerManager.Instance.AddTimer(2000,0,(seq)=>
		{
            m_txtBanner.mainTexture=Resources.Load("Texture/banner/banner_"+i%3,typeof(Texture2D)) as Texture2D;
            i++;
		});
	}
	
	
}
