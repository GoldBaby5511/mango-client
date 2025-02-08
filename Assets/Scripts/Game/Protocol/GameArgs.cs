using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// 游戏资源更新状态
/// </summary>
public enum GameUpdateState
{
    Null = 0,           //最新版本，不需要更新
    Update = 1,         //需要更新
    Download = 2,       //需要下载
}

/// <summary>
/// 连接类型
/// </summary>
public enum ConnectType
{
    Normal,             //常规链接
    ConnectOnBreak,     //断线重连
}

/// <summary>
/// 登录类型
/// </summary>
public enum LoginType
{
    Account,       //账号登录
    Visitor,       //游客登录
    OtherSDK,      //第三方登录
    Register,       //注册
}

/// <summary>
/// 自定义游戏标识
/// </summary>
public enum GameFlag
{
    Hall = 0,

    Landlords3 = 200,                   //斗地主 - 3人
}

/// <summary>
/// 登录游戏成功后操作
/// </summary>
public enum OpOnLginGame
{ 
    Null,           //无状态
    GetChair,       //请求座位 - 金币场            
    CreateRoom,     //创建房间
    JoinRoom,       //加入房间
}

/// <summary>
/// 游戏信息
/// </summary>
public class GameInfo
{
    public GameFlag flag;       //名称，如NiuNiu4
    public UInt16 kindId;       //类型ID
    public string gameName;     //游戏名称，如四人牛牛
    public string sceneName;    //游戏对应场景名称
    public GameUpdateState updateState;     //游戏更新状态

    public GameInfo(GameFlag _flag, string _gameName, string _sceneName)
    {
        flag = _flag;
        kindId = (UInt16)_flag;
        gameName = _gameName;
        sceneName = _sceneName;
        updateState = GameUpdateState.Null;
    }
}

/// <summary>
/// 商品信息
/// </summary>
[Serializable]
public class GoodInfo
{
    public int RechargeID;
    public string RechargeName;        
    public int RechargePrice;
    public int Count;               //首充、月卡此字段代表 钻石数
    public int Type;                //1-房卡   2-钻石    3-金币    4-月卡    5-首充
    public float GiveRate;          //赠送比例
    public int GiveCount;           //赠送数量 首充、月卡此字段代表 金币数
}

/// <summary>
/// 邮件信息
/// </summary>
[Serializable]
public class EmailInfo
{ 
    public int EmailId;
    public string EmailContent;
    public int EmailType;
    public string AddTime;
    public int IsRead;
    public string EmailTitle;
}

/// <summary>
/// 兑换商品信息
/// </summary>
[Serializable]
public class ExchangeInfo
{ 
    public int ID;
    public string Name;             //道具名称
    public string RegulationsInfo;  //道具描述
    public int Kind;                //类型        18-兑换红包  19-兑换钻石 20-兑换金币 21-兑换房卡 8-月卡福利
    public int Cash;                //红包券数量
    public int UseResultsCash;      //使用得到现金
    public int UseResultsGold;      //使用得到金币
    public int UseResultsIngot;     //使用得到钻石
    public int UseResultsRoomCard;  //使用得到房卡
}

/// <summary>
/// 兑换记录
/// </summary>
[Serializable]
public class ExchangeRecord
{ 
    public int RecordID;            //编号
    public int PropertyID;          //商品ID
    public string PropertyName;     //商品名称
    public string UseDate;          //兑换时间
}

/// <summary>
/// 道具信息
/// </summary>
[Serializable]
public class PropInfo
{
    public int GoodsID;         //道具ID  110-房卡 620月卡
    public int GoodsCount;      //道具数量
}

/// <summary>
/// 广告图信息
/// </summary>
[Serializable]
public class AdTextureInfo
{
    public int ID;
    public string URL;
}

/// <summary>
/// 游戏常量定义
/// </summary>
public class UserState
{
    //用户状态
    public const byte US_NULL = 0;							//没有状态
    public const byte US_FREE = 1;							//站立状态
    public const byte US_SIT = 2;							//坐下状态
    public const byte US_READY = 3;							//同意状态
    public const byte US_LOOKON = 4;						//旁观状态
    public const byte US_PLAYING = 5;						//游戏状态
    public const byte US_OFFLINE = 6;						//断线状态
}

/// <summary>
/// 提示信息类型
/// </summary>
public enum TipType
{ 
    Short,      //短提示
    Long,       //长提示
    Dialog,     //弹窗提示
}

/// <summary>
/// 聊天类型
/// </summary>
public enum ChatMessageType
{
    Text = 0,
    Audio = 1,
    Shorter = 2,
    Emoji = 3,
}

/// <summary>
/// 聊天信息
/// </summary>
public class ChatMessage
{
    public ChatMessageType type = ChatMessageType.Audio;

