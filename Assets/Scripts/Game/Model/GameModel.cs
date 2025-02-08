using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel
{

    #region 常量

    //服务器类型
    public const int ServerKind_Gold = 0x0001;               //金币场
    //public const ushort ServerKind_Score = 0x0002;              //积分场
    //public const ushort ServerKind_Match = 0x0004;              //比赛场
    //public const ushort ServerKind_Educate = 0x0008;            //练习场
    public const int ServerKind_Private = 0x0010;            //房卡场
    public const int ServerKind_RedPack = 0x0020;            //红包场
    //结算规则
    public const int PAY_USE_INGOT = 0x00000800;                //元宝结算 

    //参数定义
    public const UInt16 INVALID_CHAIR = 0xFFFF;								//无效椅子
    public const UInt16 INVALID_TABLE = 0xFFFF;								//无效桌子


    ////聊天规则
    //public const int SR_FORFEND_GAME_CHAT = 0x00000001;							    //禁止公聊
    //public const int SR_FORFEND_ROOM_CHAT = 0x00000002;						        //禁止公聊
    //public const int SR_FORFEND_WISPER_CHAT = 0x00000004;							//禁止私聊
    //public const int SR_FORFEND_WISPER_ON_GAME = 0x00000008;						//禁止私聊

    ////高级规则
    //public const int SR_ALLOW_DYNAMIC_JOIN = 0x00000010;							//动态加入
    //public const int SR_ALLOW_OFFLINE_TRUSTEE = 0x00000020;							//断线代打
    //public const int SR_ALLOW_AVERT_CHEAT_MODE = 0x00000040;						//隐藏信息

    ////游戏规则
    //public const int SR_RECORD_GAME_SCORE = 0x00000080;							    //记录积分
    //public const int SR_RECORD_GAME_TRACK = 0x00000100;							    //记录过程
    //public const int SR_DYNAMIC_CELL_SCORE = 0x00000200;							//动态底分
    //public const int SR_IMMEDIATE_WRITE_SCORE = 0x00000400;							//即时写分
    //public const int SR_USE_INGOT_PAY = 0x00000800;							        //元宝结算

    ////房间规则
    //public const int SR_FORFEND_ROOM_ENTER = 0x00001000;							//禁止进入
    //public const int SR_FORFEND_GAME_ENTER = 0x00002000;							//禁止进入
    //public const int SR_FORFEND_GAME_LOOKON = 0x00004000;							//禁止旁观

    ////银行规则
    //public const int SR_FORFEND_TAKE_IN_ROOM = 0x00010000;							//禁止取款
    //public const int SR_FORFEND_TAKE_IN_GAME = 0x00020000;							//禁止取款
    //public const int SR_FORFEND_SAVE_IN_ROOM = 0x00040000;							//禁止存钱
    //public const int SR_FORFEND_SAVE_IN_GAME = 0x00080000;							//禁止存款

    ////其他规则
    //public const int SR_FORFEND_GAME_RULE = 0x00100000;							    //禁止配置
    //public const int SR_FORFEND_LOCK_TABLE = 0x00200000;							//禁止锁桌
    //public const int SR_ALLOW_ANDROID_ATTEND = 0x00400000;							//允许陪玩
    //public const int SR_ALLOW_ANDROID_SIMULATE = 0x00800000;						//允许占位

    #endregion


    #region 音频

    public static AudioClip musicHall;
    public static AudioClip musicMJ;
    public static AudioClip musicHeLong;
    public static AudioClip musicHongWu;
    public static AudioClip musicNiuNiu;
    public static AudioClip musicShiSanShui;
    public static AudioClip musicSanMenMJ;
    public static AudioClip musicLandlords;

    public static AudioClip audioButtonAuto;
    public static AudioClip audioButtonNormal;
    public static AudioClip audioButtonClose;
    public static AudioClip audioButtonOp;
    public static AudioClip audioButtonSwitch;
    public static AudioClip audioGetAward;
    public static AudioClip audioMicroClip;
    public static AudioClip audioTimer;
    public static AudioClip audioTipWrong;
    public static AudioClip audioTipRight;
    public static AudioClip audioTipWarn;
    public static AudioClip audioTipChat;
    public static AudioClip audioScreenShot;

    public static AudioClip[] chatMen = new AudioClip[12];
    public static AudioClip[] chatWomen = new AudioClip[12];
    public static string[] chatMessage = new string[]
    {
        "不要吵了，专心玩游戏吧！",
        "不要走，决战到天亮！",
        "大家好，很高兴见到各位！",
        "不好意思，我要离开一会儿！",
        "和你合作真的太愉快了！",
        "快点儿，等得我花都谢了！",
        "你的牌打底也太好了！",
        "你是妹妹还是哥哥？！",
        "交个朋友吧，能告诉我你怎么联系吗?",
        "下次再玩吧，我要走了！",
        "再见了，我会想念大家的",
        "怎么又断线了，网络怎么这么差啊！"
    };
    public static Sprite[,] chatEmoji = new Sprite[9, 32];

    #endregion

    //自己信息
    public static UInt16 deskId = INVALID_TABLE;                    //桌号
    public static UInt16 chairId = INVALID_CHAIR;                   //座位号
    public static int roomDeskCount = 0;         //房间桌子数量
    public static int deskPlayerCount = 0;       //每张桌子玩家数量
    public static Int64 roomCoinLimit = 0;         //房间金币（钻石）限制

    public static List<TagTableState> tableList = new List<TagTableState>();                                  //牌桌列表
    public static Dictionary<int, uint> playerInDesk = new Dictionary<int, uint>();                           //同一桌玩家，chairId - userId
    public static Dictionary<uint, PlayerInRoom> playerInRoom = new Dictionary<uint, PlayerInRoom>();         //房间用户列表 

    public static bool isInGame = false;        //游戏是否正在进行中

    //游戏规则
    public static bool isLocalLanguage                  //是否本地方言
    {
        get
        {
            return PlayerPrefs.GetInt("LanguageMode", 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt("LanguageMode", value ? 0 : 1);
        }
    }

    public static int rulePlayerCount = 0;              //人数, 麻将中二人为8, 四人为0， 与服务器要求相对应
    public static int ruleGameCountIndex = 1;           //局数，对应下标
    public static int rulePayMode = 0;                  //房主支付为0， AA支付为2， 与服务器要求相对应
    public static int ruleGameGrade = 1;                //最低下注额， 与服务器要求相对应
    public static int ruleBaseScore = 1;                //游戏底分
    public static int gameRule = 0;                     //当前游戏规则，整形，与服务器对接参数

    public static int serverRule = 0;           //服务器规则
    public static int serverType = 0;           //服务器类型
    
    //局数信息
    public static int currentGameCount = 1;             //当前是第几局
    public static int totalGameCount = 3;               //总局数

    //红包进度
    public static int currentRedPackCount = 0;
    public static int totalRedPackTime = 300;
    public static int totalRedPackCount = 3;
    public static string redPackDescribe = "5分钟一场红包雨,满3局游戏,即可领取红包!";

    //网络速度
    public static int netSpeed = 2;                     //0~3之间的数字

    public static uint currentRoomId = 0;               //加入房间ID
    public static ulong hostUserId = 0;                   //房主ID
    public static int hogChairId = INVALID_CHAIR;               //庄家ID

    //音频初始化
    public static void InitRes()
    {
        musicHall = Resources.Load<AudioClip>("music/bgHall");
        musicMJ = Resources.Load<AudioClip>("music/bgMJ");
        musicHeLong = Resources.Load<AudioClip>("music/bgHeLong");
        musicHongWu = Resources.Load<AudioClip>("music/bgHongWu");
        musicNiuNiu = Resources.Load<AudioClip>("music/bgNiuNiu");
        musicShiSanShui = Resources.Load<AudioClip>("music/bgShiSanShui");
        musicSanMenMJ = Resources.Load<AudioClip>("music/bgSanMenMJ");
        musicLandlords = Resources.Load<AudioClip>("music/bgLandlords");

        audioButtonAuto = Resources.Load<AudioClip>("sound/normal/buttonAuto");
        audioButtonNormal = Resources.Load<AudioClip>("sound/normal/buttonNormal");
        audioButtonClose = Resources.Load<AudioClip>("sound/normal/buttonClose");
        audioButtonOp = Resources.Load<AudioClip>("sound/normal/buttonOp");
        audioButtonSwitch = Resources.Load<AudioClip>("sound/normal/buttonSwitch");
        audioGetAward = Resources.Load<AudioClip>("sound/normal/getAward");
        audioMicroClip = Resources.Load<AudioClip>("sound/normal/microClip");
        audioTimer = Resources.Load<AudioClip>("sound/normal/timer");
        audioTipWrong = Resources.Load<AudioClip>("sound/normal/tipWrong");
        audioTipRight = Resources.Load<AudioClip>("sound/normal/tipRight");
        audioTipWarn = Resources.Load<AudioClip>("sound/normal/tipWarn");
        audioTipChat = Resources.Load<AudioClip>("sound/normal/tipChat");
        audioScreenShot = Resources.Load<AudioClip>("sound/normal/screenShot");
        for (int i = 0; i < 12; i++)
        {
            chatMen[i] = Resources.Load<AudioClip>("sound/chat/man/chat_" + i);
            chatWomen[i] = Resources.Load<AudioClip>("sound/chat/woman/chat_" + i);
        }
        //表情资源
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                chatEmoji[i, j] = Resources.Load<Sprite>("texture/chat/emoji_" + i + "_" + j);
            }
        }
    }

    //取得房间用户
    public static PlayerInRoom GetDeskUser(int chairId)
    {
        if (playerInDesk.ContainsKey(chairId) && playerInRoom.ContainsKey(playerInDesk[chairId]))
        {
            return playerInRoom[playerInDesk[chairId]];
        }
        return null;
    }

    //取得用户的头像
    public static Texture GetUserPhoto(int chairId)
    {
        PlayerInRoom player = GetDeskUser(chairId);
        if (player == null)
        {
            return HallModel.defaultPhoto;
        }
        else
        {
            UInt64 userId = (UInt64)player.dwUserID;
            if (HallModel.userPhotos.ContainsKey(userId))
            {
                return HallModel.userPhotos[userId];
            }
            else
            {
                return HallModel.defaultPhoto;
            }
        }
    }

    //获取用户
    public static Vector2 GetUserGPSPos(int chairId)
    {
        Vector2 pos = Vector2.zero;
        PlayerInRoom player = GetDeskUser(chairId);
        if (player != null)
        {
            pos = new Vector2(player.fLongitude, player.fLatitude);
        }
        return pos;
    }

    //通过userId获取用户头像
    public static Texture GetUserPhotoByUserId(UInt64 userId)
    {
        if (HallModel.userPhotos.ContainsKey(userId))
        {
            return HallModel.userPhotos[userId];
        }
        else
        {
            return HallModel.defaultPhoto;
        }
    }

    public static Sprite[] GetEmojiSprites(int index)
    {
        int length = 0;
        for (int i = 3; i < 32; i++)
        {
            if (chatEmoji[index, i] == null)
            {
                length = i + 1;
                break;
            }
        }
        Sprite[] arr = new Sprite[length];
        for (int i = 0; i < length; i++)
        {
            arr[i] = chatEmoji[index, i];
        }
        return arr;
    }

    //元宝结算
    public static bool IsUseIngotPay()
    {
        return (serverRule & PAY_USE_INGOT) != 0;
    }

}

