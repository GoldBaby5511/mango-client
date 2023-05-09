using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent
{
    public static UnityAction<string> Test;
    //提示信息
    public static UnityAction<string, string, UnityAction, UnityAction> V_OpenDlgTip;
    public static UnityAction<string> V_OpenShortTip;
    public static UnityAction<string> V_OpenConnectTip;
    public static UnityAction V_CloseConnectTip;
    //领取低保提示
    public static UnityAction<string, string, UnityAction> V_OpenDlgBaseEnsure;
    //首充
    public static UnityAction<UnityAction> V_OpenDlgFirstCharge;             //首充
    public static UnityAction V_CloseDlgFirstCharge;             //首充
    public static UnityAction<DlgStoreArg> V_OpenDlgStoreInGame;      //商城
    public static UnityAction V_CloseDlgStoreInGame;      //商城
    public static UnityAction<int> V_OpenDlgPayMode;            //支付模式
    //离开游戏
    public static UnityAction V_OpenLeaveGame;
    //关闭离开提示
    public static UnityAction V_CloseLeaveGame;

    //计时器
    public static UnityAction<int, int, UnityAction> V_StartTimer;
    public static UnityAction V_CloseTimer;
    public static UnityAction V_StopTimeOutCallback;
    //刷新用户信息
    public static UnityAction<bool> V_RefreshUserInfo;      //参数 ： 是否刷新得分信息
    //刷新房间信息
    public static UnityAction V_RefreshRoomInfo;
    //设置面板
    public static UnityAction V_OpenDlgSet;
    //规则面板
    public static UnityAction V_OpenDlgRule;
    //红包规则
    public static UnityAction V_OpenDlgRedRule;
    //解散房间弹窗
    public static UnityAction<CMD_Game_S_DisRoomInfo> V_OpenDlgDisRoom;
    public static UnityAction V_CloseDlgDisRoom;
    //聊天窗口
    public static UnityAction V_OpenDlgChat;
    public static UnityAction V_OpenFlagAudio;
    public static UnityAction V_CloseFlagAudio;
    //每日任务
    public static UnityAction V_OpenDlgTask;
    public static UnityAction<CMD_CM_S_TaskResult> V_RefreshDlgTask;

    //挖宝窗口
    public static UnityAction<CMD_CM_S_DigTreasure> V_OpenDlgDigTreasure;



    //刷新桌子信息
    public static UnityAction S_RefreshDeskState;
    //收到聊天信息
    public static UnityAction<ChatMessage> S_ReceiveChatMessage;

    public static UnityAction V_RefreshRedPackState;
    public static UnityAction V_RefreshDigTreasureState;
    public static UnityAction<CMD_Game_S_GetRedPack> V_OpenRedPackButton;
    public static UnityAction<float, string> V_PlayRedPackAnim;

    //房间解散
    public static UnityAction S_OnDisRoom;
    public static UnityAction<CMD_Game_S_TotalScore> S_GetTotalScore;         //房卡结束 - 总结算消息
    //系统消息
    public static UnityAction S_ReceiveSystemMsg;

    //等待匹配
    public static UnityAction<bool> V_ShowWaitMatch;

}
