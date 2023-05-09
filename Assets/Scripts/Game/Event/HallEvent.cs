
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HallEvent
{
    //登录界面
    public static UnityAction V_OpenPnlLogin;
    public static UnityAction V_ClosePnlLogin;
    //登录弹窗
    public static UnityAction V_OpenDlgLogin;
    public static UnityAction V_OpenDlgGetPassword;
    public static UnityAction V_OpenDlgRegister;
    //主界面
    public static UnityAction V_OpenPnlMain;
    public static UnityAction V_ClosePnlMain;
    
    //弹窗
    public static UnityAction V_OpenDlgUserInfo;                //个人中心
    public static UnityAction V_OpenDlgModifyNickName;          //修改昵称
    public static UnityAction<DlgBindArg> V_OpenDlgBindOp;      //绑定操作

    public static UnityAction V_OpenDlgRecord;                  //游戏记录
    public static UnityAction V_RefreshDlgRecord;               //刷新游戏记录

    public static UnityAction<DlgStoreArg> V_OpenDlgStore;      //商城
    //public static UnityAction<int> V_OpenDlgPayMode;            //支付模式

    public static UnityAction V_OpenDlgExchange;                //兑换

    public static UnityAction V_OpenDlgActivity;                //活动
    public static UnityAction V_RefreshPnlSign;                 //刷新签到页面
    public static UnityAction<CMD_Hall_S_SignInResult> V_OpenDlgWheelSignDay;                //转盘签到


    public static UnityAction V_OpenDlgShare;
    public static UnityAction V_OpenDlgEmail;                   //邮件
    public static UnityAction V_OpenDlgService;                 //客服
    public static UnityAction V_OpenDlgRule;                    //规则
    public static UnityAction<float, float, float, int> V_OpenDlgGetAward;                //获得奖励
    
    
    public static UnityAction V_OpenDlgGift;                    //福利
    public static UnityAction<int> V_OpenDlgMonthCard;          //月卡
    public static UnityAction V_CloseDlgMonthCard;          //月卡
    //public static UnityAction V_OpenDlgFirstCharge;             //首充
    public static UnityAction V_OpenDlgShareTask;             //分享有礼

    public static UnityAction V_OpenDlgRealAuth;                //实名认证
    public static UnityAction V_OpenDlgBaseEnsureRule;          //救济金

    public static UnityAction V_OpenDlgCreateRoom;
    public static UnityAction V_OpenDlgJoinRoom;                //打开加入房间界面
    public static UnityAction V_CloseDlgJoinRoom;               //关闭加入房间界面
    public static UnityAction<string> V_GetRoomFail;            //查找房间ID结果

    public static UnityAction<GameFlag, ushort> V_OpenDlgRoomSelect;    //打开房间选择界面

    public static UnityAction V_OpenDlgRank;
    public static UnityAction V_CloseDlgRank;

    public static UnityAction<UInt16, string> V_OpenDlgRoomInfo;
}

public enum DlgBindArg
{ 
    BindPhonePage,
    UnBindPhonePage,
    BindAgency,
    BindSpreader,
}

public enum DlgStoreArg
{ 

    CoinPage,
    CardPage,
    DiamondPage,
}
