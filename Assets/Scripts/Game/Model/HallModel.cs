using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallModel
{

    public static int ruleLoginType = 3;        //位预算信息 ： 1-微信登录 2-手机登录
    //是否URL启动
    public static bool isStartByURL = false;
    //登录模式
    public static LoginType loginType = LoginType.Visitor;
    //登录游戏成功后操作
    public static OpOnLginGame opOnLoginGame = OpOnLginGame.CreateRoom;

    public static bool isVerify = false;
    public static bool isBackground = false;
    public static bool isReturnFromGame = false;
    public static bool isAgreeUserProtocol
    {
        get
        {
            return PlayerPrefs.GetInt("IsAgreeUserProtocol", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("IsAgreeUserProtocol", value ? 1 : 0);
        }
    }

    public static bool isSwitchAccount = false;

    //是否需要分享才能兑换
    public static bool isNeedShareGameWhenExchange = false;

    //用户信息
    public static string userAccount
    {
        get
        {
            return PlayerPrefs.GetString("UserAccount");
        }
        set
        {
            PlayerPrefs.SetString("UserAccount", value);
        }
    }
    public static string userPassword
    {
        get
        {
            return PlayerPrefs.GetString("UserPassword");
        }
        set
        {
            PlayerPrefs.SetString("UserPassword", value);
        }
    }

    public static bool isRememberUserAccount
    {
        get
        {
            return PlayerPrefs.GetInt("IsRememberUserAccount", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("IsRememberUserAccount", value ? 1 : 0);
        }
    }
    public static bool isRememberUserPassword
    {
        get
        {
            return PlayerPrefs.GetInt("IsRememberUserPassword", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("IsRememberUserPassword", value ? 1 : 0);
        }
    }

    public static string userOpenId
    {
        get
        {
            return PlayerPrefs.GetString("OpenId", "");
        }
        set
        {
            PlayerPrefs.SetString("OpenId", value);
        }
    }
    public static string dynamicPassword;           //动态密码
    public static bool isBankEnable = false;        //银行是否可用
    public static string userBankPwd
    {
        get
        {
            return PlayerPrefs.GetString("userBankPwd", null);
        }
        set
        {
            PlayerPrefs.SetString("userBankPwd", value);
        }
    }

    //默认头像
    public static Texture defaultPhoto;             
    //用户头像
    public static Dictionary<ulong, Texture> userPhotos = new Dictionary<ulong, Texture>();

    
    public static ulong userId;
    public static ulong gameId;
    public static string userIP = "xx.xxx.x.xxx （中国）";               //用户IP地址
    public static string userName = "未登录";       //用户昵称
    public static string headImgUrl = "";           //用户头像url
    public static string userPhone = "";            //用户手机号
    public static string userRateInfo;              //用户胜率信息
     
    public static int faceId;                       //系统头像ID
    public static int userSex;                      //用户性别
    public static Int64 userCoinInGame = 0;         //用户金币
    public static Int64 userCoinInBank = 0;         //银行金币
    public static Int64 userDiamondCount = 0;       //用户钻石
    public static Int64 userCardCount = 0;          //用户房卡
    public static Int64 userRedPackCount = 0;       //红包券数量

    public static bool isRealAuth = true;           //是否已实名认证
    public static bool isBindAgency = false;        //是否已绑定代理
    public static string agencyAccount = "";        //代理账户
    public static int spreaderId;                   //推荐人ID
    public static string spreaderName;              //推荐人昵称

    //月卡
    public static bool isBuyMonthCard = false;          //用户是否开通月卡
    public static bool isGetMonthCardGift = false;      //是否已领取月卡福利
    public static int lastMonthCardDay = 0;             //剩余月卡领取天数
    public static bool isFirstCharge = false;           //今日是否首充

    //游戏类型列表
    public static List<GameKindInfo> gameKindList = new List<GameKindInfo>();
    //房卡游戏信息列表
    public static Dictionary<int, CardGameServerInfo> cardServerList = new Dictionary<int, CardGameServerInfo>();   
    //游戏服务器列表
    public static Dictionary<UInt64, GameServerInfo> serverList = new Dictionary<UInt64, GameServerInfo>();
    //游戏记录
    public static Dictionary<int, CMD_Hall_S_GameRecord> gameRecordList = new Dictionary<int, CMD_Hall_S_GameRecord>();
    //排行信息
    public static Dictionary<byte, CMD_Hall_S_RankInfo> rankDic = new Dictionary<byte, CMD_Hall_S_RankInfo>();

    //public static Dictionary<UInt32, Bs.Types.RoomInfo> roomList = new Dictionary<UInt32, Bs.Types.RoomInfo>();



    //当前所选游戏
    public static GameFlag currentGameFlag = GameFlag.Landlords3;
    //当前所选服务器ID
    public static UInt64 currentServerId = 0;
    //当前所选游戏名称
    public static string currentGameName
    {
        get
        {
            return AppConfig.gameDic[currentGameFlag].gameName;
        }
    }
    //当前所选游戏类型ID
    public static int currentGameKindId
    {
        get
        {
            return AppConfig.gameDic[currentGameFlag].kindId;
        }
    }
    //当前房间名称
    public static string currentRoomName
    {
        get
        {
            return serverList[currentServerId].szServerName;
        }
    }



    //复活条件 - 金币场
    public static int coinBaseEnsureCondition = 0;
    //复活数量 - 金币场
    public static int coinBaseEnsureCount = 0;
    //低保当前次数 - 金币场
    public static int currentCoinBaseEnsureTimes = 1;
    //低保总次数 - 金币场
    public static int totalCoinBaseEnsureTimes = 4;

    //金币消耗 - 红包场复活
    public static int ingotBaseEnsureCoinCost = 20000;
    //复活条件 - 红包场复活
    public static int ingotBaseEnsureCondition = 0;
    //低保当前次数 - 红包场复活
    public static int currentIngotBaseEnsureTimes = 1;
    //低保总次数 - 红包场复活
    public static int totalIngotBaseEnsureTimes = 4;

    public static List<Texture> adTextureList = new List<Texture>();

    //签到信息
    public static bool isSign = false;
    public static int signDay = 0;
    public static TagCheckInItem[] RewardCheckIn = new TagCheckInItem[10];
    public static TagSeriesCheckInReward[] SeriesRewardInfo = new TagSeriesCheckInReward[5];

    //缺省消息
    private static int defaultMessageIndex = 0;
    public static string defaultMessage
    {
        get
        {
            string msg = "欢迎来到[FF2929FF]兴隆斗地主[-]！";
            if (defaultMessageList.Count > 0)
            {
                if (defaultMessageIndex >= defaultMessageList.Count)
                {
                    defaultMessageIndex = 0;
                }
                msg = defaultMessageList[defaultMessageIndex];
                defaultMessageIndex++;
            }
            return msg;
        }
    }
    public static List<string> defaultMessageList = new List<string>();     //缺省消息列表
    public static List<string> messageList = new List<string>();            //系统消息列表

    //任务参数
    public static List<TagTaskParameter> taskParameter = new List<TagTaskParameter>();
    public static List<TagTaskStatus> taskStatus = new List<TagTaskStatus>(); //任务状态 128

    //挖宝参数
    public static int curPlayCount = 0;
    public static int totalPlayCount = 5;
    public static List<TagDigConigItem> digConfig = new List<TagDigConigItem>();
}
