using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneInit : MonoBehaviour 
{
    private UISlider slider;
    private UILabel lblTip;

    private string downUrl = "";
    private AsyncOperation async;

    void Awake()
    {
        HallModel.isStartByURL = false;
        slider = transform.Find("bg/slider").GetComponent<UISlider>();
        lblTip = transform.Find("bg/lblTip").GetComponent<UILabel>();
        GPSManager.Instance.UpdateGPS();
        WebService.Instance.LoadAdTexture();
        //初始化
        slider.value = 0;
        lblTip.text = "";
        AppConfig.InitApp();
        GameModel.InitRes();
        //请求版本信息
        GetVersionInfo();
        //请求商城信息
        WebService.Instance.GetStoreInfo();
        //请求道具配置信息
        WebService.Instance.GetExchangeInfo();
        //自适应翻转
        Util.AdaptScreenOrientation();
        //默认系统消息
        HallModel.defaultMessageList.Add("欢迎来到[FF2929FF]兴隆斗地主[-]，本游戏为休闲平台，禁止赌博！");
    }

    void OnStartByURL(string msg)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameModel.currentRoomId = uint.Parse(msg);
        }
        else
        {
            try
            {
                GameModel.currentRoomId = uint.Parse(msg.Split('=')[1]);
            }
            catch
            {
                return;
            }
        }
        HallModel.isStartByURL = true;
        HallModel.opOnLoginGame = OpOnLginGame.JoinRoom;
    }

    void Update()
    {
        if (async != null)
        {
            slider.value = async.progress;
        }
    }

    //请求版本信息
    void GetVersionInfo()
    {
        Web_C_Version pack = new Web_C_Version();
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                pack.code = 1;
                break;
            case RuntimePlatform.IPhonePlayer:
                pack.code = 2;
                break;
            default:
                pack.code = 1;
                break;
        }
        WebService.Instance.Send<Web_S_Version>(AppConfig.url_versionInfo, pack, OnGetVersionInfo);
    }

    void OnGetVersionInfo(Web_S_Version pro)
    {
        if (pro != null)
        {
            downUrl = pro.downurl;
            int currentVersion = int.Parse(AppConfig.version.Replace(".", ""));
            int newVersion = int.Parse(pro.version.Replace(".", ""));
            HallModel.isVerify = false;
            HallModel.ruleLoginType = pro.logontype;
            Debug.Log("当前版本 ： " + currentVersion + "   新版本：" + newVersion);
            if (newVersion > currentVersion)
            { 
                //需要更新
                if (Util.IsWifi)
                {
                    UpdateVersion();
                }
                else
                {
                    //请求版本成功
                    if (GameEvent.V_OpenDlgTip != null)
                    {
                        GameEvent.V_OpenDlgTip.Invoke("发现新版本，是否立即更新？", "您当前使用的是移动网络，继续更新请点击“确定”按钮", UpdateVersion, EnterHallScene);
                    }
                }
            }
            else if (newVersion < currentVersion)
            {
                //审核状态
                if (Application.platform == RuntimePlatform.IPhonePlayer && !AppConfig.isSignVersion)
                {
                    HallModel.isVerify = true;
                }
                else
                {
                    HallModel.isVerify = false;
                }

                EnterHallScene();
            }
            else
            {
                //不需要更新
                EnterHallScene();
            }
        }
        else
        {
            //请求版本失败
            lblTip.text = "请求版本信息失败！";
            if (Application.platform == RuntimePlatform.IPhonePlayer && !AppConfig.isSignVersion)
            {
                HallModel.isVerify = true;
            }
            else
            {
                HallModel.isVerify = false;
            }
            HallModel.ruleLoginType = 3;
            EnterHallScene();
        }
    }

    //更新版本
    void UpdateVersion()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(DownloadAPK());
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.OpenURL(downUrl);
        }
        else
        {
            EnterHallScene();
        }
    }

    //下载APK
    IEnumerator DownloadAPK()
    {
        yield return new WaitForEndOfFrame();
        WWW www = new WWW(downUrl);
        while (!www.isDone)
        {
            float progress = ((int)(www.progress * 10000)) / 100f;
            lblTip.text = "正在下载更新，请耐心等待  " + progress + " %";
            slider.value = progress;
            yield return new WaitForSeconds(0.1f);
        }
        
        if (www.error != null)
        {
            lblTip.text = "版本更新失败！";
            yield return new WaitForSeconds(3f);
            EnterHallScene();
        }
        else
        {
            lblTip.text = "正在下载更新，请耐心等待  100 %";
            string path = Application.persistentDataPath + "/xianjvmj.apk";
            System.IO.File.WriteAllBytes(path, www.bytes);
            yield return new WaitForSeconds(1f);
            PluginManager.Instance.OpenAPK(path);
        }
    }

    //进入大厅场景
    void EnterHallScene()
    {
        StartCoroutine(LoadHallScene());
    }

    //加载大厅场景
    IEnumerator LoadHallScene()
    {
        yield return new WaitForSeconds(0.5f);
        async = SceneManager.LoadSceneAsync("hall");
        yield return async;
    }
	
}
