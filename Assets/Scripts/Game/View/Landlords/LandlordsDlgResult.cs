using UnityEngine;
using System.Collections;

public class LandlordsDlgResult : View
{
    private GameObject gameResult;
    private GameObject title_win;
    private GameObject title_lost;
    private Transform[] users_game = new Transform[LandlordsModel.GAME_NUM];
    private UIButton btnContinue_game, btnReturnToHall, btnCloseResult, btnContinue;

    private GameObject gameCloseAccount;
    private UIButton btnContinue_CloseAccount, btnCheckDetail, btnReturnHall_CloseAccount;

    private GameObject totalResult;
    private Transform[] users_total = new Transform[LandlordsModel.GAME_NUM];
    private UIButton btnReturnToHall_total;
    private UIButton btnShareFriend_total;
    private UIButton btnShareCircle_total;

    private GameObject shade;


    private Landlords3Result res = new Landlords3Result();
    private bool isRoomOver = false;
    private int shareType = 1;              //分享类型（好友/朋友圈）

    public override void Init()
    {
        gameResult = transform.Find("gameResult").gameObject;
        title_win = transform.Find("gameResult/title_win").gameObject;
        title_lost = transform.Find("gameResult/title_lost").gameObject;
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            users_game[i] = transform.Find("gameResult/pnlUser/user_" + i);
        }
        btnContinue_game = transform.Find("gameResult/btnContinue").GetComponent<UIButton>();
        btnReturnToHall = transform.Find("gameResult/btnReturnToHall").GetComponent<UIButton>();
        btnCloseResult = transform.Find("gameResult/btnCloseResult").GetComponent<UIButton>();
        btnContinue = transform.Find("btnContinue").GetComponent<UIButton>();

        gameCloseAccount = transform.Find("gameCloseAccount").gameObject;
        btnContinue_CloseAccount = transform.Find("gameCloseAccount/btnContinue").GetComponent<UIButton>();
        btnCheckDetail = transform.Find("gameCloseAccount/btnCheckDetail").GetComponent<UIButton>();
        btnReturnHall_CloseAccount = transform.Find("gameCloseAccount/btnReturnHall").GetComponent<UIButton>();

