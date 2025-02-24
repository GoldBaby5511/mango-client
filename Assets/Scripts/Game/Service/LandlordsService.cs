﻿using UnityEngine;
using System.Collections;

public class LandlordsService : MonoBehaviour 
{
    public static LandlordsService Instance;

    void Awake()
    {
        //Debug.Log("AddHandler");
        Instance = this;
        HallService.Instance.client.netManager.AddHandler(new LandlordsHandler());
    }

    #region 发送
    //用户叫分
    public void C2S_CallScore(byte cbCallScore)
    {
        Bs.Gameddz.C_RobLand req = new Bs.Gameddz.C_RobLand();
        req.RobLand = cbCallScore;
        //Debug.Log("用户叫分,cbCallScore=" + cbCallScore + ",RobLand=" + req.RobLand);
        GameService.Instance.client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.MDM_GF_GAME, (System.UInt32)(Bs.Gameddz.CMDGameddz.IdCCallScore), req);
        
        //CMD_Landlords_C_CallScore pro = new CMD_Landlords_C_CallScore();
        //pro.cbCallScore = cbCallScore;
        //GameService.Instance.client.SendPro(pro);
    }

    //用户加倍
    public void C2S_AddTime(byte cbAddTimes)
    {
        Bs.Gameddz.C_AddTimes req = new Bs.Gameddz.C_AddTimes();
        req.AddTimes = cbAddTimes;
        GameService.Instance.client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.MDM_GF_GAME, (System.UInt32)(Bs.Gameddz.CMDGameddz.IdCAddtimes), req);

        //CMD_Landlords_C_AddTimes pro = new CMD_Landlords_C_AddTimes();
        //pro.cbAddTimes = cbAddTimes;
        //GameService.Instance.client.SendPro(pro);
    }

    //用户出牌
    public void C2S_OutCard(byte cardCount, byte[] cardData)
    {
        Bs.Gameddz.C_OutCard req = new Bs.Gameddz.C_OutCard();
        for(int i = 0; i< cardCount; i ++)
        {
            req.CardData.Add(cardData[i]);
        }
        GameService.Instance.client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.MDM_GF_GAME, (System.UInt32)(Bs.Gameddz.CMDGameddz.IdCOutCard), req);

        //CMD_Landlords_C_OutCard pro = new CMD_Landlords_C_OutCard();
        //pro.cbCardCount = cardCount;
        //pro.cbCardData = cardData;
        //GameService.Instance.client.SendPro(pro);
    }

    //用户放弃
    public void C2S_PassCard()
    {
        GameService.Instance.client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.MDM_GF_GAME, (System.UInt32)(Bs.Gameddz.CMDGameddz.IdCPassCard), null);

        //CMD_Landlords_C_PassCard pro = new CMD_Landlords_C_PassCard();
        //GameService.Instance.client.SendPro(pro);
    }

    //托管
    public void C2S_Trustee(byte bTrustee)
    {
        Bs.Gameddz.C_TRUSTEE req = new Bs.Gameddz.C_TRUSTEE();
        req.Trustee = bTrustee;
        GameService.Instance.client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.MDM_GF_GAME, (System.UInt32)(Bs.Gameddz.CMDGameddz.IdCTrustee), req);

        //CMD_Landlords_C_Trustee pro = new CMD_Landlords_C_Trustee();
        //pro.bTrustee = bTrustee;
        //GameService.Instance.client.SendPro(pro);
    }
    #endregion

    #region 接收
    //空闲状态
    public void S2C_StateFree(Bs.Gameddz.S_StatusFree pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = false;
        for(int i = 0; i < pro.PlayStatus.Count; i ++)
        {
            LandlordsModel.playerInGame[i] = (byte)pro.PlayStatus[i];
        }
        LandlordsModel.callTime = (int)pro.TimeCallLand;
        LandlordsModel.addTime = (int)pro.TimeAddTime;
        LandlordsModel.firstOutCardTime = (int)pro.TimeHeadOutCard;
        LandlordsModel.outCardTime = (int)pro.TimeOutCard;
        LandlordsModel.canotAfford = (int)pro.TimePassCard;

        //LandlordsModel.playerInGame = pro.cbPlayStatus;
        //LandlordsModel.callTime = pro.cbTimeCallScore;
        //LandlordsModel.addTime = pro.cbTimeAddTime;
        //LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        //LandlordsModel.outCardTime = pro.cbTimeOutCard;
        //LandlordsModel.canotAfford = pro.cbTimePassCard;
        //用户头像为空时，重新加载头像信息
        for (int i = 0; i < 3; i++)
        {
            PlayerInRoom player = GameModel.GetDeskUser(i);
            if (player != null && GameModel.GetUserPhoto(i) == HallModel.defaultPhoto)
            {
                HallService.Instance.QueryUserInfo(player.dwUserID);
            }
        }
        try
        {
            //界面操作
            Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
            Util.Instance.DoAction(LandlordsEvent.S_StateFree, pro);
        }
        catch (System.Exception e)
        {
            Debug.Log("e.Message  :" + e.Message);
            throw;
        }
        
    }

    //叫分状态
    public void S2C_StatusCall(Bs.Gameddz.S_StatusCall pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = true;
        for (int i = 0; i < pro.PlayStatus.Count; i++)
        {
            LandlordsModel.playerInGame[i] = (byte)pro.PlayStatus[i];
        }
        LandlordsModel.callTime = (int)pro.TimeCallLand;
        LandlordsModel.addTime = (int)pro.TimeAddTime;
        LandlordsModel.firstOutCardTime = (int)pro.TimeHeadOutCard;
        LandlordsModel.outCardTime = (int)pro.TimeOutCard;
        LandlordsModel.canotAfford = (int)pro.TimePassCard;

        //LandlordsModel.playerInGame = pro.cbPlayStatus;
        //LandlordsModel.callTime = pro.cbTimeCallScore;
        //LandlordsModel.addTime = pro.cbTimeAddTime;
        //LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        //LandlordsModel.outCardTime = pro.cbTimeOutCard;
        //LandlordsModel.canotAfford = pro.cbTimePassCard;
        //用户头像为空时，重新加载头像信息
        for (int i = 0; i < 3; i++)
        {
            LandlordsModel.isPlayerTrustee[i] = (pro.UserTrustee[i] == 1);
            PlayerInRoom player = GameModel.GetDeskUser(i);
            if (player != null && GameModel.GetUserPhoto(i) == HallModel.defaultPhoto)
            {
                HallService.Instance.QueryUserInfo(player.dwUserID);
            }
        }
        //界面操作
        Util.Instance.DoAction(GameEvent.V_ShowWaitMatch, false);
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
        Util.Instance.DoAction(LandlordsEvent.S_StateCall, pro);
        Util.Instance.DoAction(LandlordsEvent.V_CloseDlgResult);
    }

    //加倍状态
    public void S2C_StatusAddTime(Bs.Gameddz.S_StatusAddTimes pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = true;
        for (int i = 0; i < pro.PlayStatus.Count; i++)
        {
            LandlordsModel.playerInGame[i] = (byte)pro.PlayStatus[i];
        }
        LandlordsModel.bankerChairdId = (int)pro.LandUser;
        LandlordsModel.callTime = (int)pro.TimeCallLand;
        LandlordsModel.addTime = (int)pro.TimeAddTime;
        LandlordsModel.firstOutCardTime = (int)pro.TimeHeadOutCard;
        LandlordsModel.outCardTime = (int)pro.TimeOutCard;
        LandlordsModel.canotAfford = (int)pro.TimePassCard;

        //LandlordsModel.playerInGame = pro.cbPlayStatus;
        //LandlordsModel.bankerChairdId = pro.wBankerChairId;
        //LandlordsModel.callTime = pro.cbTimeCallScore;
        //LandlordsModel.addTime = pro.cbTimeAddTime;
        //LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        //LandlordsModel.outCardTime = pro.cbTimeOutCard;
        //LandlordsModel.canotAfford = pro.cbTimePassCard;
        //用户头像为空时，重新加载头像信息
        for (int i = 0; i < 3; i++)
        {
            LandlordsModel.isPlayerTrustee[i] = (pro.UserTrustee[i] == 1);
            PlayerInRoom player = GameModel.GetDeskUser(i);
            if (player != null && GameModel.GetUserPhoto(i) == HallModel.defaultPhoto)
            {
                HallService.Instance.QueryUserInfo(player.dwUserID);
            }
        }
        //界面操作
        Util.Instance.DoAction(GameEvent.V_ShowWaitMatch, false);
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
        Util.Instance.DoAction(LandlordsEvent.S_StateAddTime, pro);
        Util.Instance.DoAction(LandlordsEvent.V_CloseDlgResult);
    }

    //游戏状态
    public void S2C_StatusPlay(Bs.Gameddz.S_StatusPlay pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = true;
        for (int i = 0; i < pro.PlayStatus.Count; i++)
        {
            LandlordsModel.playerInGame[i] = (byte)pro.PlayStatus[i];
        }
        LandlordsModel.bankerChairdId = (int)pro.BankerUser;
        LandlordsModel.callTime = (int)pro.TimeCallLand;
        LandlordsModel.addTime = (int)pro.TimeAddTime;
        LandlordsModel.firstOutCardTime = (int)pro.TimeHeadOutCard;
        LandlordsModel.outCardTime = (int)pro.TimeOutCard;
        LandlordsModel.canotAfford = (int)pro.TimePassCard;

        //LandlordsModel.playerInGame = pro.cbPlayStatus;
        //LandlordsModel.bankerChairdId = pro.wBankerUser;
        //LandlordsModel.callTime = pro.cbTimeCallScore;
        //LandlordsModel.addTime = pro.cbTimeAddTime;
        //LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        //LandlordsModel.outCardTime = pro.cbTimeOutCard;
        //LandlordsModel.canotAfford = pro.cbTimePassCard;
        //用户头像为空时，重新加载头像信息
        for (int i = 0; i < 3; i++)
        {
            LandlordsModel.isPlayerTrustee[i] = (pro.UserTrustee[i] == 1);
            PlayerInRoom player = GameModel.GetDeskUser(i);
            if (player != null && GameModel.GetUserPhoto(i) == HallModel.defaultPhoto)
            {
                HallService.Instance.QueryUserInfo(player.dwUserID);
            }
        }
        //界面操作
        Util.Instance.DoAction(GameEvent.V_ShowWaitMatch, false);
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
        Util.Instance.DoAction(LandlordsEvent.S_StatePlay, pro);
        Util.Instance.DoAction(LandlordsEvent.V_CloseDlgResult);
    }

    //游戏开始
    public void S2C_GameStart(Bs.Gameddz.S_GameStart pro)
    {
        //保存数据
        GameModel.isInGame = true;
        Util.Instance.DoAction(GameEvent.V_CloseLeaveGame);
        for (int i = 0; i < pro.PlayStatus.Count; i++)
        {
            LandlordsModel.playerInGame[i] = (byte)pro.PlayStatus[i];
        }
        Util.Instance.DoAction(GameEvent.V_ShowWaitMatch, false);
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
        Util.Instance.DoAction(LandlordsEvent.S_GameStart, pro);
        Util.Instance.DoAction(LandlordsEvent.V_CloseDlgResult);
    }

    //重新发牌
    public void S2C_ReSendCard(Bs.Gameddz.S_ReOutCard pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_ReSendCard, pro); 
    }

    //机器人扑克
    public void S2C_AndroidCard(CMD_Landlords_S_AndroidCard pro)
    {
        //Debug.Log("S2C----------------------------机器人扑克");
    }

    //作弊扑克
    public void S2C_CheatCard(Bs.Gameddz.S_CheatCard pro)
    {
        //Debug.Log("S2C----------------------------作弊扑克");
    }

    //用户叫分
    public void S2C_CallScore(Bs.Gameddz.S_RobLand pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_UserCall, pro);
    }

    //庄家信息
    public void S2C_BankerInfo(Bs.Gameddz.S_BankerInfo pro)
    {
        LandlordsModel.bankerChairdId = (int)pro.BankerUser;
        for(int i = 0; i < pro.BankerCard.Count; i ++)
        {
            LandlordsModel.bankerHoleCards[i] = (byte)pro.BankerCard[i];
            //Debug.Log("庄家信息,card=" + LandlordsModel.bankerHoleCards[i]);
        }

        Util.Instance.DoAction(LandlordsEvent.S_BankerStartOutCard, pro);
    }

    //用户加倍
    public void S2C_AddTimes(Bs.Gameddz.S_AddTimes pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_UserAddTime, pro);
    }

    //用户出牌
    public void S2C_OutCard(Bs.Gameddz.S_OutCard pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_UserOutCard, pro);
    }

    //出牌错误
    public void S2C_OutCardFail(Bs.Gameddz.S_OutCardFail pro)
    {  
        Util.Instance.DoAction(LandlordsEvent.S_OutCardFail, pro);
    }

    //放弃出牌
    public void S2C_PassCard(Bs.Gameddz.S_PassCard pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_PassCard, pro);
    }

    //游戏结束
    public void S2C_GameEnd(Bs.Gameddz.S_GameConclude pro)
    {
        //保存数据
        GameModel.isInGame = false;
//         if (GameModel.serverType == GameModel.ServerKind_RedPack)
//         {
//             GameService.Instance.OpenRedPack();
//         }        
        Util.Instance.DoAction(LandlordsEvent.S_GameEnd, pro);
    }

    //托管
    public void S2C_Trustee(Bs.Gameddz.S_TRUSTEE pro)
    {
        if (pro.TrusteeUser >= 0 && pro.TrusteeUser < 3)
        {
            LandlordsModel.isPlayerTrustee[pro.TrusteeUser] = (pro.Trustee == 1);
        }
        Util.Instance.DoAction(LandlordsEvent.OnUserTrustee, pro);
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, false);
    }
    #endregion
}
