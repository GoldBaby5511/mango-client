using UnityEngine;
using System.Collections;

public class LandlordsService : MonoBehaviour 
{
    public static LandlordsService Instance;

    void Awake()
    {
        Debug.Log("AddHandler");
        Instance = this;
        HallService.Instance.client.netManager.AddHandler(new LandlordsHandler());
    }

    #region 发送
    //用户叫分
    public void C2S_CallScore(byte cbCallScore)
    {
        CMD_Landlords_C_CallScore pro = new CMD_Landlords_C_CallScore();
        pro.cbCallScore = cbCallScore;
        GameService.Instance.client.SendPro(pro);
    }

    //用户加倍
    public void C2S_AddTime(byte cbAddTimes)
    {
        CMD_Landlords_C_AddTimes pro = new CMD_Landlords_C_AddTimes();
        pro.cbAddTimes = cbAddTimes;
        GameService.Instance.client.SendPro(pro);
    }

    //用户出牌
    public void C2S_OutCard(byte cardCount, byte[] cardData)
    {
        CMD_Landlords_C_OutCard pro = new CMD_Landlords_C_OutCard();
        pro.cbCardCount = cardCount;
        pro.cbCardData = cardData;
        GameService.Instance.client.SendPro(pro);
    }

    //用户放弃
    public void C2S_PassCard()
    {
        CMD_Landlords_C_PassCard pro = new CMD_Landlords_C_PassCard();
        GameService.Instance.client.SendPro(pro);
    }

    //托管
    public void C2S_Trustee(byte bTrustee)
    {
        CMD_Landlords_C_Trustee pro = new CMD_Landlords_C_Trustee();
        pro.bTrustee = bTrustee;
        GameService.Instance.client.SendPro(pro);
    }
    #endregion

    #region 接收
    //空闲状态
    public void S2C_StateFree(CMD_Landlords_S_StatusFree pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = false;
        LandlordsModel.playerInGame = pro.cbPlayStatus;
        LandlordsModel.callTime = pro.cbTimeCallScore;
        LandlordsModel.addTime = pro.cbTimeAddTime;
        LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        LandlordsModel.outCardTime = pro.cbTimeOutCard;
        LandlordsModel.canotAfford = pro.cbTimePassCard;
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
    public void S2C_StatusCall(CMD_Landlords_S_StatusCall pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = true;
        LandlordsModel.playerInGame = pro.cbPlayStatus;
        LandlordsModel.callTime = pro.cbTimeCallScore;
        LandlordsModel.addTime = pro.cbTimeAddTime;
        LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        LandlordsModel.outCardTime = pro.cbTimeOutCard;
        LandlordsModel.canotAfford = pro.cbTimePassCard;
        //用户头像为空时，重新加载头像信息
        for (int i = 0; i < 3; i++)
        {
            LandlordsModel.isPlayerTrustee[i] = (pro.cbUserTrustee[i] == 1);
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
    public void S2C_StatusAddTime(CMD_Landlords_S_StatusAddTime pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = true;
        LandlordsModel.playerInGame = pro.cbPlayStatus;
        LandlordsModel.bankerChairdId = pro.wBankerChairId;
        LandlordsModel.callTime = pro.cbTimeCallScore;
        LandlordsModel.addTime = pro.cbTimeAddTime;
        LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        LandlordsModel.outCardTime = pro.cbTimeOutCard;
        LandlordsModel.canotAfford = pro.cbTimePassCard;
        //用户头像为空时，重新加载头像信息
        for (int i = 0; i < 3; i++)
        {
            LandlordsModel.isPlayerTrustee[i] = (pro.cbUserTrustee[i] == 1);
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
    public void S2C_StatusPlay(CMD_Landlords_S_StatusPlay pro)
    {
        LandlordsModel.InitData();
        //保存数据
        GameModel.isInGame = true;
        LandlordsModel.playerInGame = pro.cbPlayStatus;
        LandlordsModel.bankerChairdId = pro.wBankerUser;
        LandlordsModel.callTime = pro.cbTimeCallScore;
        LandlordsModel.addTime = pro.cbTimeAddTime;
        LandlordsModel.firstOutCardTime = pro.cbTimeHeadOutCard;
        LandlordsModel.outCardTime = pro.cbTimeOutCard;
        LandlordsModel.canotAfford = pro.cbTimePassCard;
        //用户头像为空时，重新加载头像信息
        for (int i = 0; i < 3; i++)
        {
            LandlordsModel.isPlayerTrustee[i] = (pro.cbUserTrustee[i] == 1);
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
    public void S2C_GameStart(CMD_Landlords_S_GameStart pro)
    {
        //保存数据
        GameModel.isInGame = true;
        Util.Instance.DoAction(GameEvent.V_CloseLeaveGame);
        LandlordsModel.playerInGame = pro.cbPlayStatus;
        Util.Instance.DoAction(GameEvent.V_ShowWaitMatch, false);
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
        Util.Instance.DoAction(LandlordsEvent.S_GameStart, pro);
        Util.Instance.DoAction(LandlordsEvent.V_CloseDlgResult);
    }

    //重新发牌
    public void S2C_ReSendCard(CMD_Landlords_S_ReSendCard pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_ReSendCard, pro); 
    }

    //机器人扑克
    public void S2C_AndroidCard(CMD_Landlords_S_AndroidCard pro)
    {
        //Debug.Log("S2C----------------------------机器人扑克");
    }

    //作弊扑克
    public void S2C_CheatCard(CMD_Landlords_S_CheatCard pro)
    {
        //Debug.Log("S2C----------------------------作弊扑克");
    }

    //用户叫分
    public void S2C_CallScore(CMD_Landlords_S_CallScore pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_UserCall, pro);
    }

    //庄家信息
    public void S2C_BankerInfo(CMD_Landlords_S_BankerInfo pro)
    {
        LandlordsModel.bankerChairdId = pro.wBankerUser;
        LandlordsModel.bankerHoleCards = pro.cbBankerCard;

        Util.Instance.DoAction(LandlordsEvent.S_BankerStartOutCard, pro);
    }

    //用户加倍
    public void S2C_AddTimes(CMD_Landlords_S_AddTimes pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_UserAddTime, pro);
    }

    //用户出牌
    public void S2C_OutCard(CMD_Landlords_S_OutCard pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_UserOutCard, pro);
    }

    //出牌错误
    public void S2C_OutCardFail(CMD_Landlords_S_OutCardFail pro)
    {  
        Util.Instance.DoAction(LandlordsEvent.S_OutCardFail, pro);
    }

    //放弃出牌
    public void S2C_PassCard(CMD_Landlords_S_PassCard pro)
    {
        Util.Instance.DoAction(LandlordsEvent.S_GiveUpOutCard, pro);
    }

    //游戏结束
    public void S2C_GameEnd(CMD_Landlords_S_GameEnd pro)
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
    public void S2C_Trustee(CMD_Landlords_S_Trustee pro)
    {
        if (pro.wTrusteeUser >= 0 && pro.wTrusteeUser < 3)
        {
            LandlordsModel.isPlayerTrustee[pro.wTrusteeUser] = (pro.bTrustee == 1);
        }
        Util.Instance.DoAction(LandlordsEvent.OnUserTrustee, pro);
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, false);
    }
    #endregion
}
