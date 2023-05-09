using UnityEngine;
using System.Collections;

public class GPSManager : MonoBehaviour 
{
    private static GPSManager _instance;
    public static GPSManager Instance
    {

        get
        {

            if (_instance == null)
            {
                string name = "GameManager";
                GameObject manager = GameObject.Find("GameManager");
                if (manager == null)
                {
                    manager = new GameObject(name);
                    _instance = manager.AddComponent<GPSManager>();
                }
                else
                {
                    _instance = manager.GetComponent<GPSManager>();
                    if (_instance == null)
                    {
                        _instance = manager.AddComponent<GPSManager>();
                    }
                }
            }

            return _instance;
        }
    }

    public static float latitude;
    public static float longitude;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //更新GPS位置
    public void UpdateGPS()
    {
        StartCoroutine(StartGPS());
    }

 
    IEnumerator StartGPS()
    {
        // 判断GPS是否可用
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS服务不可用，请打开GPS！");
            yield return false;
        }
 
        //启动位置服务的更新
        Input.location.Start(10.0f, 10.0f);
        
        //设定超时时间为20s
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
 
        if (maxWait < 1)
        {
            Debug.Log("GPS超时！");
            yield return new WaitForSeconds(60);
            UpdateGPS();
        }
 
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("GPS定位失败！");
            yield return new WaitForSeconds(60);
            UpdateGPS();
        }
        else
        {
            //保存GPS位置信息
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            //停止GPS服务，有助于省电
            Input.location.Stop();
        }
    }
}
