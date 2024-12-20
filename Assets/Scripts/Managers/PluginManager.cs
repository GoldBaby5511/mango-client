using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// 插件管理
/// </summary>
public class PluginManager : MonoBehaviour
{
    private static PluginManager _instance;
    public static PluginManager Instance
    {
        get
        {
            if (_instance == null)
            {
                string name = "PluginManager";
                GameObject obj = GameObject.Find(name);
                if (obj == null)
                {
                    obj = new GameObject(name);
                    _instance = obj.AddComponent<PluginManager>();
                }
                else
                {
                    _instance = obj.GetComponent<PluginManager>();
                    if (_instance == null)
                    {
                        _instance = obj.AddComponent<PluginManager>();
                    }
                }
            }
            return _instance;
        }
    }

    [DllImport("__Internal")]
    private static extern int doWeiXinLogin();
    [DllImport("__Internal")]
    private static extern int doWeiXinShare(int _type, string url, string title, string message, string imagePath, string extInfo);
    [DllImport("__Internal")]
    private static extern void _copyTextToClipboard(string text);
    [DllImport("__Internal")]
    private static extern float getiOSBatteryLevel();

    private int shareType = 1;                          //0-好友 1-空间
    private static UnityAction shareCallBack;     //分享成功后回调

    private int goodId = 0;     //所选商品ID

    void Awake()
    {
        ConfigureStoreKitEvents();
    }

    void OnDestroy()
    {
        CancelStoreKitEvents();
    }



    #region WeChat

    /// <summary>
    /// 微信充值
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="goodId">商品ID</param>
    public void WxPay(int userId, int _goodId)
    {
        goodId = _goodId;
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        //    {
        //        using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
        //        {
        //            jo.Call("WxPay", AppConfig.goodDic_android[goodId].Type, AppConfig.goodDic_android[goodId].Count.ToString(), userId.ToString() + "," + goodId + "," + AppConfig.goodDic_android[goodId].RechargeName);
        //        }
        //    }
        //}
        //Web充值
        string timrStr = Util.GetDateTimeSeconds();
        string param = userId.ToString() + AppConfig.goodDic_android[goodId].RechargePrice + AppConfig.webKey1 + timrStr + _goodId;
        string md5Param = Util.GetMd5(param);
        string md5 = Util.GetMd5(md5Param + AppConfig.webKey2);
        //string url = AppConfig.url_weixinPayH5 + "?userid=" + userId.ToString() + "&awardID=" + goodId + "&money=" + AppConfig.goodDic_android[goodId].RechargePrice + "&ip=" + Network.player.ipAddress + "&buyType=" + AppConfig.goodDic_android[goodId].Type + "&sign=" + md5 + "&timestr=" + timrStr;
        
        //Application.OpenURL(url);
        Invoke("QueryCoinInfo", 60f);
        Invoke("QueryCoinInfo", 120f);
    }