    public int chairId;         //发送者座位ID
    public string userName;     //发送者昵称
    public int sex;             //发送者性别
    public Sprite photon;       //发送者头像

    public string time;

    public string message;
    public AudioClip clip;

    public ChatMessage()
    { }

    public ChatMessage(AudioClip audio)
    {
        type = ChatMessageType.Audio;
        clip = audio;
    }

    public ChatMessage(ChatMessageType t, string m)
    {
        type = t;
        message = m;
    }



    /// <summary>
    /// 消息编码，便于服务端发送，格式 【类型】+【表情】+【内容】
    /// </summary>
    public void EncodeMessage()
    {
        message = (int)type + "&" + message;
    }

    /// <summary>
    /// 消息解码
    /// </summary>
    public void DecodeMessage()
    {
        try
        {
            string[] str = message.Split('&');
            type = (ChatMessageType)int.Parse(str[0]);
            message = str[1];
        }
        catch (Exception e)
        {
            type = ChatMessageType.Text;
            message = "聊天消息解码异常 ： " + e.Message;
        }
    }
}

//基础信息
public class ServiceBaseInfo
{
    public string strName;
    public UInt32 dwType;
    public UInt32 dwID;
    public string strListenOnAddr;
    public string strCenterAddr;
};

/// <summary>
/// 游戏类型信息
/// </summary>
public class GameKindInfo
{
    public UInt16 wTypeID;							//分类索引  1~4 捕鱼 休闲 多人 棋牌
    public UInt16 wJoinID;							//挂接索引
    public UInt16 wSortID;							//排序索引
    public UInt16 wKindID;							//游戏索引 
    public UInt16 wGameID;							//模块索引
    public byte cbSuportType;						//支持类型
    public UInt16 wRecommend;						//推荐游戏
    public UInt16 wGameFlag;						//游戏标志
    public UInt32 dwOnLineCount;					//在线人数
    public UInt32 dwAndroidCount;					//在线人数
    public UInt32 dwFullCount;						//满员人数
    public string szKindName;				//游戏名字 64
    public string szProcessName;			//进程名字	 64
}

/// <summary>
/// 游戏服务器信息<房间>
/// </summary>
public class GameServerInfo
{
    public ServiceBaseInfo baseInfo;    //基础信息
    public UInt16 wKindID;              //名称索引 - 游戏类型
    public UInt16 wNodeID;              //节点索引
    public UInt16 wSortID;              //排序索引
    public UInt16 wServerID;            //房间索引
    public UInt16 wServerKind;          //房间类型
    public UInt16 wServerType;          //房间类型
    public UInt16 wServerLevel;         //房间等级
    public UInt16 wServerPort;          //房间端口
    public Int64 lCellScore;            //单元积分
    public Int16 wRatio;                //税收比例
    public Int64 serviceMoney;          //服务费
    public Byte cbEnterMember;          //进入会员
    public Int64 lEnterScore;           //进入积分
    public UInt32 dwServerRule;         //房间规则
    public UInt32 dwOnLineCount;        //在线人数
    public UInt32 dwAndroidCount;       //机器人数
    public UInt32 dwFullCount;          //满员人数
    public string szServerAddr;         //房间名称 - IP地址
    public string szServerName;			//房间名称
}

/// <summary>
/// 房卡游戏服务器信息
/// </summary>
public class CardGameServerInfo
{
    public UInt32 dwUserID;				//用户标识
    public UInt16 wKindID;				//类型标识
    public UInt16 wServerID;			//房间标识
    public UInt32 dwVersion;			//游戏版本
    public UInt32 dwRoomID;             //房间ID
    public UInt32 dwRoomNum;			//房间号码
    public UInt32 dwFanTimes;			//游戏蕃数
    public UInt32 dwMaxTimes;			//游戏封顶
    public UInt32 dwGameRule;			//游戏规则
    public byte cbRoomType;				//房间类型	
    public byte[] cbPlayCout = new byte[4];			    //玩家局数
    public Int64[] lPlayCost = new Int64[4];			//消耗点数 房主支付
    public Int64[] lAAPlayCost = new Int64[4];			//消耗点数 AA
    public Int64 lCostGold;				//消耗点数
    public byte cbPlayCountIndex;		//局数索引
}

/// <summary>
/// 用户状态
/// </summary>
public class TagUserState
{
    public UInt16 wTableID;							//桌子索引
    public UInt16 wChairID;							//椅子位置
    public Byte cbUserStatus;						//用户状态
}

/// <summary>
/// 桌子状态
/// </summary>
public class TagTableState
{
    public byte cbTableLock;						//锁定标志
    public byte cbPlayStatus;						//游戏标志
    public int lCellScore;							//单元积分
}

