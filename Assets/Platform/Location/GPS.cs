using UnityEngine;
using System.Collections;
namespace Platform
{
    public delegate void DispacherLocation(int code, float lng, float lag);


    public class GPS : Single<GPS>
    {

        public string gps_info = "";
        public int flash_num = 1;

        private DispacherLocation m_del;

        public void Start(DispacherLocation del)
        {
            m_del = del;
            GameEngine.Instance.StartCoroutine(StartGPS());
        }

        void OnGUI()
        {
            GUI.skin.label.fontSize = 28;
            GUI.Label(new Rect(20, 20, 600, 48), this.gps_info);
            GUI.Label(new Rect(20, 50, 600, 48), this.flash_num.ToString());

            GUI.skin.button.fontSize = 50;
            if (GUI.Button(new Rect(Screen.width / 2 - 110, 200, 220, 85), "GPS定位"))
            {
                // 这里需要启动一个协同程序
                // StartCoroutine(StartGPS());
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 110, 500, 220, 85), "刷新GPS"))
            {
                this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
                this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
                this.flash_num += 1;
            }
        }

        public void StopGPS()
        {
            Input.location.Stop();
        }

        IEnumerator StartGPS()
        {
            // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置
            // LocationService.isEnabledByUser 用户设置里的定位服务是否启用
            if (!Input.location.isEnabledByUser)
            {
                this.gps_info = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
                m_del(201, 0f, 0f);
                yield break;
            }
            // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用
            Input.location.Start(50.0f, 50.0f);

            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                // 暂停协同程序的执行(1秒)
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            if (maxWait < 1)
            {
                this.gps_info = "Init GPS service time out";
                m_del(202, 0f, 0f);
                yield break;
            }
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                this.gps_info = "Unable to determine device location";
                m_del(203, 0f, 0f);
                yield break;
            }
            else
            {
                this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
                this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
                m_del(200, Input.location.lastData.longitude, Input.location.lastData.latitude);
            }
        }
    }
}