        totalResult = transform.Find("totalResult").gameObject;
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            users_total[i] = transform.Find("totalResult/user_" + i);
        }
        btnReturnToHall_total = transform.Find("totalResult/btnReturnToHall").GetComponent<UIButton>();
        btnShareFriend_total = transform.Find("totalResult/btnShareFriend").GetComponent<UIButton>();
        btnShareCircle_total = transform.Find("totalResult/btnShareCircle").GetComponent<UIButton>();

        shade = transform.Find("shade").gameObject;


        EventDelegate.Add(btnReturnToHall.onClick, OnBtnReturnToHallClick);
        EventDelegate.Add(btnReturnToHall_total.onClick, OnBtnReturnToHallClick);
        EventDelegate.Add(btnContinue_game.onClick, OnBtnContinueClick);
        EventDelegate.Add(btnCloseResult.onClick, OnBtnCloseResultClick);

        EventDelegate.Add(btnContinue_CloseAccount.onClick, OnBtnContinueClick);
        EventDelegate.Add(btnCheckDetail.onClick, OnBtnCheckDetailClick);
        EventDelegate.Add(btnReturnHall_CloseAccount.onClick, OnBtnReturnToHallClick);

        EventDelegate.Add(btnContinue.onClick, OnBtnContinueClick);         
        EventDelegate.Add(btnShareFriend_total.onClick, OnBtnShareFriendClick);
        EventDelegate.Add(btnShareCircle_total.onClick, OnBtnShareCircleClick);

        gameObject.SetActive(false);
        gameResult.SetActive(false);
        gameCloseAccount.SetActive(false);
        totalResult.SetActive(false);
        btnContinue.gameObject.SetActive(false);
        shade.SetActive(false);
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            users_total[i].gameObject.SetActive(false);
        }

        totalResult.GetComponent<TweenScale>().ResetToBeginning();
        totalResult.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
        isRoomOver = false;
        btnContinue_game.transform.localPosition = new Vector3(131f, -295, 0);
    }

    public override void RegisterAction()
    {
        GameEvent.S_OnDisRoom += OnDisRoom;
        GameEvent.S_GetTotalScore += OnGetTotalScore;
        LandlordsEvent.V_GameOver += OnGameOver;
        LandlordsEvent.V_CloseDlgResult += Close;
    }

    public override void RemoveAction()
    {
        GameEvent.S_OnDisRoom -= OnDisRoom;
        GameEvent.S_GetTotalScore -= OnGetTotalScore;
        LandlordsEvent.V_GameOver -= OnGameOver;
        LandlordsEvent.V_CloseDlgResult -= Close;
    }


    //房间中途解散
    void OnDisRoom()
    {
        if (this == null) { return; }
        isRoomOver = true;
        //中途解散，弹出总结算面板
        if (gameResult)
        {
            if (GameModel.serverType == GameModel.ServerKind_Private && !gameCloseAccount.activeSelf && !IsInvoking("OpenGameCloseAccount"))
            {
                OpenTotalResult();
            }
        }        
    }

    //收到总得分信息
    void OnGetTotalScore(CMD_Game_S_TotalScore pro)
    {
        for (int i = 0; i < 3; i++)
        {
            res.winCount[i] = pro.cbWinConut[i];
            res.totalScore[i] = pro.totalScore[i];
        }
        isRoomOver = true;
    }

    void OnGameOver(Landlords3Result pro, float delay)
    {
        //保存数据
        res.chairId = pro.chairId;
        res.isBankerId = pro.isBankerId;
        res.gameId = pro.gameId;
        res.userName = pro.userName;
        res.userPhotos = pro.userPhotos;

        res.userScore = pro.userScore;

        //打开游戏结算面板
        Invoke("OpenGameCloseAccount", delay);
    }

    void Close()
    {
        if (gameResult.activeSelf)
        {
            CloseGameResult();
        }
        if (gameCloseAccount.activeSelf)
        {
            CloseGameCloseAccount();
        }
        if (totalResult.activeSelf)
        {
            CloseTotalResult();
        }
        if (gameObject.activeSelf)
        {
            DoActionDelay(CloseCor, 0.4f);
        }
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
        gameResult.SetActive(false);
        totalResult.SetActive(false);
        shade.SetActive(false);
    }

    //打开游戏结算面板
    void OpenGameResult()
    {
        shade.SetActive(true);
        shade.GetComponent<TweenAlpha>().PlayForward();
        btnReturnToHall.gameObject.SetActive(GameModel.serverType != GameModel.ServerKind_Private);
        if (GameModel.serverType == GameModel.ServerKind_Private)
        {
            btnContinue_game.transform.localPosition = new Vector3(0f, -295, 0);
        }
        if (!GameModel.isInGame)
        {
            gameObject.SetActive(true);
            gameResult.SetActive(true);
            totalResult.SetActive(false);
        }
        //标题
        title_win.SetActive(res.userScore[res.chairId] >= 0);
        title_lost.SetActive(res.userScore[res.chairId] < 0);
        //初始化界面
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            users_game[i].Find("lblCardScore").gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_Private);
            users_game[i].Find("lblDiamondCount").gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_RedPack);
            users_game[i].Find("lblCoinScore").gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_Gold);
        }        
        //数据显示
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            users_game[i].Find("photo").GetComponent<UITexture>().mainTexture = res.userPhotos[i] == null ? HallModel.defaultPhoto : res.userPhotos[i];
            users_game[i].Find("lblUserName").GetComponent<UILabel>().text = res.userName[i];
            users_game[i].Find("lblCardScore").GetComponent<UILabel>().text = res.userScore[i].ToString();
            users_game[i].Find("lblDiamondCount").GetComponent<UILabel>().text = res.userScore[i].ToString();
            users_game[i].Find("lblCoinScore").GetComponent<UILabel>().text = res.userScore[i].ToString();
            users_game[i].Find("flagHog").gameObject.SetActive(res.isBankerId[i] == 1);
        }
        //关闭计时器
        DoAction(GameEvent.V_CloseTimer);
    }
    void CloseGameResult()
    {
        gameResult.SetActive(false);
    }

    //打开小结算面板
    void OpenGameCloseAccount()
    {
        shade.SetActive(true);
        shade.GetComponent<TweenAlpha>().PlayForward();
        if (!GameModel.isInGame)
        {
            gameObject.SetActive(true);
            gameCloseAccount.SetActive(true);
            gameResult.SetActive(false);
            totalResult.SetActive(false);
        }
        btnReturnHall_CloseAccount.gameObject.SetActive(GameModel.serverType != GameModel.ServerKind_Private);        
    }

    void CloseGameCloseAccount()
    {
        gameCloseAccount.SetActive(false);
        shade.SetActive(false);
    }

    //打开总结算面板
    void OpenTotalResult()
    {
        gameObject.SetActive(true);
        gameResult.SetActive(false);
        gameCloseAccount.SetActive(false);
        totalResult.SetActive(true);
        shade.SetActive(true);
        totalResult.GetComponent<TweenScale>().PlayForward();
        totalResult.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
        //信息显示
        int maxWinUser = 0;
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            users_total[i].gameObject.SetActive(true);
            if (res.totalScore[i] > res.totalScore[maxWinUser])
            {
                maxWinUser = i;
            }
            users_total[i].GetComponent<UISprite>().spriteName = "f2";
            users_total[i].Find("lblUserName").GetComponent<UILabel>().color = new Color(158 / 255f, 88 / 255f, 0);
            users_total[i].Find("lblGameId").GetComponent<UILabel>().color = new Color(158 / 255f, 88 / 255f, 0);
            users_total[i].Find("lblTotalScore").GetComponent<UILabel>().color = new Color(158 / 255f, 88 / 255f, 0);

            users_total[i].Find("photo").GetComponent<UITexture>().mainTexture = res.userPhotos[i] == null ? HallModel.defaultPhoto : res.userPhotos[i];
            users_total[i].Find("lblUserName").GetComponent<UILabel>().text = res.userName[i];
            users_total[i].Find("lblGameId").GetComponent<UILabel>().text = "ID: " + res.gameId[i].ToString();
            users_total[i].Find("lblTotalScore").GetComponent<UILabel>().text = res.totalScore[i].ToString();
        }
        //大赢家特殊化显示
        users_total[maxWinUser].GetComponent<UISprite>().spriteName = "f1";
        users_total[maxWinUser].Find("lblUserName").GetComponent<UILabel>().color = new Color(255 / 255f, 250 / 255f, 88 / 255f);
        users_total[maxWinUser].Find("lblGameId").GetComponent<UILabel>().color = new Color(255 / 255f, 250 / 255f, 88 / 255f);
        users_total[maxWinUser].Find("lblTotalScore").GetComponent<UILabel>().color = new Color(255 / 255f, 250 / 255f, 88 / 255f);
    }

    void CloseTotalResult()
    {
        totalResult.GetComponent<TweenScale>().PlayReverse();
        totalResult.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
    }

    void OnBtnReturnToHallClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        //红包场判断
        if(GameModel.serverType == GameModel.ServerKind_RedPack)
        {
            GameEvent.V_OpenLeaveGame.Invoke();
        }
        else
        {
            Close();
            GameService.Instance.ReturnToHall();
        }
    }

    void OnBtnContinueClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);

        //房间找不到用户属于异常 直接走人
        if (GameModel.playerInRoom.ContainsKey((uint)HallModel.userId) == false ||
            GameModel.playerInRoom[(uint)HallModel.userId] == null)
        {
            Close();
            GameService.Instance.ReturnToHall();
            return;
        }

        //红包场判断 钻石不足
        PlayerInRoom playerme = GameModel.playerInRoom[(uint)HallModel.userId];
        System.Int64 lUserSocre = playerme.lScore;
        if (GameModel.IsUseIngotPay() == true) lUserSocre = playerme.lIngot;
        if (GameModel.serverType == GameModel.ServerKind_RedPack && lUserSocre < HallModel.serverList[HallModel.currentServerId].lEnterScore)
        {
            //首充判断
            if(HallModel.isFirstCharge == false)
            {
                Util.Instance.DoAction(GameEvent.V_OpenDlgFirstCharge, null);
            }
            else
            {
                //打开商城
                GameEvent.V_OpenDlgTip.Invoke("钻石不足,快去商城充值吧！", "", delegate { GameEvent.V_OpenDlgStoreInGame.Invoke(DlgStoreArg.DiamondPage); }, null);
            }
            return;
        }
        btnContinue.gameObject.SetActive(false);
        DoAction(LandlordsEvent.V_ReStartNewGame);
        if (GameModel.serverType == GameModel.ServerKind_Private)
        { 
            if (isRoomOver)
            {
                CloseGameResult();
                OpenTotalResult();
            }
            else
            {
                GameService.Instance.UserAgree();
                GameModel.hogChairId = 65535;
                Close();
            }
        }
        else
        {            
            if (GameModel.deskId == 65535 || GameModel.chairId == 65535)
            {
                if (GameEvent.V_OpenDlgTip != null)
                {
                    if (GameModel.serverType == GameModel.ServerKind_Gold)
                    {
                        GameEvent.V_OpenDlgTip.Invoke("您的金币不足，无法继续游戏！", "别忘了领取救济金哦", GameService.Instance.UserStand, GameService.Instance.UserStand);
                    }
                    else if (GameModel.serverType == GameModel.ServerKind_RedPack)
                    {
                        //上面已做过判断，这里直接获取座位开始就行了
                        GameService.Instance.GetChair();

                        //GameEvent.V_OpenDlgTip.Invoke("您的钻石不足，无法继续游戏！", "别忘了领取救济金哦", GameService.Instance.UserStand, GameService.Instance.UserStand);
                    }
                }
            }
            else
            {
                GameService.Instance.UserAgree();
            }
            GameModel.hogChairId = 65535;            
            DoAction(GameEvent.V_RefreshUserInfo, true);
            Close();
        }
    }

    void OnBtnCloseResultClick()
    {
        gameResult.SetActive(false);
        btnContinue.gameObject.SetActive(true);
    }

    void OnBtnCheckDetailClick()
    {
        gameCloseAccount.SetActive(false);
        OpenGameResult();
    }

    void OnBtnShareFriendClick()
    {
        shareType = 0;
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //截图
        StartCoroutine(ScreenShotBridge.SaveScreenShot("xjmj_", "ScreenShot", true, OnScreenShotFinish));
    }

    void OnBtnShareCircleClick()
    {
        shareType = 1;
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //截图
        StartCoroutine(ScreenShotBridge.SaveScreenShot("xjmj_", "ScreenShot", true, OnScreenShotFinish));
    }

    //截图完成
    void OnScreenShotFinish(string filePath)
    {
        PluginManager.Instance.WxShareImage(shareType, filePath, null);
    }


}
