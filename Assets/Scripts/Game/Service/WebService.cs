using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WebService : MonoBehaviour 
{
    private static WebService _instance;
    public static WebService Instance
    {
        get
        {
            if (_instance == null)
            {
                string name = "WebService";
                GameObject obj = GameObject.Find(name);
                if (obj == null)
                {
                    obj = new GameObject(name);
                    _instance = obj.AddComponent<WebService>();
                }
                else
                {
                    _instance = obj.GetComponent<WebService>();
                    if (_instance == null)
                    {
                        _instance = obj.AddComponent<WebService>();
                    }
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Send<T>(string url, object msg, Action<T> callback) where T : class
    {
        StartCoroutine(SendCor(url, msg, callback));
    }

    IEnumerator SendCor<T>(string url, object msg, Action<T> callback) where T : class
    {
        WWW www = null;
        string json = null;
        try
        {
            if (msg != null)
            {
                json = JsonUtility.ToJson(msg);
            }
        }
        catch(Exception e)
        {
            if (callback != null)
            {
                callback.Invoke(null);
            }
            Debug.LogError("Json序列化异常 ： " + typeof(T) + "  -  " + e.ToString());
        }
        
        if (msg != null)
        {
            www = new WWW(url, System.Text.Encoding.UTF8.GetBytes(json));
        }
        else
        {
            www = new WWW(url);
        }
        yield return www;
        if (www.error == null)
        {
            T t = null;
            try
            {
                t = JsonUtility.FromJson<T>(www.text);
            }
            catch(Exception e)
            {
                Debug.LogError("Json反序列化异常 - " + typeof(T) + " : " + e.Message);
            }
            if (callback != null)
            {
                callback.Invoke(t);
            }
        }
        else
        {
            if (callback != null)
            {
                callback.Invoke(null);
            }
            Debug.LogError("WebService网络异常 - " + www.error + " : " + typeof(T));
        }
    }


    //微信登录
    public void WxLogin(string code)
    {
        Web_C_WeiXinLogin pro = new Web_C_WeiXinLogin();
        pro.code = code;
        WebService.Instance.Send<Web_S_WeiXinLogin>(AppConfig.url_weixinLogin, pro, OnWxLogin);
    }

    void OnWxLogin(Web_S_WeiXinLogin pro)
    {
        Debug.Log("微信登录Web返回 ： " + pro.unionid);
        HallService.Instance.InitAccountInfo(LoginType.OtherSDK, "", "", pro.unionid, pro.nickName, pro.sex, pro.headimgurl, "", "");
        HallService.Instance.Connect(ConnectType.Normal);

    }



    //请求商城信息
    public void GetStoreInfo()
    {
        Send<Web_S_StoreInfo>(AppConfig.url_storeInfo, null, OnGetStoreInfo);
    }

    void OnGetStoreInfo(Web_S_StoreInfo pro)
    {
        if (pro == null)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "请求商城配置信息失败...");
        }
        else if (pro.return_code != "10000")
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "请求商城配置信息失败...");
        }
        else
        {
            AppConfig.goodDic_android.Clear();
            for(int i = 0; i < pro.return_message.Count; i++)
            {
                if (!AppConfig.goodDic_android.ContainsKey(pro.return_message[i].RechargeID))
                {
                    AppConfig.goodDic_android.Add(pro.return_message[i].RechargeID, pro.return_message[i]);
                }
                else
                {
                    Debug.LogError("商品信息异常 ： ID重复");
                }
            }
            
        }
    }

    //请求兑换信息
    public void GetExchangeInfo()
    { 
        Send<Web_S_ExchangeList>(AppConfig.url_Exchange, new Web_C_ExchangeList(), OnGetExchangeInfo);
    }

    void OnGetExchangeInfo(Web_S_ExchangeList pro)
    {
        if (pro == null)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "请求兑换配置信息失败...");
        }
        else if (pro.return_code != 10000)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "请求兑换配置信息失败...");
        }
        else
        {
            AppConfig.exchangeDic.Clear();
            for (int i = 0; i < pro.return_message.Count; i++)
            {
                if (!AppConfig.exchangeDic.ContainsKey(pro.return_message[i].ID))
                {
                    AppConfig.exchangeDic.Add(pro.return_message[i].ID, pro.return_message[i]);
                }
                else
                {
                    Debug.LogError("商品信息异常 ： ID重复");
                }
            }
        }
    }



    //请求道具信息
    public void GetPropInfo()
    {
        HallModel.isBuyMonthCard = false;
        HallModel.isGetMonthCardGift = false;
        HallModel.lastMonthCardDay = 0;

        Web_C_PropInfo pro = new Web_C_PropInfo();
        pro.userId = HallModel.userId;
        Send<Web_S_PropInfo>(AppConfig.url_PropInfo, pro, OnGetPropInfo);
    }

    void OnGetPropInfo(Web_S_PropInfo pro)
    {
        if (pro == null)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "请求道具信息失败...");
        }
        else if (pro.return_code != 10000)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, pro.message);
        }
        else
        {
            for (int i = 0; i < pro.return_message.Count; i++)
            {
                if (pro.return_message[i].GoodsID == 620)
                {
                    HallModel.isBuyMonthCard = pro.return_message[i].GoodsCount > 0;
                    break;
                }
            }
        }
        //请求月卡信息
        GetMonthCardInfo();