/// <summary>
/// 用户分数
/// </summary>
public class TagUserScore
{
    //积分信息
    public Int64 lScore;							//用户分数
    public Int64 lGrade;							//用户成绩
    public Int64 lInsure;							//用户银行
    public Int64 lIngot;							//用户元宝
    public Int64 repackCount;                       //红包数
    public double dBeans;							//用户游戏豆
     
    //输赢信息
    public UInt32 dwWinCount;						//胜利盘数
    public UInt32 dwLostCount;						//失败盘数
    public UInt32 dwDrawCount;						//和局盘数
    public UInt32 dwFleeCount;						//逃跑盘数
    public Int64 lIntegralCount;					//积分总数(当前房间)
     
    //全局信息
    public UInt32 dwExperience;						//用户经验
    public uint lLoveLiness;						//用户魅力
}

//MQ推送信息
public class TagRabbitMQInfo
{
    public UInt32 dwUserID;                  // 用户ID
    public int nBuyType;                  // 购买类型 2钻石、3金币、4月卡、5首充
    public int nAwardID;                  // 具体商品ID（唯一）
    public int nPayType;                  // 付款类型 4微信、5支付宝
};

//签到配置
public class TagCheckInItem
{
    public int nConfigID;                   //配置ID
    public int nPropID;                     //道具ID  0钻石、1金币、600礼券
    public int nGiveCount;                 //赠送数量
    public byte cbBigReward;            //是否大奖
};

//连续签到奖励
public class TagSeriesCheckInReward
{
    public int nSeriesDays;
    public TagCheckInItem RewardItem = new TagCheckInItem();
};

/// <summary>
/// 任务参数
/// </summary>
public class TagTaskParameter
{
    //基本信息
    public UInt16 wTaskID;                           //任务标识
    public UInt16 wTaskType;                         //任务类型
    public UInt16 wTaskObject;                       //任务目标
    public byte cbPlayerType;                      //玩家类型
    public UInt16 wKindID;                           //类型标识
    public UInt32 dwTimeLimit;                      //时间限制

    //奖励信息
    public int nStandardAwardPropID;                   //奖励道具 0钻石、1金币、600礼券
    public int nStandardAwardPropCount;                //奖励数量
    public int nMemberAwardPropID;                 //奖励道具
    public int nMemberAwardPropCount;					//奖励数量
    public int nActivityCount;                                     //活跃度奖励

    //描述信息
    public string szTaskName;            //任务名称 128
    public string szTaskDescribe;				//任务描述 640
};

/// <summary>
/// 任务状态
/// </summary>
public class TagTaskStatus
{
    public UInt16 wTaskID;                           //任务标识
    public UInt16 wTaskProgress;                     //任务进度
    public byte cbTaskStatus;						//任务状态 0 为未完成  1为成功   2为失败  3已领奖
};

/// <summary>
/// 挖宝配置
/// </summary>
public class TagDigConigItem
{
    public int nConfigID;
    public int nPropID;
    public int nPropCount;
};

/// <summary>
/// 系统时间结构体
/// </summary>
public class TimeStruct
{
    public UInt16 wYear;
    public UInt16 wMonth;
    public UInt16 wDayOfWeek;
    public UInt16 wDay;
    public UInt16 wHour;
    public UInt16 wMinute;
    public UInt16 wSecond;
    public UInt16 wMilliseconds;

    public override string ToString()
    {
        return wYear + "-" + wMonth + "-" + wDay + " " + wHour + ":" + wMinute;
    }

