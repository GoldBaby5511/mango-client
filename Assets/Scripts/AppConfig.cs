using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConfig
{

    public static bool IsLocalMode = false;              //资源更新模式 - 本地
    public const string AppName = "兴隆斗地主";             //App名称
    public static string resUpdateUrl = "";              //游戏资源更新地址

    public const string version = "1.0.9";              //当前版本
    public static int channelCode
    {
        get 
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return 100;
            }
            else
            {
                return PlayerPrefs.GetInt("ChannelCode", 0);
            }
        }
        //0-官方版本
    }

    public static bool isSignVersion = true;       //是否加签版本

    //服务器地址
    public static bool isLocalNetwork = true;
    public static int serverIndex = 0;
    public static string[] localAddress = new string[] { "127.0.0.1", "logontest.hzzfzx.com", "192.168.0.100" }; 
    public static string[] serverUrl
    {
        get
        {
            if (isLocalNetwork)
            {
                return localAddress;
            }
            else
            {
                return new string[] { "l0.mile1990.com", "logontest.mile1990.com", "logontest.mile1990.com" };
            }
        }
    }
    public static int[] serverPort
    {
        get
        {
            if (isLocalNetwork)
            {
                return new int[] { 10100, 8500, 8500 };
            }
            else
            {
                return new int[] { 8500, 8500, 8500 };
            }
        }
    }

    //微信分享图片下载地址
    public static string weixinShareTextureUrl
    {
        get
        {
            return "http://wx.mile1990.com/ShareQRCode.ashx?gameid=" + HallModel.gameId + "&channelCode=" + channelCode;
        }
    }
    //微信分享地址
    public static string weixinShareUrl
    {
        get
        {
            return "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxead8d5a949646e2d&redirect_uri=http://wxhb.hzzfzx.com/Down.aspx?gameid=" + HallModel.gameId + "%26channelCode=" + AppConfig.channelCode + "&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";
        }
    }


    public static float screenWidth = 1280;
    public static float screenHeight = 720;

    #region 网站地址

    //加密字符
    public const string webKey1 = "a234sdfsdtg4564234fggsrebfdgur6";
    public const string webKey2 = "ooiyuidfttewrasdqweqweetryrtw3";

    //接口域名
    public static string url_mainURL 
    {
        get
        {
            if (isLocalNetwork)
            {
                return "http://localhost:10005/";
            }
            else
            {
                return "http://interface.mile1990.com/";
            }
        }
    }

    //版本信息
    public static string url_versionInfo = url_mainURL + "MobileVersion.ashx";
    //内购校验地址
    public static string url_IAPCheckOut = "";
    //微信登录地址
    public static string url_weixinLogin = url_mainURL + "WeiXin/Wx_MobileLoginHand.ashx";
    //微信H5充值
    public static string url_weixinPayH5 = "http://xlpay.mile1990.com/Pay/WeiXin/WeiXinPay.aspx";
    //支付宝H5充值
    public static string url_alipayH5 = "http://xlpay.mile1990.com/Pay/Aliwappay/Aliwappay.aspx";
    //商城信息
    public static string url_storeInfo = url_mainURL + "GetRechargeConfig.ashx";
    //手机绑定操作
    public static string url_BindPhoneOp = url_mainURL + "Users/UserBindingPhone1.ashx";
    //邮件
    public static string url_Email = url_mainURL + "Users/GetEmailInfo.ashx";
    //兑换
    public static string url_Exchange = url_mainURL + "GameProperty/GameProperty.ashx";
    //邀请有礼
    public static string url_InviteFriend = url_mainURL + "ActivityCenter/ReceiveTask.ashx";
    //重置密码
    public static string url_ResetPassword = url_mainURL + "Users/PassWord.ashx";
    //道具信息
    public static string url_PropInfo = url_mainURL + "GameProperty/GameProperty.ashx";
    //广告图
    public static string url_adTexture = url_mainURL + "MobileBanner/MobileBanner.ashx";
    #endregion


    //微信公众号
    public static string wxAccount = "兴隆娱乐中心";


    #region IOS商城配置

    //内购标识
    public static string[] IAP_Identifiers = { "com.hicity.game88168.coins_6", "com.hicity.game88168.coins_18", "com.hicity.game88168.coins_30", "com.hicity.game88168.coins_50", "com.hicity.game88168.coins_108", "com.hicity.game88168.coins_298" };
    //自定义标识，用于官网识别
    public const string IAP_UserFlag = "a234sdfsdtg4564234fggsrebfdgur6";

    #endregion


    //游戏列表
    public static Dictionary<GameFlag, GameInfo> gameDic = new Dictionary<GameFlag, GameInfo>();
    //商品信息
    public static Dictionary<int, GoodInfo> goodDic_android = new Dictionary<int, GoodInfo>();
    public static Dictionary<int, GoodInfo> goodDic_ios = new Dictionary<int, GoodInfo>();
    //兑换商品信息
    public static Dictionary<int, ExchangeInfo> exchangeDic = new Dictionary<int, ExchangeInfo>();
    //初始化App
    public static void InitApp()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.fullScreen = false;
        if (Screen.width > Screen.height)
        {
            screenWidth = screenHeight * (Screen.width / (float)Screen.height);
        }
        else
        {
            screenWidth = screenHeight * (Screen.height / (float)Screen.width);
        }
        
        Debug.Log("屏幕宽度 ： " + screenWidth+",flag," + GameFlag.Landlords3);

        gameDic.Clear();
        gameDic.Add(GameFlag.Landlords3, new GameInfo(GameFlag.Landlords3, "3人斗地主", "gameLandlords"));
    }



}