    //微信登录
    public void WxLogin()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("WxLogin");
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            doWeiXinLogin();
        }
        else
        {
            Debug.LogError("微信登录操作请在真机环境下进行！");
        }
    }

    /// <summary>
    /// 微信分享-URL
    /// </summary>
    /// <param name="type">0-好友 1-空间</param>
    /// <param name="url">地址</param>
    /// <param name="title">标题</param>
    /// <param name="message">内容</param>
    public void WxShareUrl(int type, string url, string title, string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("WxShareUrl", type, url, title, message);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            doWeiXinShare(2, url, title, message, "", type.ToString());
        }
        else
        {
            Debug.LogError("微信分享操作请在真机环境下进行！");
        }
    }

    /// <summary>
    /// 微信分享图片
    /// </summary>
    /// <param name="type">0-好友 1-空间</param>
    /// <param name="filePath">图片路径</param>
    public void WxShareImage(int type, string filePath, UnityAction callback)
    {
        shareType = type;
        shareCallBack = callback;
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("WxShareImage", type, filePath);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            doWeiXinShare(1, "", "", "", filePath, type.ToString());
        }
        else
        {
            Debug.LogError("微信分享操作请在真机环境下进行！");
        }
    }






    //插件返回 - 微信支付
    public void OnWeiXinPayFinish(string result)
    {
        string[] strs = result.Split('|');
        int resultCode = int.Parse(strs[0]);
        // 0 - 成功， 1 - 失败  2 - 支付取消
        OnPayFinish(resultCode);
    }

    //插件返回 - 微信登陆
    public void OnWeiXinLoginFinish(string result)
    {
        //登录
        try
        {
            //resultCode   0 - 成功   非0 - 失败
            int resultCode = int.Parse(result.Split('|')[0]);
            string resultStr = result.Split('|')[1];
            if (resultCode == 0)
            {
                WebService.Instance.WxLogin(resultStr);
                Debug.Log("调用微信登录返回 ： " + resultStr);
            }
            else
            {
                Debug.LogError("登陆异常 ：" + result);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("微信返回结果异常 ： " + e.ToString());
        }
    }

    //插件返回 - 微信分享
    public void OnWeiXinShareFinish(string result)
    {
        try
        {
            //resultCode   0 - 分享成功   非0 - 分享失败
            int resultCode = int.Parse(result.Split('|')[0]);
            string resultStr = result.Split('|')[1];
            if (resultCode == 0 && shareType == 1 && shareCallBack != null)
            {
                shareCallBack.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("微信分享异常：" + e.ToString());
        }
    }



    #endregion


    #region AliPay

    /// <summary>
    /// 支付宝充值
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="goodId">商品ID</param>
    public void AliPay(int userId, int _goodId)
    {
        goodId = _goodId;
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        //    {
        //        using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
        //        {
        //            jo.Call("Alipay", AppConfig.goodDic_android[goodId].Type, AppConfig.goodDic_android[goodId].Count.ToString(), userId.ToString() + "," + goodId + "," + AppConfig.goodDic_android[goodId].RechargeName);
        //        }
        //    }
        //}

        //Web充值
        //string timrStr = Util.GetDateTimeSeconds();
        //string param = userId.ToString() + AppConfig.goodDic_android[goodId].RechargePrice + AppConfig.webKey1 + timrStr + Network.player.ipAddress + _goodId;
        //string md5Param = Util.GetMd5(param);
        //string md5 = Util.GetMd5(md5Param + AppConfig.webKey2);
        //string url = AppConfig.url_alipayH5 + "?userid=" + userId.ToString() + "&awardID=" + goodId + "&money=" + AppConfig.goodDic_android[goodId].RechargePrice + "&ip=" + Network.player.ipAddress + "&buyType=" + AppConfig.goodDic_android[goodId].Type + "&sign=" + md5 + "&timestr=" + timrStr;
        //Application.OpenURL(url);
        //Invoke("QueryCoinInfo", 60f);
        //Invoke(" ", 120f);
    }

    /// <summary>
    /// 支付结果回调
    /// </summary>
    public void OnAliPayFinish(string result)
    {
        string[] strs = result.Split('|');
        int resultCode = int.Parse(strs[0]);
        // 0 - 成功， 1 - 失败  2 - 支付取消
        OnPayFinish(resultCode);
    }

    #endregion


    #region IOS内购

    /// <summary>
    /// IOS内购购买
    /// </summary>
    /// <param name="goodID">商品ID</param>
    public void IOSPay(int goodID)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (EasyStoreKit.CanMakePayments())
            {
                EasyStoreKit.LoadProducts();
            }
        }
        else
        {
            Debug.LogError("内购充值测试请在真机环境下进行！");
        }
    }

    //配置内购事件
    void ConfigureStoreKitEvents()
    {
        EasyStoreKit.productsLoadedEvent += OnProductsLoaded;
        EasyStoreKit.transactionPurchasedEvent += OnTransactionPurchased;
        EasyStoreKit.transactionFailedEvent += OnTransactionFailed;
        EasyStoreKit.transactionCancelledEvent += OnTransactionCancelled;
    }

    //取消内购事件
    void CancelStoreKitEvents()
    {
        EasyStoreKit.productsLoadedEvent -= OnProductsLoaded;
        EasyStoreKit.transactionPurchasedEvent -= OnTransactionPurchased;
        EasyStoreKit.transactionFailedEvent -= OnTransactionFailed;
        EasyStoreKit.transactionCancelledEvent -= OnTransactionCancelled;
    }

    //产品加载成功
    void OnProductsLoaded(StoreKitProduct[] products)
    {
        EasyStoreKit.BuyProductWithIdentifier(AppConfig.IAP_Identifiers[goodId], 1);
    }

    //支付成功
    void OnTransactionPurchased(string flag)
    {
        //将支付成功消息发送至服务器
        //string userID = HallModel.userId.ToString();     //用户ID
        //string productIdentifier = "";              //产品标识
        //string userIdentifier = "";                 //唯一标识，用于苹果官网验证
        //string coinCount = "";                      //金币数
        //string productPrice = "";                   //产品价格
        //string md5 = "";

        //string[] str = flag.Split('$');
        //if (str.Length > 1)
        //{
        //    productIdentifier = str[0];
        //    userIdentifier = str[1];

        //    if (string.Equals(productIdentifier, AppConfig.IAP_Identifiers[0]))
        //    {
        //        coinCount = AppConfig.diamonds_ios[0].ToString();
        //        productPrice = AppConfig.prices_ios[0].ToString();
        //    }
        //    if (string.Equals(productIdentifier, AppConfig.IAP_Identifiers[1]))
        //    {
        //        coinCount = AppConfig.diamonds_ios[1].ToString();
        //        productPrice = AppConfig.prices_ios[1].ToString();
        //    }
        //    if (string.Equals(productIdentifier, AppConfig.IAP_Identifiers[2]))
        //    {
        //        coinCount = AppConfig.diamonds_ios[2].ToString();
        //        productPrice = AppConfig.prices_ios[2].ToString();
        //    }
        //    if (string.Equals(productIdentifier, AppConfig.IAP_Identifiers[3]))
        //    {
        //        coinCount = AppConfig.diamonds_ios[3].ToString();
        //        productPrice = AppConfig.prices_ios[3].ToString();
        //    }
        //    if (string.Equals(productIdentifier, AppConfig.IAP_Identifiers[4]))
        //    {
        //        coinCount = AppConfig.diamonds_ios[4].ToString();
        //        productPrice = AppConfig.prices_ios[4].ToString();
        //    }
        //    if (string.Equals(productIdentifier, AppConfig.IAP_Identifiers[5]))
        //    {
        //        coinCount = AppConfig.diamonds_ios[5].ToString();
        //        productPrice = AppConfig.prices_ios[5].ToString();
        //    }

        //    string ss = userID + productIdentifier + userIdentifier + coinCount + productPrice + AppConfig.IAP_UserFlag;
        //    md5 = Util.GetMd5(ss);
        //}


        //WWWForm form = new WWWForm();
        //form.AddField("userID", userID);                                        //用户ID
        //form.AddField("productIdentifier", productIdentifier);                  //商品标识
        //form.AddField("userIdentifier", userIdentifier);                        //唯一标识，用于官网验证
        //form.AddField("coinCount", coinCount);                                  //金币数量
        //form.AddField("productPrice", productPrice);                            //商品价格
        //form.AddField("md5", md5);                                              //MD5

        //StartCoroutine(SendHttpRequest(AppConfig.IAP_Url, form));
    }

    IEnumerator SendHttpRequest(string url, WWWForm form)
    {
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error == null)
        {
            OnPayFinish(0);
        }
        else
        {
            Debug.LogError("发送内购验证数据失败：" + www.error);
        }
    }

    //支付失败
    void OnTransactionFailed(string productIdentifier, string errorMessage)
    {
        OnPayFinish(1);
    }

    //支付取消
    public void OnTransactionCancelled(string productIdentifier)
    {
        OnPayFinish(1);
    }

    #endregion


    //支付完成后通知
    private void OnPayFinish(int resultCode)
    {
        string mainTip = "";
        string subTip = "如遇充值问题，请联系客服";
        if (resultCode == 0)
        {
            mainTip = "充值成功，商品将自动存入账户，请注意查收!";
            Invoke("QueryCoinInfo", 15f);
            Invoke("QueryCoinInfo", 30f);
            Invoke("QueryCoinInfo", 60f);
        }
        else
        {
            mainTip = "充值失败！";
        }
        if (GameEvent.V_OpenDlgTip != null)
        {
            GameEvent.V_OpenDlgTip.Invoke(mainTip, subTip, QueryCoinInfo, null);
        }
    }

    //查询金币信息
    private void QueryCoinInfo()
    {
        HallService.Instance.QueryBankInfo();
    }

    //打开apk
    public void OpenAPK(string path)
    {
        try
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("OpenAPK", path);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("打开apk异常 : " + e.Message + "路径：" + path);
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke("升级失败,请前往“兴隆娱乐中心”公众号重新下载安装!", "", null, null);
            }
        }
    }

    //日志
    public void Log(string msg)
    {
        Debug.Log(msg);
    }

    //复制内容到剪切板
    public void CopyToClipboard(string text)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("CopyTextToClipboard", text);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _copyTextToClipboard(text);
        }
    }

    public float GetIOSBatteryLevel()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return getiOSBatteryLevel();
        }
        return 1;
    }

}