//         if (!HallModel.isBuyMonthCard)
//         {
//             Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, 0);
//         }
//         else
//         {
//             GetMonthCardInfo();
//         }
    }

    //请求月卡信息
    public void GetMonthCardInfo()
    {
        Web_C_MonthCardInfo pro = new Web_C_MonthCardInfo();
        pro.userId = HallModel.userId;
        Send<Web_S_MonthCardInfo>(AppConfig.url_PropInfo, pro, OnGetMonthCardInfo);
    }

    void OnGetMonthCardInfo(Web_S_MonthCardInfo pro)
    {
        if (pro == null)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "请求月卡信息失败...");
        }
        else
        {
            HallModel.isGetMonthCardGift = pro.return_message.MonthCardUseCount > 0;
            HallModel.lastMonthCardDay = pro.return_message.LastDayCount;
            HallModel.isFirstCharge = pro.return_message.FirstChargeCount > 0;
            HallModel.isNeedShareGameWhenExchange = pro.return_message.NeedSendCircle > 0;
//             if (!HallModel.isBuyMonthCard)
//             {
//                 Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, 0);
//             }
//             else
//             {
//                 if (!HallModel.isGetMonthCardGift)
//                 {
//                     Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, pro.return_message.MonthCardType);
//                 }
//                 else
//                 {
//                     Util.Instance.DoActionDelay(HallEvent.V_OpenDlgShareTask, 0.2f);
//                 }
//             }
            
        }
    }


    //加载玩家头像
    public void LoadUserPhoto(int userId, string url)
    {
        StartCoroutine(LoadPhoto(userId, url));
    }

    IEnumerator LoadPhoto(int userId, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            if (HallModel.userPhotos.ContainsKey(userId))
            {
                HallModel.userPhotos[userId] = www.texture;
            }
            else
            {
                HallModel.userPhotos.Add(userId, www.texture);
            }
            yield return new WaitForEndOfFrame();
            //刷新个人信息
            if (GameEvent.V_RefreshUserInfo != null)
            {
                GameEvent.V_RefreshUserInfo.Invoke(false);
            }
        }
        else
        {
            //Debug.LogError("加载头像失败...");
        }
    }


    //加载广告图
    public void LoadAdTexture()
    {
        Send<Web_S_AdTextureInfo>(AppConfig.url_adTexture, null, OnGetAdTextureInfo);
    }

    void OnGetAdTextureInfo(Web_S_AdTextureInfo info)
    {
        if (info.return_code == 10000)
        {
            HallModel.adTextureList.Clear();
            StartCoroutine(LoadAdTexture(info.return_message));
        }
        else
        {
            Debug.LogError("加载广告图失败 ： " + info.return_code + " - " + info.return_message);
        }
    }

    IEnumerator LoadAdTexture(List<AdTextureInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            WWW www = new WWW(list[i].URL);
            yield return www;
            if (www.error == null)
            {
                HallModel.adTextureList.Add(www.texture);
            }
            else
            {
                Debug.LogError("下载广告图异常 ： " + www.error);
            }
        }
    }

}
