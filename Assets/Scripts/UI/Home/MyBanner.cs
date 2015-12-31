using UnityEngine;
using System.Collections;
using GameCore;


public class MyBanner : MonoBehaviour
{

	[SerializeField]
	private UITexture m_txtBanner;
   

	void Start () {

        int i=0;
		TimerManager.Instance.AddTimer(2000,0,(seq)=>
		{
            ResourceLoad.TextureHandler.Instance.LoadTexture("Texture/banner/banner"+i%3,(txt)=>
                                                             {
                m_txtBanner.mainTexture= (txt as Texture);
                i++;
            });
//            ResourceLoad.Downloader.Instance.LoadAsyncTexture("Texture/banner/banner"+i%3,(txt)=>
//            {
//                m_txtBanner.mainTexture= (txt as Texture);
//                i++;
//            });
		});
	}
	
	
}
