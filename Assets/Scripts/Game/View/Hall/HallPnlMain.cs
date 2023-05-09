using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class HallPnlMain : View
{
    private UITexture bghall;
    private UITexture bgother;
    //top
    private Transform top;
    private Transform info;
    private Transform systemMessage;
    private UITexture sptPhoto;
    private UILabel lblNickName;
    private UILabel lblGameId;
    private UILabel lblZuanCount;
    private UILabel lblRedPackCount;
    private UILabel lblCoinCount;
    private UIButton btnBuyZuan;
    private UIButton btnBuyCoin;
    private UIButton btnGift;
    private UIButton btnActivity;
    private UIButton btnTask;
    
    private UIButton btnReturn;
    private UILabel lblSystemMsg;
    private UILabel lblGameType;
    private UIButton btnDuiHuan;
    private UIButton btnInviteFriend;
    
    //bottom
    private Transform bottom;
    private Transform hallBtnPnl;
    private UIButton btnRecord;
    private UIButton btnService;
    private UIButton btnExchange;
    private UIButton btnStore;
    private UIButton btnRule;
    private UIButton btnShare;
    private UIButton btnEmail;

    private UIButton btnSet;
    private UIButton btnRealAuth;

    private UIButton btnQuickStart;

    //center
    private GameObject center;
    private Transform btnRedPackGame;
    private Transform btnCoinGame;
    private Transform btnPrivateGame;

    private UIButton btnBaseEnsure;
    private UIButton btnMonthGift;
    //privateRoom
    private GameObject privateRoom;
    private Transform btnCreateRoom;
    private Transform btnJoinRoom;
    private Transform recordList;
    private GameObject itemRecord;
    private List<GameObject> gameRecordList = new List<GameObject>();


    private UITexture adTexture_0;
    private UITexture adTexture_1;

    //roomList
    private GameObject roomList;
    private Transform[] btnRooms = new Transform[4];

    private ushort serverType = 0;

    public override void Init()
    {
        bgother = transform.Find("bg").GetComponent<UITexture>();
        bghall = transform.Find("bghall").GetComponent<UITexture>();

        top = transform.Find("top");
        info = transform.Find("top/info");
        systemMessage = transform.Find("top/systemMessage");
        sptPhoto = transform.Find("top/info/sptPhoto").GetComponent<UITexture>();
        lblNickName = transform.Find("top/info/lblNickName").GetComponent<UILabel>();
        lblGameId = transform.Find("top/info/lblGameId").GetComponent<UILabel>();
        lblZuanCount = transform.Find("top/lblZuanCount").GetComponent<UILabel>();
        lblRedPackCount = transform.Find("top/lblRedPackCount").GetComponent<UILabel>();
        lblCoinCount = transform.Find("top/lblCoinCount").GetComponent<UILabel>();
        btnBuyZuan = transform.Find("top/lblZuanCount/btnBuyZuan").GetComponent<UIButton>();
        btnBuyCoin = transform.Find("top/lblCoinCount/btnBuyCoin").GetComponent<UIButton>();
       
        btnGift = transform.Find("top/btnGift").GetComponent<UIButton>();
        
        
        btnReturn = transform.Find("top/info/btnReturn").GetComponent<UIButton>();
        lblSystemMsg = transform.Find("top/systemMessage/Panel/Label").GetComponent<UILabel>();
        lblGameType = transform.Find("top/info/lblGameType").GetComponent<UILabel>();
        btnDuiHuan = transform.Find("top/lblRedPackCount/btnDuiHuan").GetComponent<UIButton>();
        btnInviteFriend = transform.Find("top/btnInviteFriend").GetComponent<UIButton>();

        bottom = transform.Find("bottom");
        hallBtnPnl = transform.Find("bottom/Panel");
        btnRecord = transform.Find("bottom/Panel/btnRecord").GetComponent<UIButton>();
        btnService = transform.Find("bottom/Panel/btnService").GetComponent<UIButton>();
        btnExchange = transform.Find("bottom/Panel/btnExchange").GetComponent<UIButton>();
        btnStore = transform.Find("bottom/Panel/btnStore").GetComponent<UIButton>();
        btnActivity = transform.Find("bottom/Panel/btnActivity").GetComponent<UIButton>();
        btnTask = transform.Find("bottom/Panel/btnTask").GetComponent<UIButton>();
        btnRule = transform.Find("bottom/Panel/btnRule").GetComponent<UIButton>();
        btnShare = transform.Find("bottom/Panel/btnShare").GetComponent<UIButton>();
        btnSet = transform.Find("bottom/Panel/btnSet").GetComponent<UIButton>();
        btnRealAuth = transform.Find("bottom/Panel/btnRealAuth").GetComponent<UIButton>();
        btnEmail = transform.Find("bottom/Panel/btnEmail").GetComponent<UIButton>();
        btnQuickStart = transform.Find("bottom/btnQuickStart").GetComponent<UIButton>();


        center = transform.Find("center").gameObject;
        btnRedPackGame = transform.Find("center/btnRedPackGame");
        btnCoinGame = transform.Find("center/btnCoinGame");
        btnPrivateGame = transform.Find("center/btnPrivateGame");
        adTexture_0 = transform.Find("center/AdTexture/Panel/Texture_0").GetComponent<UITexture>();
        adTexture_1 = transform.Find("center/AdTexture/Panel/Texture_1").GetComponent<UITexture>();

        btnBaseEnsure = transform.Find("center/btnBaseEnsure").GetComponent<UIButton>();
        btnMonthGift = transform.Find("center/btnMonthGift").GetComponent<UIButton>();

        privateRoom = transform.Find("privateRoom").gameObject;
        btnCreateRoom = transform.Find("privateRoom/btnCreateRoom");
        btnJoinRoom = transform.Find("privateRoom/btnJoinRoom");
        recordList = transform.Find("privateRoom/recordList");
        itemRecord = transform.Find("privateRoom/recordList/Scroll View/itemRecord").gameObject;

        roomList = transform.Find("roomList").gameObject;
        for (int i = 0; i < 4; i++)
        {
            btnRooms[i] = transform.Find("roomList/Panel/btnRoom_" + i);
        }
        
        EventDelegate.Add(sptPhoto.GetComponent<UIButton>().onClick, OnBtnPhotoClick);
        EventDelegate.Add(btnBuyZuan.onClick, OnBtnBuyZuanClick);
        EventDelegate.Add(btnBuyCoin.onClick, OnBtnBuyCoinClick);
        EventDelegate.Add(btnExchange.onClick, OnBtnExchangeClick);
        EventDelegate.Add(btnGift.onClick, OnBtnGiftClick);
        EventDelegate.Add(btnActivity.onClick, OnBtnActivityClick);
        EventDelegate.Add(btnTask.onClick, OnBtnTaskClick);
        EventDelegate.Add(btnReturn.onClick, OnBtnReturnClick);
        EventDelegate.Add(btnDuiHuan.onClick, OnBtnExchangeClick);
        EventDelegate.Add(btnInviteFriend.onClick, OnBtnInviteFriendClick);

        EventDelegate.Add(btnRecord.onClick, OnBtnRecordClick);
        EventDelegate.Add(btnService.onClick, OnBtnServiceClick);
        EventDelegate.Add(btnStore.onClick, OnBtnStoreClick);

        EventDelegate.Add(btnSet.onClick, OnBtnSetClick);
        EventDelegate.Add(btnEmail.onClick, OnBtnEmailClick);
        EventDelegate.Add(btnShare.onClick, OnBtnShareClick);
        EventDelegate.Add(btnRule.onClick, OnBtnRuleClick);
        EventDelegate.Add(btnRealAuth.onClick, OnBtnRealAuthClick);
        EventDelegate.Add(btnBaseEnsure.onClick, OnBtnBaseEnsureClick);
        EventDelegate.Add(btnMonthGift.onClick, OnBtnMonthGiftClick);
        EventDelegate.Add(btnQuickStart.onClick, OnBtnQuckStartClick);

        EventDelegate.Add(btnCreateRoom.GetComponent<UIButton>().onClick, OnBtnCreateRoomClick);
        EventDelegate.Add(btnJoinRoom.GetComponent<UIButton>().onClick, OnBtnJoinRoomClick);
        EventDelegate.Add(btnRedPackGame.GetComponent<UIButton>().onClick, OnBtnRedPackGameClick);
        EventDelegate.Add(btnCoinGame.GetComponent<UIButton>().onClick, OnBtnCoinGameClick);
        EventDelegate.Add(btnPrivateGame.GetComponent<UIButton>().onClick, OnBtnPrivateGameClick);

        UIEventListener.Get(adTexture_0.gameObject).onDragStart = OnDragStart;
        UIEventListener.Get(adTexture_0.gameObject).onDragEnd = OnDragEnd;
        UIEventListener.Get(adTexture_1.gameObject).onDragStart = OnDragStart;
        UIEventListener.Get(adTexture_1.gameObject).onDragEnd = OnDragEnd;
        for (int i = 0; i < 4; i++)
        {
            UIEventListener.Get(btnRooms[i].gameObject).onClick = OnBtnRoomClick;
        }

        gameObject.SetActive(false);
        itemRecord.SetActive(false);
        top.localPosition = new Vector3(0f, 485f, 0f);
        bottom.localPosition = new Vector3(0f, -430f, 0f);

        center.SetActive(true);
        center.GetComponent<TweenAlpha>().ResetToBeginning();

        privateRoom.GetComponent<TweenAlpha>().ResetToBeginning();
        recordList.GetComponent<TweenPosition>().ResetToBeginning();
        btnCreateRoom.GetComponent<TweenPosition>().ResetToBeginning();
        btnJoinRoom.GetComponent<TweenPosition>().ResetToBeginning();

        //初始化玩家游戏记录列表
        gameRecordList.Clear();
        for (int i = 0; i < 30; i++)
        {
            GameObject obj = PoolManager.Instance.Spawn(itemRecord);
            obj.transform.parent = itemRecord.transform.parent;
            obj.name = "itemRecord_" + i;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(0f, 114f - 82f * gameRecordList.Count, 0f);
            gameRecordList.Add(obj);
        }
        //自适应
        hallBtnPnl.GetComponent<UIGrid>().cellWidth = 140f * (AppConfig.screenWidth / 1280f);
        roomList.transform.Find("Panel").GetComponent<UIGrid>().cellWidth = 300f * (AppConfig.screenWidth / 1280f);
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenPnlMain += Open;
        HallEvent.V_ClosePnlMain += Close;
        GameEvent.V_RefreshUserInfo += RefreshUserInfo;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenPnlMain -= Open;
        HallEvent.V_ClosePnlMain -= Close;
        GameEvent.V_RefreshUserInfo -= RefreshUserInfo;
    }

    void Open()
    {
        //广告图
        if (HallModel.adTextureList.Count == 0)
        {
            HallModel.adTextureList.Add(Resources.Load<Texture>("texture/adTexture"));
        }

        gameObject.SetActive(true);
        OpenTop();
        OpenBottom();
        OpenCenter();

        btnReturn.gameObject.SetActive(false);
        info.DOLocalMoveX(-AppConfig.screenWidth/2 + 150f, 0.3f);
        RefreshUserInfo(true);
        StartCoroutine(PlayerSystemMessage());
    }

    void Close()
    {
        CloseTop();
        CloseBottom();
        if (center.activeSelf)
        {
            CloseCenter();
        }
        if (gameObject.activeSelf)
        {
            StopCoroutine(PlayerSystemMessage());
            DoActionDelay(CloseCor, 0.4f);
        }
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }




    void OpenTop()
    {
        top.DOLocalMoveY(315f, 0.3f);
    }

    void CloseTop()
    {
        top.DOLocalMoveY(485f, 0.3f);
    }

    void OpenBottom()
    {
        bottom.DOLocalMoveY(-310f, 0.3f);
    }

    void CloseBottom()
    {
        bottom.DOLocalMoveY(-430f, 0.5f);
    }

    void OpenCenter()
    {
        DoAction(HallEvent.V_OpenDlgRank);

        center.SetActive(true);
        roomList.SetActive(false);
        privateRoom.SetActive(false);
        btnBaseEnsure.gameObject.SetActive(true);
        btnMonthGift.gameObject.SetActive(true);

        center.transform.localPosition = new Vector3(120f, 0f, 0f);
        center.transform.DOLocalMoveX(0f, 0.3f);
        center.GetComponent<TweenAlpha>().PlayForward();

        if (HallModel.adTextureList.Count > 0)
        {
            adTexture_0.mainTexture = HallModel.adTextureList[0];
            adTexture_1.mainTexture = HallModel.adTextureList[0];
        }
        CancelInvoke("SwitchAdTextureLeft");
        Invoke("SwitchAdTextureLeft", 3f);
    }

    void CloseCenter()
    {
        btnBaseEnsure.gameObject.SetActive(false);
        btnMonthGift.gameObject.SetActive(false);
        center.transform.DOLocalMoveX(120f, 0.3f);
        center.GetComponent<TweenAlpha>().PlayReverse();
        DoAction(HallEvent.V_CloseDlgRank);

        CancelInvoke("SwitchAdTextureLeft");
    }

    void OpenRoomList()
    {
        //背景切换
        hallBtnPnl.gameObject.SetActive(false);
        btnQuickStart.gameObject.SetActive(true);
        systemMessage.gameObject.SetActive(false);
        
        center.SetActive(false);
        roomList.SetActive(true);
        privateRoom.SetActive(false);
        roomList.transform.Find("Panel").GetComponent<UIScrollView>().ResetPosition();
        //数据保存
        Dictionary<System.UInt32, bs.types.RoomInfo> serverDic = new Dictionary<System.UInt32, bs.types.RoomInfo>();
        foreach(bs.types.RoomInfo server in HallModel.roomList.Values)
        {
            if (server.kind == (ushort)HallModel.currentGameKindId && server.type == bs.types.RoomInfo.RoomType.Gold)
            {
                if (!serverDic.ContainsKey(server.level))
                {
                    serverDic.Add(server.level, server);
                }
            }
        }
        //界面初始化
        for (System.UInt32 i = 0; i < 4; i++)
        {
            if (serverDic.ContainsKey(i))
            {
                btnRooms[i].gameObject.SetActive(true);
                btnRooms[i].transform.Find("lblBaseScore").GetComponent<UILabel>().text = "底分：" + serverDic[i].base_score;
                btnRooms[i].transform.Find("lblLimit").GetComponent<UILabel>().text = "最低进入" + serverDic[i].join_min + "金币";
                //btnRooms[i].GetComponent<TweenScale>().ResetToBeginning();
                btnRooms[i].GetComponent<TweenScale>().PlayForward();
                btnRooms[i].GetComponent<TweenAlpha>().PlayForward();
            }
            else
            {
                btnRooms[i].gameObject.SetActive(false);
            }
        }
        int quickIndex = -1;
        for (int i = 3; i >= 0; i--)
        {
            if (serverDic.ContainsKey((System.UInt32)i))
            {
                if (quickIndex == -1 && HallModel.userCoinInGame > serverDic[(System.UInt32)i].join_min)
                {
                    quickIndex = i;
                }
            }
        }

        //快速开始房间
        string roomName = "金币不足";
        if(quickIndex != -1)
        {
            if(quickIndex == 0) roomName="初级场";
            else if(quickIndex == 1) roomName="中级场";
            else if(quickIndex == 2) roomName="高级场";
            else if(quickIndex == 3) roomName="大师场"; 
        }
        btnQuickStart.transform.Find("lblRoomName").GetComponent<UILabel>().text = roomName;
    }

    void CloseRoomList()
    {
        hallBtnPnl.gameObject.SetActive(true);
        btnQuickStart.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            if (btnRooms[i].gameObject.activeSelf)
            {
                btnRooms[i].GetComponent<TweenScale>().PlayReverse();
                btnRooms[i].GetComponent<TweenAlpha>().PlayForward();
            }
        }
    }

    void OpenPrivateRoom()
    {
        systemMessage.gameObject.SetActive(false);
        center.SetActive(false);
        roomList.SetActive(false);
        privateRoom.SetActive(true);

        privateRoom.GetComponent<TweenAlpha>().PlayForward();
        btnCreateRoom.GetComponent<TweenPosition>().PlayForward();
        btnJoinRoom.GetComponent<TweenPosition>().PlayForward();
        recordList.GetComponent<TweenPosition>().PlayForward();

        RefreshGameRecord();
    }

    void ClosePrivateRoom()
    {
        privateRoom.GetComponent<TweenAlpha>().PlayReverse();
        btnCreateRoom.GetComponent<TweenPosition>().PlayReverse();
        btnJoinRoom.GetComponent<TweenPosition>().PlayReverse();
        recordList.GetComponent<TweenPosition>().PlayReverse();
    }

    //刷新玩家游戏记录
    void RefreshGameRecord()
    {
        //记录排序
        List<CMD_Hall_S_GameRecord> list = new List<CMD_Hall_S_GameRecord>();
        foreach (CMD_Hall_S_GameRecord info in HallModel.gameRecordList.Values)
        {
            if (info.wKindID == AppConfig.gameDic[GameFlag.Landlords3].kindId && list.Count < 30)
            {
                list.Add(info);
            }
        }
        list.Sort
            (
                    delegate(CMD_Hall_S_GameRecord record1, CMD_Hall_S_GameRecord record2)
                    {
                        return -record1.InsertTime.CompareTo(record2.InsertTime);//升序
                    }
            );
        //刷新页面
        for (int i = 0; i < 30; i++)
        {
            if (i < list.Count)
            {
                gameRecordList[i].SetActive(true);
                gameRecordList[i].transform.Find("lblRoomId").GetComponent<UILabel>().text = "房间ID：" + list[i].dwRoomNumber + "（" + list[i].dwGameCount + "局）";
                gameRecordList[i].transform.Find("lblDateTime").GetComponent<UILabel>().text = list[i].InsertTime.ToString();
                for (int j = 0; j < 3; j++)
                {
                    gameRecordList[i].transform.Find("user_" + j).gameObject.SetActive(true);
                    gameRecordList[i].transform.Find("user_" + j + "/lblUserName").GetComponent<UILabel>().text = list[i].szUserNickName[j];
                    gameRecordList[i].transform.Find("user_" + j + "/photo").GetComponent<UITexture>().mainTexture = GameModel.GetUserPhotoByUserId(list[i].dwUserID[j]);
                    if (list[i].lUserScore[j] > 0)
                    {
                        gameRecordList[i].transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().color = new Color(255f / 255, 66f / 255, 66f / 255);
                        gameRecordList[i].transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().text = "+" + list[i].lUserScore[j].ToString();
                    }
                    else
                    {
                        gameRecordList[i].transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().color = new Color(77f / 255, 153f / 255, 203f / 255);
                        gameRecordList[i].transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().text = list[i].lUserScore[j].ToString();
                    }
                }
            }
            else
            {
                gameRecordList[i].SetActive(false);
            }
        }
    }

    void RefreshUserInfo(bool isRefreshUserInfo)
    {
        lblNickName.text = HallModel.userName;
        lblGameId.text = "ID : " + HallModel.gameId.ToString();
        lblZuanCount.text = HallModel.userDiamondCount.ToString("N0");
        lblRedPackCount.text = HallModel.userRedPackCount.ToString();
        lblCoinCount.text = HallModel.userCoinInGame.ToString("N0");

        if (HallModel.userPhotos.ContainsKey(HallModel.userId))
        {
            sptPhoto.mainTexture = HallModel.userPhotos[HallModel.userId];
        }
        else
        {
            sptPhoto.mainTexture = HallModel.defaultPhoto;
        }
    }


    IEnumerator PlayerSystemMessage()
    {
        string message = "";
        if (HallModel.messageList.Count > 0)
        {
            message = HallModel.messageList[0];
            HallModel.messageList.RemoveAt(0);
        }
        else
        {
            message = HallModel.defaultMessage;
        }
        yield return new WaitForEndOfFrame();
        lblSystemMsg.text = message;
        float dis = 420f + lblSystemMsg.localSize.x;
        float timer = dis / 100f;
        lblSystemMsg.transform.localPosition = new Vector3(dis/2, 0f, 0f);
        lblSystemMsg.transform.DOLocalMoveX(-dis/2, timer).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timer);
        StartCoroutine(PlayerSystemMessage());
    }


    #region 广告图

    private int adIndex = 0;
    private float adDragStartPosX;
    private float adDragEndPosX;

    void OnDragStart(GameObject obj)
    {
        CancelInvoke("SwitchAdTextureLeft");
        adDragStartPosX = Input.mousePosition.x;
    }

    void OnDragEnd(GameObject obj)
    {
        adDragEndPosX = Input.mousePosition.x;
        if (adDragEndPosX - adDragStartPosX > 20)
        {
            SwitchTextureRight();
        }
        else if (adDragEndPosX - adDragStartPosX < -20)
        {
            SwitchAdTextureLeft();
        }
        else
        {
            Invoke("SwitchAdTextureLeft", 2f);
        }
    }

    void SwitchAdTextureLeft()
    {
        if (HallModel.adTextureList.Count == 0)
        {
            return;
        }
        //复位
        if (adTexture_0.transform.localPosition.x > 315f)
        {
            adTexture_0.transform.localPosition = new Vector3(-320f, 0f, 0f);
            SwitchAdTextureLeft();
            return;
        }
        if (adTexture_1.transform.localPosition.x > 315f)
        {
            adTexture_1.transform.localPosition = new Vector3(-320f, 0f, 0f);
            SwitchAdTextureLeft();
            return;
        }
        //动画
        adIndex++;
        if (adIndex >= HallModel.adTextureList.Count)
        {
            adIndex = 0;
        }
        if (adTexture_0.transform.localPosition.x > adTexture_1.transform.localPosition.x)
        {
            adTexture_1.transform.localPosition = new Vector3(320f, 3f, 0f);
            adTexture_1.mainTexture = HallModel.adTextureList[adIndex];
            adTexture_0.transform.DOLocalMoveX(-320f, 0.5f);
            adTexture_1.transform.DOLocalMoveX(0f, 0.5f);
        }
        else
        {
            adTexture_0.transform.localPosition = new Vector3(320f, 3f, 0f);
            adTexture_0.mainTexture = HallModel.adTextureList[adIndex];
            adTexture_1.transform.DOLocalMoveX(-320f, 0.5f);
            adTexture_0.transform.DOLocalMoveX(0f, 0.5f);
        }
       
        CancelInvoke("SwitchAdTextureLeft");
        Invoke("SwitchAdTextureLeft", 5f);
    }

    void SwitchTextureRight()
    {
        if (HallModel.adTextureList.Count == 0)
        {
            return;
        }
        //复位
        if (adTexture_0.transform.localPosition.x < -315f)
        {
            adTexture_0.transform.localPosition = new Vector3(320f, 0f, 0f);
            SwitchTextureRight();
            return;
        }
        if (adTexture_1.transform.localPosition.x < -315f)
        {
            adTexture_1.transform.localPosition = new Vector3(320f, 0f, 0f);
            SwitchTextureRight();
            return;
        }
        //动画
        adIndex--;
        if (adIndex < 0)
        {
            adIndex = HallModel.adTextureList.Count - 1;
        }
        if (adTexture_0.transform.localPosition.x > adTexture_1.transform.localPosition.x)
        {
            adTexture_0.transform.localPosition = new Vector3(-320f, 3f, 0f);
            adTexture_0.mainTexture = HallModel.adTextureList[adIndex];
            adTexture_0.transform.DOLocalMoveX(0f, 0.5f);
            adTexture_1.transform.DOLocalMoveX(320f, 0.5f);
        }
        else
        {
            adTexture_1.transform.localPosition = new Vector3(-320f, 3f, 0f);
            adTexture_1.mainTexture = HallModel.adTextureList[adIndex];
            adTexture_1.transform.DOLocalMoveX(0f, 0.5f);
            adTexture_0.transform.DOLocalMoveX(320f, 0.5f);
        }
        
        CancelInvoke("SwitchAdTextureLeft");
        Invoke("SwitchAdTextureLeft", 5f);
    }

    #endregion


    #region 菜单

    //个人中心
    void OnBtnPhotoClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgUserInfo);
    }

    //购买钻石
    void OnBtnBuyZuanClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //首充判断
        if (HallModel.isFirstCharge == false)
        {
            DoAction(GameEvent.V_OpenDlgFirstCharge, delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.DiamondPage); });

        }
        else
        {
            DoAction(HallEvent.V_OpenDlgStore, DlgStoreArg.DiamondPage);
        }
    }

    void OnBtnBuyCoinClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //首充判断
        if (HallModel.isFirstCharge == false)
        {
            DoAction(GameEvent.V_OpenDlgFirstCharge, delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.CoinPage); });

        }
        else
        {
            DoAction(HallEvent.V_OpenDlgStore, DlgStoreArg.CoinPage);
        }
    }

    //兑换
    void OnBtnExchangeClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgExchange);
    }

    //福利
    void OnBtnGiftClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgGift);
    }

    //邀请好友
    void OnBtnInviteFriendClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgShareTask);
    }

    //活动
    void OnBtnActivityClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgActivity);
    }

    //任务
    void OnBtnTaskClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(GameEvent.V_OpenDlgTask);
    }

    void OnBtnReturnClick()
    {
        sptPhoto.gameObject.SetActive(true);
        lblNickName.gameObject.SetActive(true);
        lblGameId.gameObject.SetActive(true);
        lblGameType.gameObject.SetActive(false);
        bghall.depth = 1;
        bgother.depth = 0;
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        if (serverType == GameModel.ServerKind_Private)
        {
            ClosePrivateRoom();
        }
        else if (serverType == GameModel.ServerKind_Gold)
        {
            CloseRoomList();
        }
        CloseBottom();
        systemMessage.gameObject.SetActive(true);
        info.DOLocalMoveX(-AppConfig.screenWidth/2 + 150f, 0.3f);
        DoActionDelay(OpenBottom, 0.3f);
        DoActionDelay(OpenCenter, 0.3f);
    }

    //游戏记录
    void OnBtnRecordClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgRecord);
    }

    //客服
    void OnBtnServiceClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgService);
    }

    //排行
    void OnBtnRankClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgRank);
    }

    //规则
    void OnBtnRuleClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgRule);
    }

    //商城
    void OnBtnStoreClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //首充判断
        if (HallModel.isFirstCharge == false)
        {
            DoAction(GameEvent.V_OpenDlgFirstCharge,delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.DiamondPage); });
        }
        else
        {
            DoAction(HallEvent.V_OpenDlgStore, DlgStoreArg.DiamondPage);
        }
        
    }

    //设置
    void OnBtnSetClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(GameEvent.V_OpenDlgSet);
    }

    //邮件
    void OnBtnEmailClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgEmail);
    }

    //分享
    void OnBtnShareClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgShare);
        //DoAction(HallEvent.V_OpenDlgWheelSignDay);
    }

    //实名认证
    void OnBtnRealAuthClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgRealAuth);
    }
    
    //低保
    void OnBtnBaseEnsureClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(HallEvent.V_OpenDlgBaseEnsureRule);
    }

    //周卡月卡
    void OnBtnMonthGiftClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //int opentype =  0;
        //if (HallModel.isBuyMonthCard == true) opentype = 4;
        Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, 4);
    }

        //快速开始
        void OnBtnQuckStartClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //数据保存
        Dictionary<int, GameServerInfo> serverDic = new Dictionary<int, GameServerInfo>();
        foreach(GameServerInfo server in HallModel.serverList.Values)
        {
            if (server.wKindID == (ushort)HallModel.currentGameKindId && server.wServerType == GameModel.ServerKind_Gold)
            {
                if (!serverDic.ContainsKey(server.wServerLevel))
                {
                    serverDic.Add((int)server.wServerLevel, server);
                }
            }
        }
        //房间选择
        bool bSuccess = false;
        for (int i = 3; i >= 0; i--)
        {
            if (serverDic.ContainsKey(i) && HallModel.userCoinInGame > serverDic[i].lEnterScore)
            {
                OnBtnRoomClick(btnRooms[i].gameObject);
                bSuccess = true;
                break;
            }
        }
        if(bSuccess == false)
        {
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke("抱歉，您的金币不足，请充值", "", delegate { Util.Instance.DoAction(HallEvent.V_OpenDlgStore, DlgStoreArg.CoinPage); }, null);
            }
        }
    }

    #endregion

    #region 房卡操作


    void OnBtnCreateRoomClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        DoAction(HallEvent.V_OpenDlgCreateRoom);
    }

    void OnBtnJoinRoomClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        DoAction(HallEvent.V_OpenDlgJoinRoom);
    }

    //红包场直接进入游戏
    void OnBtnRedPackGameClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        HallModel.currentGameFlag = GameFlag.Landlords3;
        serverType = GameModel.ServerKind_RedPack;

        HallModel.opOnLoginGame = OpOnLginGame.GetChair;
        GameModel.currentRoomId = 0xFFFFFFFF;
        HallService.Instance.GetRoomServerInfo(GameModel.ServerKind_RedPack, 1);
    }

    void OnBtnCoinGameClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        serverType = GameModel.ServerKind_Gold;
        CloseCenter();
        CloseBottom();
        //背景切换
        bghall.depth = 0;
        bgother.depth = 1;

        //头像信息切换
        sptPhoto.gameObject.SetActive(false);
        lblNickName.gameObject.SetActive(false);
        lblGameId.gameObject.SetActive(false);
        lblGameType.text="金币场";
        lblGameType.gameObject.SetActive(true);

        DoActionDelay(OpenRoomList, 0.3f);
        DoActionDelay(OpenBottom, 0.3f);

        btnReturn.gameObject.SetActive(true);
        info.DOLocalMoveX(-AppConfig.screenWidth/2 + 220f, 0.3f);
    }

    void OnBtnPrivateGameClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        serverType = GameModel.ServerKind_Private;
        CloseBottom();
        CloseCenter();

        //背景切换
        bghall.depth = 0;
        bgother.depth = 1;
        
        //头像信息切换
        sptPhoto.gameObject.SetActive(false);
        lblNickName.gameObject.SetActive(false);
        lblGameId.gameObject.SetActive(false);
        lblGameType.text="好友场";
        lblGameType.gameObject.SetActive(true);

        DoActionDelay(OpenPrivateRoom, 0.3f);
        DoActionDelay(OpenBottom, 0.3f);

        btnReturn.gameObject.SetActive(true);
        info.DOLocalMoveX(-AppConfig.screenWidth/2 + 220f, 0.3f);
    }

    //点击房间
    void OnBtnRoomClick(GameObject obj)
    {
        ushort level = ushort.Parse(obj.name.Split('_')[1]);

        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        HallModel.currentGameFlag = GameFlag.Landlords3;

        HallModel.opOnLoginGame = OpOnLginGame.GetChair;
        GameModel.currentRoomId = 0xFFFFFFFF;
        HallService.Instance.GetRoomServerInfo(GameModel.ServerKind_Gold, level);
    }

    #endregion

}
