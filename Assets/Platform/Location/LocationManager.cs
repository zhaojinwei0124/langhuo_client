using UnityEngine;
using System.Collections;
using Network;
using System.Collections.Generic;
using GameCore;

namespace Platform
{
    /// <summary>
    /// 利用腾讯地图的api http://lbs.qq.com/index.html
    /// 1、实现行政区域的定位
    /// 2、根据经纬度经行距离的计算
    /// </summary>
    public class LocationManager : Single<LocationManager>
    {

        public delegate void LocationHandler();
        
        public event LocationHandler locationHandler;

        //经度，纬度
        private float lng, lat;
        private string stat;

        public void Init()
        {
#if UNITY_EDITOR
            lat = 31.2918f;
            lng = 121.5318f;
            GameEngine.Instance.StartCoroutine(UpdateLocation());
#else
            Start();
#endif
        }


        public void Start()
        {
            GPS.Instance.Start((_code, _lng, _lat) =>
            {
                stat = ParseCode(_code);
                if(_code!=200)
                {
                    Toast.Instance.Show(stat);
                }
                lng = _lng;
                lat = _lat;
                Stop();
                GameEngine.Instance.StartCoroutine(UpdateLocation());
            });
        }

        public void Stop()
        {
            GPS.Instance.StopGPS();
        }

        ///获取行政位置
        public IEnumerator UpdateLocation()
        {
            WWW www = new WWW("http://apis.map.qq.com/ws/geocoder/v1/?location=" + lat + "," +
                              lng + "&key=CAIBZ-GYNLF-GXBJN-JGABN-MMAF3-KAFRW&get_poi=0");
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                NLocation loc = GameCore.Util.Instance.Get<NLocation>(www.text.Trim());
                GameBaseInfo.Instance.address=loc.result.address_component;
                if(locationHandler!=null)  locationHandler();
                Debug.Log("city: " + loc.result.address);
            } else
            {
                Toast.Instance.Show(10023);
            }
            www.Dispose();
        }

        ///距离不超过十公里
        public IEnumerator GetDistant(float lng2, float lat2, DelManager.FloatDelegate del)
        {
            WWW www = new WWW("http://apis.map.qq.com/ws/distance/v1/?mode=driving&from=" + lat + "," + 
                              lng + "&to=" + lat2 + "," + lng2 + "&key=CAIBZ-GYNLF-GXBJN-JGABN-MMAF3-KAFRW");
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("rcv :"+www.text.Trim());
                NDistance loc = GameCore.Util.Instance.Get<NDistance>(www.text.Trim());
                float dis = loc.result.elements[0].distance;
                Debug.Log("city dis: " + dis);
                del(dis);
            }
            else
            {
                Toast.Instance.Show(10025);
            }
            www.Dispose();
        }

        private string ParseCode(int code)
        {
            string info = Localization.Get(10022);
            if (code == 201)
            {
                info = Localization.Get(10019);
            } else if (code == 202)
            {
                info = Localization.Get(10020);
            } else if (code == 203)
            {
                info = Localization.Get(10021);
            }
            return info;
        }
    }
}