    public int CompareTo(TimeStruct other)
    {
        //年
        if (wYear > other.wYear)
        {
            return 1;
        }
        else if (wYear < other.wYear)
        {
            return -1;
        }
        else
        {
            //月
            if (wMonth > other.wMonth)
            {
                return 1;
            }
            else if (wMonth < other.wMonth)
            {
                return -1;
            }
            else
            { 
                //日
                if (wDay > other.wDay)
                {
                    return 1;
                }
                else if (wDay < other.wDay)
                {
                    return -1;
                }
                else
                {
                    //时
                    if (wHour > other.wHour)
                    {
                        return 1;
                    }
                    else if (wHour < other.wHour)
                    {
                        return -1;
                    }
                    else
                    {
                        //分
                        if (wMinute > other.wMinute)
                        {
                            return 1;
                        }
                        else if (wMinute < other.wMinute)
                        {
                            return -1;
                        }
                        else
                        {
                            //秒
                            if (wSecond > other.wSecond)
                            {
                                return 1;
                            }
                            else if (wSecond < other.wSecond)
                            {
                                return -1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                }
            }
        }
    }
}

/// <summary>
/// 房间中玩家信息
/// </summary>
public class PlayerInRoom
{
    //用户属性
    public UInt32 dwGameID;							//游戏 I D
    public UInt32 dwUserID;							//用户 I D
    public UInt32 dwGroupID;						//社团 I D

    //头像信息
    public UInt16 wFaceID;							//头像索引   
    public UInt32 dwCustomID;						//自定标识

    //用户属性
    public bool bIsAndroid;							//机器标识
    public Byte cbGender;							//用户性别
    public Byte cbMemberOrder;						//会员等级
    public Byte cbMasterOrder;						//管理等级
    public UInt32 dwUserType;						//用户类型

    //用户状态
    public UInt16 wTableID;							//桌子索引
    public UInt16 wChairID;							//椅子索引
    public Byte cbUserStatus;						//用户状态

    //GPS信息
    public float fLatitude;										//用户纬度
    public float fLongitude;									//用户经度
    public float fHeight;										//用户高度

    //积分信息
    public Int64 lScore;						    //用户分数
    public Int64 lGrade;						    //用户成绩
    public Int64 lInsure;							//用户银行
    public Int64 lIngot;							//用户元宝
    public Int64 redPack;                           //用户红包
    public Double dBeans;							//用户游戏豆

    //游戏信息
    public UInt32 dwWinCount;						//胜利盘数
    public UInt32 dwLostCount;						//失败盘数
    public UInt32 dwDrawCount;						//和局盘数
    public UInt32 dwFleeCount;						//逃跑盘数
    public UInt32 dwExperience;						//用户经验
    public UInt32 lLoveLiness;						//用户魅力
    public Int64 lIntegralCount;					//积分总数(当前房间)

    //代理信息
    public UInt32 dwAgentID;						//代理ID

    public UInt16 dataSize;
    public UInt16 dataDescrible;
    public string nickName;                         //玩家昵称
}

/// <summary>
/// 玩家游戏中状态
/// </summary>
public class PlayerStateInGame
{ 
    //用户信息
	public UInt32 dwUserID;							//用户标识
    public UInt32 dwGameID;							//游戏标识
    public string szNickName;			            //用户昵称 64
    public UInt16 wFaceID;							//头像索引

	//等级信息
    public byte cbGender;							//用户性别
    public byte cbMemberOrder;						//会员等级
    public byte cbMasterOrder;						//管理等级

	//位置信息
    public UInt16 wKindID;							//类型标识
    public UInt16 wServerID;						//房间标识
    public string szGameServer;			            //房间位置 32

	//用户状态
    public UInt16 wTableID;							//桌子索引
    public UInt16 wLastTableID;					    //游戏桌子
    public UInt16 wChairID;							//椅子索引
    public byte cbUserStatus;						//用户状态
}




/// <summary>
/// 扑克牌信息
/// </summary>
public class Poker
{
    public int index;               //编码,与服务端发牌标号对应
    public string name = "";        //名称，与图集名称对应
    public int value;               //牌的值，游戏中自由设定
    public int trueValue;           //牌的值，固定
    public int typeValue;           //花色对应值

    public Poker(int _index, string _name, int _value, int _typeValue)
    {
        index = _index;
        name = _name;
        value = _value;
        typeValue = _typeValue;
    }

    public Poker(int _index, string _name, int _value, int _trueValue, int _typeValue)
    {
        index = _index;
        name = _name;
        value = _value;
        trueValue = _trueValue;
        typeValue = _typeValue;
    }
}




/// <summary>
/// 牛牛牌型结果
/// </summary>
public class NiuCardResult
{
    public int code;                        //计算结果 0~10 分别对应 无牛~牛牛，11五花牛，12五金，13炸弹，14五小
    public string name;                     //结果名称 对应图集名称
    public int[] upIndex = new int[3];      //有牛三张牌下标
    public int[] downIndex = new int[2];    //无牛三张牌下标
    public int clipIndex;                   //结算结果对应音频文件的下标

    public NiuCardResult()
    {
        code = 0;
        name = "Niu_0";
        upIndex = new int[3];
        downIndex = new int[2];
        clipIndex = 0;
    }
}

/// <summary>
/// 河龙扑克组合
/// </summary>
public class HeLongPokerWeave
{
    public int cardCount;
    public int cardValue;
    public int lineCount;           //王的统计中，线数不等于牌张数
    public List<byte> cardList;

    public HeLongPokerWeave()
    {
        cardCount = 0;
        cardValue = 0;
        lineCount = 0;
        cardList = new List<byte>();
    }

    public HeLongPokerWeave(int count, int value, int line, byte data)
    {
        cardCount = count;
        cardValue = value;
        lineCount = line;
        cardList = new List<byte>();
        cardList.Add(data);
    }
}
