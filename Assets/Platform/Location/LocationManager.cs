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
            WWW www=new WWW("http://lbs.juhe.cn/api/getaddressbylngb?lngx="+lng+"&lngy="+lat);
            yield return www;
            NLocation loc = GameCore.Util.Instance.Get<NLocation>(www.text.Trim());
        }


        private string ParseCode(int code)
        {
            string info = Localization.Get(10022);
            if (code == 201)
            {
                info = Localization.Get(10019);
            }
            else if (code == 202)
            {
                info = Localization.Get(10020);
            }
            else if (code == 203)
            {
                info = Localization.Get(10021);
            }
            return info;
        }
    }
}