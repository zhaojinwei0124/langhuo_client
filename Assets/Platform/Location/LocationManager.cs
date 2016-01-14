using UnityEngine;
using System.Collections;
using Network;
using System.Collections.Generic;

namespace Platform
{
    public class LocationManager : Single<LocationManager>
    {
        //经度，纬度
        private float lng, lat;
        private string stat;

        public void Test()
        {
            lat = 31.2918f;
            lng = 121.5318f;
            GameEngine.Instance.StartCoroutine(UpdateLocation());
        }

        public void Start()
        {
            GetGPS.Instance.Start((_code, _lng, _lat) =>
            {
                stat = ParseCode(_code);
                lng = _lng;
                lat = _lat;
                Stop();
            });
        }

        public void Stop()
        {
            GetGPS.Instance.StopGPS();
        }

        public IEnumerator UpdateLocation()
        {
            WWW www = new WWW("http://apis.map.qq.com/ws/geocoder/v1/?location=" + lat + "," + lng + "&key=CAIBZ-GYNLF-GXBJN-JGABN-MMAF3-KAFRW&get_poi=0");
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                NLocation loc = GameCore.Util.Instance.Get<NLocation>(www.text.Trim());
                Debug.Log("location: " + www.text.Trim());
                Debug.Log("city: " + loc.result.address + " nation: " + loc.result.address_component.nation + " pro: " + loc.result.address_component.province);
            } else
            {
                Toast.Instance.Show(10023);
            }
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