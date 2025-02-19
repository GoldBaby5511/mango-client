using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class LandlordsPnlGame : View
{
    #region 变量声明
    private UIButton btnStart;
    private UIButton btnAgree;
    private UIButton btnChangeDesk;
    private UIButton btnJoinFriend;
    private UIButton btnCopyRoomId;
    private UIButton btnTrustee, btnCancelSelect, btnAuto;

    private UIButton btnOutCard, btnNotOutCard, btnCardTip, btnCallLandlord, btnGradLandlord, btnNotCall, btnNotHog, btnCannotAfford, btnDouble, btnNotDouble;

    private Transform[] userPos = new Transform[3];
    private GameObject[] agreeFlags = new GameObject[3];
    private GameObject[] notCallFlags = new GameObject[3];
    private GameObject[] notHogFlags = new GameObject[3];
    private GameObject[] notDoubleFlags = new GameObject[3];
    private GameObject[] doubleFlags = new GameObject[3];
    private GameObject[] hogFlags = new GameObject[3];
    private GameObject[] gradLandlordFlags = new GameObject[3];
    private GameObject[] notOutCardFlags = new GameObject[3];
    private GameObject[] landlordFlags = new GameObject[3];
    private GameObject[] autoFlags = new GameObject[3];
    private GameObject[] callPoliceFlags = new GameObject[3];
    private GameObject[] zhaDanFlags = new GameObject[3];
    private GameObject shunZiFlag, lianDuiFlag, planeFlag, wangZhaFlag, chunTianFlag, fanChunTianFlag;

    private GameObject[] timeCount = new GameObject[3];
    private int currentTimer = 0;
    private UnityAction timeOutCallBack = null;

    private Transform myHandCardParent, landlordPokerFlag;
    private UISprite[] myHandCards = new UISprite[LandlordsModel.MAX_COUNT];
    private GameObject[] userCardCount = new GameObject[3];
    private UISprite[] bankerHoleCards = new UISprite[3];
    private Transform[] outCardParent = new Transform[3];
    private GameObject[,] outCards = new GameObject[3, 20];
    private GameObject outCardItem;
    private GameObject winFlag, lostFlag;
    private Transform landlordAnim;

    private UILabel bottomScore, currentTotalScore;
    private UILabel[] lblWinScore = new UILabel[3];
    private UILabel[] lblLoseScore = new UILabel[3];


    private List<byte> handCardList = new List<byte>(); //手牌列表
    private List<byte> selectCardList = new List<byte>();   //自己选择的牌
    private List<byte> beforeUserOutCardList = new List<byte>();   //上家出的牌
    private List<byte[]> tipCardList = new List<byte[]>();//提示出牌的数据

    private int selectIndex = 0;   //提示选择下标
    #endregion

    #region 初始化
    public override void Init()
    {
        btnStart = transform.Find("btnStart").GetComponent<UIButton>();
        btnAgree = transform.Find("btnAgree").GetComponent<UIButton>();
        btnChangeDesk = transform.Find("btnChangeDesk").GetComponent<UIButton>();
        btnJoinFriend = transform.Find("btnInviteFriends").GetComponent<UIButton>();
        btnCopyRoomId = transform.Find("btnCopyRoomId").GetComponent<UIButton>();
        btnTrustee = transform.Find("btnTrustee").GetComponent<UIButton>();
        btnCancelSelect = transform.Find("btnCancelSelect").GetComponent<UIButton>();
        btnAuto = transform.Find("btnAuto").GetComponent<UIButton>();

        btnOutCard = transform.Find("btnOutCard").GetComponent<UIButton>();
        btnNotOutCard = transform.Find("btnNotOut").GetComponent<UIButton>();
        btnCardTip = transform.Find("btnTip").GetComponent<UIButton>();
        btnCallLandlord = transform.Find("btnCallLandlord").GetComponent<UIButton>();
        btnGradLandlord = transform.Find("btnGrabLandlord").GetComponent<UIButton>();
        btnNotCall = transform.Find("btnNotCall").GetComponent<UIButton>();
        btnNotHog = transform.Find("btnNotHog").GetComponent<UIButton>();
        btnCannotAfford = transform.Find("btnCanotAfford").GetComponent<UIButton>();
        btnDouble = transform.Find("btnDouble").GetComponent<UIButton>();
        btnNotDouble = transform.Find("btnNotDouble").GetComponent<UIButton>();

        for (int i = 0; i < 3; i++)
        {
            userPos[i] = transform.Find("userCard_" + i + "userPos");
            agreeFlags[i] = transform.Find("userCard_" + i + "/agreeFlag").gameObject;
            notCallFlags[i] = transform.Find("userCard_" + i + "/notCallFlag").gameObject;
            notHogFlags[i] = transform.Find("userCard_" + i + "/notHogFlag").gameObject;
            notDoubleFlags[i] = transform.Find("userCard_" + i + "/notDoubleFlag").gameObject;
            doubleFlags[i] = transform.Find("userCard_" + i + "/doubleFlag").gameObject;
            hogFlags[i] = transform.Find("userCard_" + i + "/hogFlag").gameObject;
            gradLandlordFlags[i] = transform.Find("userCard_" + i + "/gradLandlordFlag").gameObject;
            notOutCardFlags[i] = transform.Find("userCard_" + i + "/notOutCardFlag").gameObject;
            landlordFlags[i] = transform.Find("landlordFlag_" + i).gameObject;
            autoFlags[i] = transform.Find("userCard_" + i + "/autoFlag").gameObject;
            callPoliceFlags[i] = transform.Find("userCard_" + i + "/callPoliceFlag").gameObject;
            zhaDanFlags[i] = transform.Find("userCard_" + i + "/zhaDanFlag").gameObject;
            timeCount[i] = transform.Find("userCard_" + i + "/timeCount").gameObject;

            userCardCount[i] = transform.Find("userCard_" + i + "/cardCount").gameObject;
            bankerHoleCards[i] = transform.Find("HoleCards/HoleCard_" + i).GetComponent<UISprite>();
            outCardParent[i] = transform.Find("outCard_" + i);

            lblWinScore[i] = transform.Find("lblWinScore_" + i).GetComponent<UILabel>();
            lblLoseScore[i] = transform.Find("lblLoseScore_" + i).GetComponent<UILabel>();
        }
        shunZiFlag = transform.Find("shunZiFlag").gameObject;
        lianDuiFlag = transform.Find("lianDuiFlag").gameObject;
        planeFlag = transform.Find("planeFlag").gameObject;
        wangZhaFlag = transform.Find("wangZhaFlag").gameObject;
        chunTianFlag = transform.Find("chunTianFlag").gameObject;
        fanChunTianFlag = transform.Find("fanChunTianFlag").gameObject;
        winFlag = transform.Find("WinFlag").gameObject;
        lostFlag = transform.Find("LostFlag").gameObject;
        landlordAnim = transform.Find("landlordAnim");

        myHandCardParent = transform.Find("userCard_0/pokers");
        landlordPokerFlag = transform.Find("userCard_0/pokers/landlordPokerFlag");
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            myHandCards[i] = transform.Find("userCard_0/pokers/poker_" + i).GetComponent<UISprite>();
        }
        outCardItem = transform.Find("outCardItem").gameObject;
        bottomScore = transform.Find("bottomScore").GetComponent<UILabel>();
        currentTotalScore = transform.Find("mulScore").GetComponent<UILabel>();

        EventDelegate.Add(btnStart.onClick, OnBtnStartClick);
        EventDelegate.Add(btnAgree.onClick, OnBtnAgreeClick);
        EventDelegate.Add(btnChangeDesk.onClick, OnBtnSwitchDeskClick);
        EventDelegate.Add(btnJoinFriend.onClick, OnBtnJoinFriendClick);
        EventDelegate.Add(btnCopyRoomId.onClick, OnBtnCopyRoomIdClick);
        EventDelegate.Add(btnTrustee.onClick, OnBtnTrusteeClick);
        EventDelegate.Add(btnCancelSelect.onClick, OnBtnCancelSelectClick);
        EventDelegate.Add(btnAuto.onClick, OnBtnAutoClick);

        EventDelegate.Add(btnOutCard.onClick, OnBtnOutCardClick);
        EventDelegate.Add(btnNotOutCard.onClick, OnBtnDisCardClick);
        EventDelegate.Add(btnCardTip.onClick, OnBtnCardTipClick);
        EventDelegate.Add(btnCallLandlord.onClick, OnBtnCallLandlordClick);
        EventDelegate.Add(btnGradLandlord.onClick, OnBtnCallLandlordClick);
        EventDelegate.Add(btnCannotAfford.onClick, OnBtnCannotAffordClick);
        EventDelegate.Add(btnNotCall.onClick, OnBtnNotHogClick);
        EventDelegate.Add(btnNotHog.onClick, OnBtnNotHogClick);
        EventDelegate.Add(btnDouble.onClick, OnBtnDoubleClick);
        EventDelegate.Add(btnNotDouble.onClick, OnBtnNotDoubleClick);

        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            UIEventListener.Get(myHandCards[i].gameObject).onClick = OnBtnHandCardClick;
            UIEventListener.Get(myHandCards[i].gameObject).onDragOver = OnHandCardDrag;
        }

        InitPageConfig();
        InitData();
    }

    void InitPageConfig()
    {
        gameObject.SetActive(true);
        btnStart.gameObject.SetActive(false);
        btnAgree.gameObject.SetActive(false);
        btnChangeDesk.gameObject.SetActive(false);
        btnJoinFriend.gameObject.SetActive(false);
        btnCopyRoomId.gameObject.SetActive(false);
        btnTrustee.gameObject.SetActive(false);
        btnAuto.gameObject.SetActive(false);

        btnOutCard.gameObject.SetActive(false);
        btnNotOutCard.gameObject.SetActive(false);
        btnCardTip.gameObject.SetActive(false);
        btnCallLandlord.gameObject.SetActive(false);
        btnGradLandlord.gameObject.SetActive(false);
        btnNotCall.gameObject.SetActive(false);
        btnNotHog.gameObject.SetActive(false);
        btnCannotAfford.gameObject.SetActive(false);
        btnDouble.gameObject.SetActive(false);
        btnNotDouble.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            agreeFlags[i].SetActive(false);
            notCallFlags[i].SetActive(false);
            notHogFlags[i].SetActive(false);
            notDoubleFlags[i].SetActive(false);
            doubleFlags[i].SetActive(false);
            hogFlags[i].SetActive(false);
            gradLandlordFlags[i].SetActive(false);
            notOutCardFlags[i].SetActive(false);
            landlordFlags[i].SetActive(false);
            autoFlags[i].SetActive(false);
            callPoliceFlags[i].SetActive(false);
            zhaDanFlags[i].SetActive(false);
            timeCount[i].SetActive(false);
            outCardParent[i].gameObject.SetActive(true);
            lblWinScore[i].gameObject.SetActive(false);
            //lblWinScore[i].GetComponent<TweenAlpha>().ResetToBeginning();
            lblWinScore[i].GetComponent<TweenPosition>().ResetToBeginning();
            lblLoseScore[i].gameObject.SetActive(false);
            //lblLoseScore[i].GetComponent<TweenAlpha>().ResetToBeginning();
            lblLoseScore[i].GetComponent<TweenPosition>().ResetToBeginning();
        }
        outCardItem.SetActive(false);
        outCardItem.transform.Find("landlordsFlag").gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                outCards[i, j] = PoolManager.Instance.Spawn(outCardItem);
                outCards[i, j].SetActive(false);
                outCards[i, j].transform.parent = outCardParent[i];
                outCards[i, j].transform.localScale = Vector3.one;
                outCards[i, j].GetComponent<UISprite>().depth = 2 * j;
                outCards[i, j].transform.Find("landlordsFlag").GetComponent<UISprite>().depth = 2 * j + 1;
            }
        }

        shunZiFlag.SetActive(false);
        lianDuiFlag.SetActive(false);
        planeFlag.SetActive(false);
        wangZhaFlag.SetActive(false);
        chunTianFlag.SetActive(false);
        fanChunTianFlag.SetActive(false);
        shunZiFlag.GetComponent<TweenAlpha>().ResetToBeginning();
        lianDuiFlag.GetComponent<TweenAlpha>().ResetToBeginning();
        winFlag.SetActive(false);
        lostFlag.SetActive(false);
        winFlag.GetComponent<TweenPosition>().ResetToBeginning();
        winFlag.GetComponent<TweenAlpha>().ResetToBeginning();
        lostFlag.GetComponent<TweenPosition>().ResetToBeginning();
        lostFlag.GetComponent<TweenAlpha>().ResetToBeginning();
        winFlag.transform.Find("Sprite").GetComponent<TweenScale>().ResetToBeginning();
        lostFlag.transform.Find("Sprite").GetComponent<TweenScale>().ResetToBeginning();
        landlordAnim.gameObject.SetActive(false);

        bottomScore.gameObject.SetActive(false);
        currentTotalScore.gameObject.SetActive(false);
        //自适应
        landlordFlags[0].transform.localPosition = new Vector3(-AppConfig.screenWidth / 2 + 60f, -215f, 0f);
        landlordFlags[1].transform.localPosition = new Vector3(AppConfig.screenWidth / 2 - 80f, 230f, 0f);
        landlordFlags[2].transform.localPosition = new Vector3(-AppConfig.screenWidth / 2 + 80f, 230f, 0f);
    }

    #endregion

    void InitData()
    {
        btnAuto.gameObject.SetActive(false);
        btnTrustee.gameObject.SetActive(false);
        isAuto = false;
        btnAuto.transform.Find("autoImg").gameObject.SetActive(true);
        btnAuto.transform.Find("autoingImg").gameObject.SetActive(false);
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            myHandCards[i].gameObject.SetActive(false);
            myHandCards[i].GetComponent<BoxCollider>().enabled = false;
            myHandCards[i].transform.Find("landlordPokerFlag").gameObject.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            notCallFlags[i].SetActive(false);
            notHogFlags[i].SetActive(false);
            notDoubleFlags[i].SetActive(false);
            doubleFlags[i].SetActive(false);
            hogFlags[i].SetActive(false);
            gradLandlordFlags[i].SetActive(false);
            notOutCardFlags[i].SetActive(false);
            landlordFlags[i].SetActive(false);
            autoFlags[i].SetActive(false);
            callPoliceFlags[i].SetActive(false);
            timeCount[i].SetActive(false);
            userCardCount[i].SetActive(false);
            bankerHoleCards[i].gameObject.SetActive(false);
        }
        CloseCardTypeFlag();
        handCardList.Clear();
        selectCardList.Clear();
        beforeUserOutCardList.Clear();
        myHandCardParent.localPosition = new Vector3(69f, -5f, 0f);
        LandlordsModel.isPlayerCallLandlord = false;
    }


    public override void RegisterAction()
    {
        GameEvent.V_RefreshUserInfo += RefreshAgreeState;
        GameEvent.V_StartTimer += StartTimeCount;
        GameEvent.V_CloseTimer += CloseTimer;
        GameEvent.V_StopTimeOutCallback += StopTimeOutCallBack;
        GameEvent.V_PlayRedPackAnim += OnGetRedPack;

        LandlordsEvent.S_StateFree += OnStateFree;
        LandlordsEvent.S_StateCall += OnStateCall;
        LandlordsEvent.S_StateAddTime += OnStateAddTime;
        LandlordsEvent.S_StatePlay += OnStatePlay;

        LandlordsEvent.S_GameStart += OnGameStart;
        LandlordsEvent.S_UserCall += OnUserCall;
        LandlordsEvent.S_ReSendCard += OnReSendCard;
        LandlordsEvent.S_BankerStartOutCard += OnBankerStartOutCard;
        LandlordsEvent.S_UserAddTime += OnUserAddTime;
        LandlordsEvent.S_UserOutCard += OnUserOutCard;
        LandlordsEvent.S_OutCardFail += OnOutCardFail;
        LandlordsEvent.S_PassCard += OnPassCard;
        LandlordsEvent.S_GameEnd += OnGameEnd;
        LandlordsEvent.OnUserTrustee += OnUserTrustee;
        LandlordsEvent.V_ReStartNewGame += ReStartNewGame;
    }

    public override void RemoveAction()
    {
        GameEvent.V_RefreshUserInfo -= RefreshAgreeState;
        GameEvent.V_StartTimer -= StartTimeCount;
        GameEvent.V_CloseTimer -= CloseTimer;
        GameEvent.V_StopTimeOutCallback -= StopTimeOutCallBack;
        GameEvent.V_PlayRedPackAnim -= OnGetRedPack;

        LandlordsEvent.S_StateFree -= OnStateFree;
        LandlordsEvent.S_StateCall -= OnStateCall;
        LandlordsEvent.S_StateAddTime -= OnStateAddTime;
        LandlordsEvent.S_StatePlay -= OnStatePlay;

        LandlordsEvent.S_GameStart -= OnGameStart;
        LandlordsEvent.S_UserCall -= OnUserCall;
        LandlordsEvent.S_ReSendCard -= OnReSendCard;
        LandlordsEvent.S_BankerStartOutCard -= OnBankerStartOutCard;
        LandlordsEvent.S_UserAddTime -= OnUserAddTime;
        LandlordsEvent.S_UserOutCard -= OnUserOutCard;
        LandlordsEvent.S_OutCardFail -= OnOutCardFail;
        LandlordsEvent.S_PassCard -= OnPassCard;
        LandlordsEvent.S_GameEnd -= OnGameEnd;
        LandlordsEvent.OnUserTrustee -= OnUserTrustee;
        LandlordsEvent.V_ReStartNewGame -= ReStartNewGame;
    }

    #region UI响应

    //开始游戏
    void OnBtnStartClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        int playerNum = 0;
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            PlayerInRoom player = GameModel.GetDeskUser(i);
            if (player != null)
            {
                playerNum += 1;
            }
        }
        if (playerNum > 1)
        {
            //关闭上局游戏状态
            CloseOnGameEnd();

            btnStart.gameObject.SetActive(false);
            btnJoinFriend.gameObject.SetActive(false);
            btnCopyRoomId.gameObject.SetActive(false);
            GameService.Instance.UserAgree();
        }
        else
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "请等待其他玩家加入！");
        }
    }

    //准备
    void OnBtnAgreeClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        btnAgree.gameObject.SetActive(false);
        GameService.Instance.UserAgree();
    }

    //换桌
    void OnBtnSwitchDeskClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        btnChangeDesk.gameObject.SetActive(false);
        GameService.Instance.ChangeChair();
    }

    //邀请好友
    void OnBtnJoinFriendClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        string title = "兴隆斗地主 - 房间ID：" + GameModel.currentRoomId;
        string hostInfo = "";
        if (GameModel.playerInRoom.ContainsKey((uint)GameModel.hostUserId))
        {
            //hostInfo = "房主：" + GameModel.playerInRoom[(uint)GameModel.hostUserId].nickName + "，";
        }
        string content = "斗地主，约起来，" + GameModel.deskPlayerCount + "人房，" + hostInfo + GameModel.totalGameCount + "局，" + (GameModel.rulePayMode == 0 ? "房主支付" : "AA支付");
        if (GameModel.hostUserId != HallModel.userId)
        {
            content = "斗地主，约起来，" + GameModel.deskPlayerCount + "人房，" + hostInfo + GameModel.totalGameCount + "局";
        }
        string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2ce328d2daf7d92&redirect_uri=http://wx.mile1990.com/Down.aspx?gameid=" + HallModel.gameId + "%26channelCode=" + AppConfig.channelCode + "%26roomId=" + GameModel.currentRoomId + "%26appId=1" + "&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";
        PluginManager.Instance.WxShareUrl(0, url, title, content);
    }

    //复制房间ID
    void OnBtnCopyRoomIdClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        Util.CopyMessage(GameModel.currentRoomId.ToString());
        DoAction(GameEvent.V_OpenShortTip, "房间ID已复制到剪切板！");
    }

    //出牌
    void OnBtnOutCardClick()
    {
        if (selectCardList.Count == 0)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "您还没有选择牌不能出牌！");
            return;
        }
        //出牌
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        byte[] data = new byte[20];
        for (int i = 0; i < selectCardList.Count; i++)
        {
            data[i] = selectCardList[i];
        }
        LandlordsService.Instance.C2S_OutCard((byte)selectCardList.Count, data);
    }

    //弃牌
    void OnBtnDisCardClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        LandlordsService.Instance.C2S_PassCard();
    }

    //提示   
    void OnBtnCardTipClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (tipCardList.Count != 0)
        {
            if (selectIndex >= tipCardList.Count)
            {
                selectIndex = 0;
            }
            if (selectIndex < tipCardList.Count)
            {
                selectCardList.Clear();
                for (int j = 0; j < tipCardList[selectIndex].Length; j++)
                {
                    selectCardList.Add(tipCardList[selectIndex][j]);
                }
                for (int i = 0; i < handCardList.Count; i++)
                {
                    myHandCards[i].transform.localPosition = new Vector3(-460f + i * 48.4f, 0f, 0f);
                }

                //把handCardList中符合的index 保存下来
                List<int> tempIndexList = new List<int>();
                for (int i = 0; i < handCardList.Count; i++)
                {
                    for (int j = 0; j < selectCardList.Count; j++)
                    {
                        if (handCardList[i] == selectCardList[j])
                        {
                            if (!tempIndexList.Contains(i))
                            {
                                tempIndexList.Add(i);
                            }
                        }
                    }
                }
                //把 selectCardList中的元素对应自己的手牌中的牌y轴提高一些
                for (int i = 0; i < tempIndexList.Count; i++)
                {
                    myHandCards[tempIndexList[i]].transform.localPosition += new Vector3(0f, 25f, 0f);
                }
                selectIndex++;
            }
        }
    }

    //叫地主 抢地主
    void OnBtnCallLandlordClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        LandlordsService.Instance.C2S_CallScore(1);
    }

    //要不起
    void OnBtnCannotAffordClick()
    {
        try
        {
            AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
            LandlordsService.Instance.C2S_PassCard();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error handling button click: {e.Message}");
        }
    }

    //不抢 不叫
    void OnBtnNotHogClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        LandlordsService.Instance.C2S_CallScore(0);
    }

    //加倍
    void OnBtnDoubleClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        LandlordsService.Instance.C2S_AddTime(1);
    }

    //不加倍
    void OnBtnNotDoubleClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        LandlordsService.Instance.C2S_AddTime(0);
    }

    //托管
    void OnBtnTrusteeClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonAuto);
        LandlordsService.Instance.C2S_Trustee(0);
    }

    void OnBtnCancelSelectClick()
    {
        RefreshMyHandCardsPos();
    }

    bool isAuto = false;
    //托管
    private void OnBtnAutoClick()
    {
        if (!isAuto)//点击一次，托管状态
        {
            LandlordsService.Instance.C2S_Trustee(1);
        }
        else//点击两次，放弃托管
        {
            LandlordsService.Instance.C2S_Trustee(0);
        }
    }

    //点击手牌
    void OnBtnHandCardClick(GameObject obj)
    {
        //AudioManager.Instance.PlaySound(ShiSanShuiModel8.audioClickCard);
        int index = int.Parse(obj.name.Split('_')[1]);
        if (myHandCards[index].transform.localPosition.y < 5)
        {
            myHandCards[index].transform.localPosition += new Vector3(0f, 25f, 0f);
            selectCardList.Add(handCardList[index]);
        }
        else
        {
            myHandCards[index].transform.localPosition += new Vector3(0f, -25f, 0f);
            selectCardList.Remove(handCardList[index]);
        }
    }

    //拖拽牌
    void OnHandCardDrag(GameObject obj)
    {
        //AudioManager.Instance.PlaySound(ShiSanShuiModel8.audioClickCard);
        int index = int.Parse(obj.name.Split('_')[1]);
        if (myHandCards[index].transform.localPosition.y < 5)
        {
            myHandCards[index].transform.localPosition += new Vector3(0f, 25f, 0f);
            selectCardList.Add(handCardList[index]);
        }
        else
        {
            myHandCards[index].transform.localPosition += new Vector3(0f, -25f, 0f);
            selectCardList.Remove(handCardList[index]);
        }
    }

    #endregion

    #region 刷新方法

    //刷新准备状态
    void RefreshAgreeState(bool isRefreshScore)
    {
        for (int i = 0; i < 3; i++)
        {
            int chairId = (i + GameModel.chairId) % 3;
            PlayerInRoom player = GameModel.GetDeskUser(chairId);
            if (player != null && player.cbUserStatus == UserState.US_READY)
            {
                agreeFlags[i].SetActive(true);
            }
            else
            {
                agreeFlags[i].SetActive(false);
            }
        }
        if (agreeFlags[0].activeSelf)
        {
            btnAgree.gameObject.SetActive(false);
        }
    }

    bool isHaveRedPack = false;
    void OnGetRedPack(float a, string str)
    {
        isHaveRedPack = true;
    }


    //手牌排序
    void RangeHandCard()
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            for (int j = i + 1; j < handCardList.Count; j++)
            {
                if (LandlordsModel.CompareCard(handCardList[i], handCardList[j]) < 0)
                {
                    byte mid = handCardList[i];
                    handCardList[i] = handCardList[j];
                    handCardList[j] = mid;
                }
            }
        }
    }

    //手牌升序排序
    List<byte> RangeIncreaseHandCard(List<byte> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = i + 1; j < data.Count; j++)
            {
                if (LandlordsModel.CompareCard(data[i], data[j]) > 0)
                {
                    byte mid = data[i];
                    data[i] = data[j];
                    data[j] = mid;
                }
            }
        }
        return data;
    }

    //刷新手牌
    void RefreshHandCard()
    {
        selectCardList.Clear();
        //手牌排序
        RangeHandCard();
        int lackCardCount = (20 - handCardList.Count);
        myHandCardParent.DOLocalMoveX(23f * lackCardCount, 0.1f);
        //手牌位置，显示，码牌
        for (int i = 0; i < 20; i++)
        {
            if (i < handCardList.Count)
            {
                //手牌
                myHandCards[i].gameObject.SetActive(true);
                myHandCards[i].transform.DOLocalMoveY(0f, 0.1f);
                myHandCards[i].spriteName = LandlordsModel.GetPokerName(handCardList[i]);
            }
            else
            {
                myHandCards[i].gameObject.SetActive(false);
            }
        }
        if (LandlordsModel.bankerChairdId != 65535 && LandlordsModel.bankerChairdId == GameModel.chairId)
        {
            if (handCardList.Count > 0)
            {
                myHandCards[handCardList.Count - 1].transform.Find("landlordPokerFlag").gameObject.SetActive(true);
            }
        }
    }

    //刷新出牌
    void RefreshOutCard(int userIndex, int cardCount, byte[] cardData)
    {
        if (userIndex == 0 || cardCount < 11)
        {
            //显示
            for (int i = 0; i < 20; i++)
            {
                if (i < cardCount)
                {
                    //牌的初始坐标，以及间距
                    float dis = 45f;
                    float startPos = -280f;
                    if (cardCount == 1)
                    {
                        dis = 45f;
                        startPos = 0f;
                    }
                    else
                    {
                        dis = 720f / (cardCount - 1);
                    }
                    if (dis > 45f)
                    {
                        dis = 45f;
                        startPos = -dis * (cardCount - 1) / 2f;
                    }
                    //显示牌
                    outCards[userIndex, i].SetActive(true);
                    outCards[userIndex, i].transform.localPosition = new Vector3(startPos + dis * i, 0f, 0f);
                    outCards[userIndex, i].GetComponent<UISprite>().spriteName = LandlordsModel.GetPokerName(cardData[i]);
                }
                else
                {
                    outCards[userIndex, i].SetActive(false);
                }
            }
        }
        else
        {
            //显示
            for (int i = 0; i < 20; i++)
            {
                if (i < cardCount)
                {
                    if (i <= 9)
                    {
                        //牌的初始坐标，以及间距
                        float dis = 45f;
                        float startPos = -202f;
                        //显示牌
                        outCards[userIndex, i].SetActive(true);
                        outCards[userIndex, i].transform.localPosition = new Vector3(startPos + dis * i, 0f, 0f);
                    }
                    else
                    {
                        //牌的初始坐标，以及间距
                        float dis = 45f;
                        float startPos = -202f;
                        //显示牌
                        outCards[userIndex, i].SetActive(true);
                        outCards[userIndex, i].transform.localPosition = new Vector3(startPos + dis * (i - 10), -80f, 0f);
                    }
                    outCards[userIndex, i].GetComponent<UISprite>().spriteName = LandlordsModel.GetPokerName(cardData[i]);
                }
                else
                {
                    outCards[userIndex, i].SetActive(false);
                }
            }
        }

    }

    //刷新手牌数目
    void RefreshOtherHandCardNum(byte[] cardCount)
    {
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            if (index != 0)
            {
                userCardCount[index].GetComponent<UILabel>().text = cardCount[i].ToString();
            }
            if (cardCount[i] < 4)//显示警报
            {
                callPoliceFlags[index].SetActive(true);
            }
        }
    }

    //刷新自己手牌的位置
    void RefreshMyHandCardsPos()
    {
        selectCardList.Clear();
        for (int i = 0; i < handCardList.Count; i++)
        {
            myHandCards[i].transform.DOLocalMoveY(0f, 0.1f);
        }
    }

    //刷新托管按钮
    void RefreshAutoButton()
    {
        if (!isAuto)//点击一次，托管状态
        {
            isAuto = true;
            btnAuto.transform.Find("autoImg").gameObject.SetActive(false);
            btnAuto.transform.Find("autoingImg").gameObject.SetActive(true);
        }
        else//点击两次，放弃托管
        {
            isAuto = false;
            btnAuto.transform.Find("autoImg").gameObject.SetActive(true);
            btnAuto.transform.Find("autoingImg").gameObject.SetActive(false);
        }
    }

    //播放出牌音效
    void PlayOutCardTypeAudioAndAnim(UInt16 OutCardUser, byte CardTyp, byte firstCard)
    {
        //性别
        int sex = 0;
        PlayerInRoom player = GameModel.GetDeskUser(OutCardUser);
        int outCardIndex = (OutCardUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        if (player != null)
        {
            sex = player.cbGender;
        }
        int firstCardValue = LandlordsModel.GetPokerValue(firstCard);
        switch (CardTyp)
        {
            case 1:
                #region 单张
                if (sex == 0)
                {
                    if (firstCardValue == 14)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DanZhang[0]);
                    }
                    else if (firstCardValue == 15)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DanZhang[1]);
                    }
                    else if (firstCardValue == 21)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DanZhang[13]);
                    }
                    else if (firstCardValue == 22)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DanZhang[14]);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DanZhang[firstCardValue - 1]);
                    }
                }
                else
                {
                    if (firstCardValue == 14)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DanZhang[0]);
                    }
                    else if (firstCardValue == 15)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DanZhang[1]);
                    }
                    else if (firstCardValue == 21)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DanZhang[13]);
                    }
                    else if (firstCardValue == 22)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DanZhang[14]);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DanZhang[firstCardValue - 1]);
                    }
                }
                #endregion
                break;
            case 2:
                #region 对子
                if (sex == 0)
                {
                    if (firstCardValue == 14)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DuiZi[0]);
                    }
                    else if (firstCardValue == 15)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DuiZi[1]);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_DuiZi[firstCardValue - 1]);
                    }
                }
                else
                {
                    if (firstCardValue == 14)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DuiZi[0]);
                    }
                    else if (firstCardValue == 15)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DuiZi[1]);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_DuiZi[firstCardValue - 1]);
                    }
                }
                #endregion
                break;
            case 3:
                #region 连对
                lianDuiFlag.SetActive(true);
                lianDuiFlag.GetComponent<TweenAlpha>().PlayForward();
                Invoke("CloseCardTypeFlag", 1.2f);
                AudioManager.Instance.PlaySound(LandlordsModel.audioShunZi);
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_lianDui);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_lianDui);
                }
                #endregion
                break;
            case 4:
                #region 三张
                if (sex == 0)
                {
                    if (firstCardValue == 14)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_SanZhang[0]);
                    }
                    else if (firstCardValue == 15)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_SanZhang[1]);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_SanZhang[firstCardValue - 1]);
                    }
                }
                else
                {
                    if (firstCardValue == 14)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_SanZhang[0]);
                    }
                    else if (firstCardValue == 15)
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_SanZhang[1]);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(LandlordsModel.man_SanZhang[firstCardValue - 1]);
                    }
                }
                #endregion
                break;
            case 5:
                #region 三带一
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_sanDaiYi);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_sanDaiYi);
                }
                #endregion
                break;
            case 6:
                #region 三带一对
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_sanDaiDui);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_sanDaiDui);
                }
                #endregion
                break;
            case 7:
                #region 顺子
                shunZiFlag.SetActive(true);
                shunZiFlag.GetComponent<TweenAlpha>().PlayForward();
                Invoke("CloseCardTypeFlag", 1.2f);
                AudioManager.Instance.PlaySound(LandlordsModel.audioShunZi);
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_shunZi);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_shunZi);
                }
                #endregion
                break;
            case 8:
                #region 四带单
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_siDaiEr);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_siDaiEr);
                }
                #endregion
                break;
            case 9:
                #region 四带双
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_siDaiLiangDui);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_siDaiLiangDui);
                }
                #endregion
                break;
            case 10:
                #region 飞机
                planeFlag.SetActive(true);
                Invoke("CloseCardTypeFlag", 2.06f);
                AudioManager.Instance.PlaySound(LandlordsModel.audioFeiJi);
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_feiJi);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_feiJi);
                }
                #endregion
                break;
            case 11:
                #region 炸弹
                zhaDanFlags[outCardIndex].SetActive(true);
                Invoke("CloseCardTypeFlag", 1.11f);
                AudioManager.Instance.PlaySound(LandlordsModel.audioZhaDan);
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_zhaDan);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_zhaDan);
                }
                #endregion
                break;
            case 12:
                #region 王炸
                wangZhaFlag.SetActive(true);
                Invoke("CloseCardTypeFlag", 2.2f);
                AudioManager.Instance.PlaySound(LandlordsModel.audioZhaDan);
                if (sex == 0)
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_wangZha);
                }
                else
                {
                    AudioManager.Instance.PlaySound(LandlordsModel.man_wangZha);
                }
                #endregion
                break;
            default:

                break;
        }
    }

    void CloseCardTypeFlag()
    {
        shunZiFlag.SetActive(false);
        lianDuiFlag.SetActive(false);
        planeFlag.SetActive(false);
        wangZhaFlag.SetActive(false);
        chunTianFlag.SetActive(false);
        fanChunTianFlag.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            zhaDanFlags[i].SetActive(false);
        }
        shunZiFlag.GetComponent<TweenAlpha>().PlayReverse();
        lianDuiFlag.GetComponent<TweenAlpha>().PlayReverse();
    }

    //关闭功能按钮
    void CloseGameUI()
    {
        btnNotCall.gameObject.SetActive(false);
        btnCallLandlord.gameObject.SetActive(false);
        btnNotHog.gameObject.SetActive(false);
        btnGradLandlord.gameObject.SetActive(false);
        btnCannotAfford.gameObject.SetActive(false);
        btnOutCard.gameObject.SetActive(false);
        btnNotOutCard.gameObject.SetActive(false);
        btnCardTip.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            notCallFlags[i].SetActive(false);
            notOutCardFlags[i].SetActive(false);
        }
    }

    //游戏结束后关闭相关显示
    void CloseOnGameEnd()
    {
        for (int i = 0; i < 3; i++)
        {
            lblWinScore[i].gameObject.SetActive(false);
            //lblWinScore[i].GetComponent<TweenAlpha>().PlayReverse();
            lblWinScore[i].GetComponent<TweenPosition>().PlayReverse();
            lblLoseScore[i].gameObject.SetActive(false);
            //lblLoseScore[i].GetComponent<TweenAlpha>().PlayReverse();
            lblLoseScore[i].GetComponent<TweenPosition>().PlayReverse();
            for (int j = 0; j < 20; j++)
            {
                if (outCards[i, j])
                {
                    outCards[i, j].SetActive(false);
                    outCards[i, j].transform.Find("landlordsFlag").gameObject.SetActive(false);
                }
            }
        }
        //清空数据
        InitData();
        LandlordsModel.InitData();
    }


    void ReStartNewGame()
    {
        CloseOnGameEnd();
        //showChangeDeskBtn();
    }

    void showChangeDeskBtn()
    {
        btnChangeDesk.gameObject.SetActive(GameModel.serverType != GameModel.ServerKind_Private);
    }
    //换桌提示
    void SendTipMessage()
    {
        if (GameModel.serverType == GameModel.ServerKind_RedPack && HallModel.serverList.ContainsKey(HallModel.currentServerId))
        {
            HallModel.messageList.Add("此房间每局消耗 " + HallModel.serverList[HallModel.currentServerId].serviceMoney + " 钻石");
            //HallModel.messageList.Add("提示：游戏中换桌不影响红包雨局数累计哦！");
        }
        if (GameModel.serverType == GameModel.ServerKind_Gold && HallModel.serverList.ContainsKey(HallModel.currentServerId))
        {
            HallModel.messageList.Add("此房间每局消耗 " + HallModel.serverList[HallModel.currentServerId].serviceMoney + " 金币");
        }
        DoAction(GameEvent.S_ReceiveSystemMsg);
    }
    #endregion

    #region 网络响应

    #region 游戏状态
    //空闲状态
    void OnStateFree(Bs.Gameddz.S_StatusFree pro)
    {
        InitData();//清空临时数据
        Invoke("SendTipMessage", 2f);
        #region 显示底分和倍数
        bottomScore.gameObject.SetActive(true);
        currentTotalScore.gameObject.SetActive(true);
        bottomScore.text = pro.CellScore.ToString();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i == GameModel.chairId)
                currentTotalScore.text = pro.Times[i].ToString();
        }

        #endregion
        //Debug.Log("serverType=" + GameModel.serverType);
        if (GameModel.serverType == GameModel.ServerKind_Private) //房卡模式
        {
            if (GameModel.currentGameCount > 0)//有局数
            {
                GameService.Instance.UserAgree();
            }
            else
            {
                btnJoinFriend.gameObject.SetActive(true);
                btnCopyRoomId.gameObject.SetActive(true);
                GameService.Instance.UserAgree();
                btnJoinFriend.transform.localPosition = new Vector3(-180, -130, 0);
                btnCopyRoomId.transform.localPosition = new Vector3(180, -130, 0);
            }
        }
        else//金币场
        {
            if (GameModel.serverType == GameModel.ServerKind_Gold) //金币场
            {
                //托管状态
                btnTrustee.gameObject.SetActive(LandlordsModel.isPlayerTrustee[GameModel.chairId]);
                GameService.Instance.UserAgree();
                Invoke("showChangeDeskBtn", 0.1f);
            }
        }
        RefreshAgreeState(false);
    }

    //叫抢地主状态
    void OnStateCall(Bs.Gameddz.S_StatusCall pro)
    {
        InitData();//清空临时数据
        Invoke("SendTipMessage", 2f);
        CancelInvoke("showChangeDeskBtn");
        btnChangeDesk.gameObject.SetActive(false);
        #region 显示底分和倍数
        bottomScore.gameObject.SetActive(true);
        currentTotalScore.gameObject.SetActive(true);
        bottomScore.text = pro.CellScore.ToString();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i == GameModel.chairId)
                currentTotalScore.text = pro.Times[i].ToString();
        }
        #endregion
        #region 显示玩家托管状态
        if (GameModel.serverType != GameModel.ServerKind_Private)
        {
            //btnAuto.gameObject.SetActive(true);
            btnTrustee.gameObject.SetActive(LandlordsModel.isPlayerTrustee[GameModel.chairId]);
            if (LandlordsModel.isPlayerTrustee[GameModel.chairId])
            {
                isAuto = false;
            }
            else
            {
                isAuto = true;
            }
            RefreshAutoButton();
            for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
            {
                int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
                autoFlags[index].SetActive(LandlordsModel.isPlayerTrustee[i]);
            }
        }
        #endregion
        #region 显示我自己的手牌
        //关闭上局显示的牌        
        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            myHandCards[i].gameObject.SetActive(false);
            myHandCards[i].GetComponent<BoxCollider>().enabled = true;
        }
        for (int i = 0; i < pro.HandCardData.Count; i++)
        {
            handCardList.Add((byte)pro.HandCardData[i]);
        }
        for (int j = 0; j < LandlordsModel.NORMAL_HANDCARDNUM; j++)
        {
            AudioManager.Instance.PlaySound(LandlordsModel.audioSendCard);
            myHandCards[j].gameObject.SetActive(true);
            myHandCards[j].transform.localPosition += new Vector3(0f, 5f, 0f);
            myHandCards[j].transform.DOLocalMoveY(0f, 0.1f);
            myHandCards[j].transform.Find("landlordPokerFlag").gameObject.SetActive(false);
            myHandCards[j].GetComponent<UISprite>().spriteName = LandlordsModel.GetPokerName(handCardList[j]);
        }
        //显示手牌数量
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            userCardCount[i].SetActive(true);
            userCardCount[i].GetComponent<UILabel>().text = LandlordsModel.NORMAL_HANDCARDNUM.ToString();
        }
        RefreshHandCard();//重排手牌
        #endregion
        #region 显示玩家抢庄结果
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            if (pro.ScoreInfo[i] != 255)
            {
                notCallFlags[index].SetActive(pro.ScoreInfo[i] == 0);
                notHogFlags[index].SetActive(pro.ScoreInfo[i] == 1);
                hogFlags[index].SetActive(pro.ScoreInfo[i] == 2);
                gradLandlordFlags[index].SetActive(pro.ScoreInfo[i] == 3);
            }
        }
        int currentCallIndex = ((int)pro.CurrentUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        if (pro.CurrentUser != 65535)
        {
            hogFlags[currentCallIndex].SetActive(false);
            gradLandlordFlags[currentCallIndex].SetActive(false);
            notCallFlags[currentCallIndex].SetActive(false);
            notHogFlags[currentCallIndex].SetActive(false);
            if (pro.CurrentUser == GameModel.chairId)
            {
                btnGradLandlord.gameObject.SetActive(true);
                btnNotHog.gameObject.SetActive(true);
            }
        }
        CloseTimer();
        StartTimeCount(LandlordsModel.callTime, currentCallIndex, null);
        #endregion
    }

    //加倍状态
    void OnStateAddTime(Bs.Gameddz.S_StatusAddTimes pro)
    {
        InitData();//清空临时数据
        Invoke("SendTipMessage", 2f);
        CancelInvoke("showChangeDeskBtn");
        btnChangeDesk.gameObject.SetActive(false);
        #region 显示底分和倍数
        bottomScore.gameObject.SetActive(true);
        currentTotalScore.gameObject.SetActive(true);
        bottomScore.text = pro.CellScore.ToString();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i == GameModel.chairId)
                currentTotalScore.text = pro.Times[i].ToString();
        }
        #endregion
        #region 显示玩家托管状态
        if (GameModel.serverType != GameModel.ServerKind_Private)
        {
            //btnAuto.gameObject.SetActive(true);
            btnTrustee.gameObject.SetActive(LandlordsModel.isPlayerTrustee[GameModel.chairId]);
            if (LandlordsModel.isPlayerTrustee[GameModel.chairId])
            {
                isAuto = false;
            }
            else
            {
                isAuto = true;
            }
            RefreshAutoButton();
            for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
            {
                int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
                autoFlags[index].SetActive(LandlordsModel.isPlayerTrustee[i]);
            }
        }
        #endregion
        #region 显示我自己的手牌
        //关闭上局显示的牌        
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            myHandCards[i].gameObject.SetActive(false);
            myHandCards[i].GetComponent<BoxCollider>().enabled = true;
        }
        if (pro.LandUser == GameModel.chairId)
        {
            for (int i = 0; i < pro.HandCardData.Count; i++)
            {
                handCardList.Add((byte)pro.HandCardData[i]);
            }
        }
        else
        {
            for (int i = 0; i < pro.HandCardData.Count; i++)
            {
                handCardList.Add((byte)pro.HandCardData[i]);
            }
        }
        for (int j = 0; j < handCardList.Count; j++)
        {
            AudioManager.Instance.PlaySound(LandlordsModel.audioSendCard);
            myHandCards[j].gameObject.SetActive(true);
            myHandCards[j].transform.localPosition += new Vector3(0f, 5f, 0f);
            myHandCards[j].transform.DOLocalMoveY(0f, 0.1f);
            myHandCards[j].transform.Find("landlordPokerFlag").gameObject.SetActive(false);
            myHandCards[j].GetComponent<UISprite>().spriteName = LandlordsModel.GetPokerName(handCardList[j]);
        }
        RefreshHandCard();//重排手牌
        #endregion
        #region 显示地主标志、底牌
        int bankerIndex = ((int)pro.LandUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        landlordFlags[bankerIndex].SetActive(true);
        for (int i = 0; i < pro.BankerCard.Count; i++)
        {
            bankerHoleCards[i].gameObject.SetActive(true);
            bankerHoleCards[i].spriteName = LandlordsModel.GetPokerName((byte)pro.BankerCard[i]);
        }
        #endregion
        #region 显示加倍分数
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            if (pro.AddTimes[i] != 255)
            {
                doubleFlags[index].SetActive(pro.AddTimes[i] == 1);
                notDoubleFlags[index].SetActive(pro.AddTimes[i] == 0);
                if (pro.AddTimes[i] == 1)
                {
                    doubleFlags[index].SetActive(true);
                }
                else
                {
                    notDoubleFlags[index].SetActive(true);
                }
            }
            else
            {
                if (index == 0)
                {
                    btnNotDouble.gameObject.SetActive(true);
                    btnDouble.gameObject.SetActive(true);
                }
            }
        }
        #endregion
    }

    //游戏中状态
    void OnStatePlay(Bs.Gameddz.S_StatusPlay pro)
    {
        InitData();//清空临时数据
        Invoke("SendTipMessage", 2f);
        CancelInvoke("showChangeDeskBtn");
        btnChangeDesk.gameObject.SetActive(false);
        #region 显示底分和倍数
        bottomScore.gameObject.SetActive(true);
        currentTotalScore.gameObject.SetActive(true);
        bottomScore.text = pro.CellScore.ToString();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i == GameModel.chairId)
                currentTotalScore.text = pro.Times[i].ToString();
        }
        #endregion
        #region 显示玩家托管状态
        if (GameModel.serverType != GameModel.ServerKind_Private)
        {
            //btnAuto.gameObject.SetActive(true);
            btnTrustee.gameObject.SetActive(LandlordsModel.isPlayerTrustee[GameModel.chairId]);
            if (LandlordsModel.isPlayerTrustee[GameModel.chairId])
            {
                isAuto = false;
            }
            else
            {
                isAuto = true;
            }
            RefreshAutoButton();
            for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
            {
                int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
                autoFlags[index].SetActive(LandlordsModel.isPlayerTrustee[i]);
            }
        }
        #endregion
        #region 显示地主、地主底牌
        int bankerIndex = ((int)pro.BankerUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        landlordFlags[bankerIndex].SetActive(true);
        for (int i = 0; i < pro.BankerCard.Count; i++)
        {
            bankerHoleCards[i].gameObject.SetActive(true);
            bankerHoleCards[i].spriteName = LandlordsModel.GetPokerName((byte)pro.BankerCard[i]);
        }
        #endregion
        #region 显示本轮出牌
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i != pro.CurrentUser)
            {
                if (pro.LastOutState[i] != 255)
                {
                    if (pro.LastCardCount[i] != 0)//该玩家出牌了
                    {
                        //显示出牌玩家出的牌
                        int outCardIndex = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
                        byte[] cardData = new byte[pro.LastCardCount[i]];
                        for (int j = 0; j < pro.LastCardCount[i]; j++)
                        {
                            var cardInfo = pro.LastCardData[i];
                            if (cardInfo.Card[j] != 0)
                            {
                                cardData[j] = (byte)cardInfo.Card[j];
                            }
                        }
                        RefreshOutCard(outCardIndex, (int)pro.LastCardCount[i], cardData);
                        if (i == pro.BankerUser)   //显示地主出牌的标志
                        {
                            for (int j = 0; j < pro.LastCardCount[i]; j++)
                            {
                                outCards[outCardIndex, j].transform.Find("landlordsFlag").gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        int notOutCardIndex = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
                        notOutCardFlags[notOutCardIndex].SetActive(true);
                    }
                }
            }
        }
        #endregion
        #region 显示我自己的剩余手牌
        //关闭上局显示的牌        
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            myHandCards[i].gameObject.SetActive(false);
            myHandCards[i].GetComponent<BoxCollider>().enabled = true;
        }
        int myHandCardCount = 0;
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            if (index == 0)
            {
                myHandCardCount = (int)pro.HandCardCount[i];
            }
            else//显示别人剩余多少张牌
            {
                userCardCount[index].SetActive(true);
                userCardCount[index].GetComponent<UILabel>().text = pro.HandCardCount[i].ToString();
            }
        }
        for (int i = 0; i < myHandCardCount; i++)
        {
            handCardList.Add((byte)pro.HandCardData[i]);
        }
        for (int j = 0; j < handCardList.Count; j++)
        {
            AudioManager.Instance.PlaySound(LandlordsModel.audioSendCard);
            myHandCards[j].gameObject.SetActive(true);
            myHandCards[j].transform.localPosition += new Vector3(0f, 5f, 0f);
            myHandCards[j].transform.DOLocalMoveY(0f, 0.1f);
            myHandCards[j].transform.Find("landlordPokerFlag").gameObject.SetActive(false);
            myHandCards[j].GetComponent<UISprite>().spriteName = LandlordsModel.GetPokerName(handCardList[j]);
        }
        RefreshHandCard();//重排手牌
        #endregion
        #region 显示当前出牌玩家操作
        int currentIndex = ((int)pro.CurrentUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        if (pro.CurrentUser == GameModel.chairId)
        {
            if (pro.Active == 0)//被动出牌
            {
                if (pro.SearchCount != 0)
                {
                    btnOutCard.transform.localPosition = new Vector3(220f, -45f, 0f);
                    btnCardTip.transform.localPosition = new Vector3(0f, -45f, 0f);
                    btnNotOutCard.transform.localPosition = new Vector3(-220f, -45f, 0f);
                    btnOutCard.gameObject.SetActive(true);
                    btnCardTip.gameObject.SetActive(true);
                    btnNotOutCard.gameObject.SetActive(true);
                    //要的起 保存提示信息
                    tipCardList = new List<byte[]>();
                    for (int i = 0; i < pro.SearchCount; i++)
                    {
                        var cardInfo = pro.ResultCard[i];
                        byte[] temp = new byte[pro.SearchCardCount[i]];
                        for (int j = 0; j < pro.SearchCardCount[i]; j++)
                        {
                            temp[j] = (byte)cardInfo.Card[j];
                        }
                        tipCardList.Add(temp);
                    }
                }
                else
                {
                    btnCannotAfford.gameObject.SetActive(true);
                }
            }
            else//主动出牌
            {
                btnOutCard.transform.localPosition = new Vector3(0f, -45f, 0f);
                btnOutCard.gameObject.SetActive(true);
            }

        }
        CloseTimer();
        StartTimeCount(LandlordsModel.outCardTime, currentIndex, null);
        #endregion
    }
    #endregion

    //游戏开始
    void OnGameStart(Bs.Gameddz.S_GameStart pro)
    {
        InitData();//清空临时数据
        CancelInvoke("showChangeDeskBtn");
        currentTotalScore.text = "1";
        //关闭上局显示的牌
        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            myHandCards[i].gameObject.SetActive(false);
        }
        btnJoinFriend.gameObject.SetActive(false);
        btnCopyRoomId.gameObject.SetActive(false);
        btnChangeDesk.gameObject.SetActive(false);
        //btnAuto.gameObject.SetActive(GameModel.serverType != GameModel.ServerKind_Private);  

        Debug.Log("游戏开始,StartUser=" + pro.StartUser + ",Count=" + pro.CardData.Count);

        LandlordsModel.bankerChairdId = 65535;
        LandlordsModel.currentUser = (int)pro.StartUser;
        for (int i = 0; i < pro.CardData.Count; i++)
        {
            LandlordsModel.myHandCards[i] = (byte)pro.CardData[i];
        }

        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            handCardList.Add(LandlordsModel.myHandCards[i]);
        }

        StartCoroutine(PlayGameStartAnim());
    }
    //用户叫分
    void OnUserCall(Bs.Gameddz.S_RobLand pro)
    {
        #region 叫分音效
        PlayerInRoom player = GameModel.GetDeskUser((int)pro.RobLandUser);
        if (player != null)
        {
            if (player.cbGender == 0)
            {
                switch (pro.RobLand)
                {
                    case 0:
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_notCall);
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_notGrad);
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_callLandlord);
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_gradLandlord[UnityEngine.Random.Range(0, 3)]);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (pro.RobLand)
                {
                    case 0:
                        AudioManager.Instance.PlaySound(LandlordsModel.man_notCall);
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound(LandlordsModel.man_notGrad);
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound(LandlordsModel.woman_callLandlord);
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound(LandlordsModel.man_gradLandlord[UnityEngine.Random.Range(0, 3)]);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        #region 显示倍数
        for (int i = 0; i < pro.Times.Count; i++)
        {
            if (i == GameModel.chairId)
                currentTotalScore.text = pro.Times[i].ToString();
        }
        #endregion
        #region  轮到自己抢地主时，显示抢地主按钮
        int index = ((int)pro.RobLandUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        notCallFlags[index].SetActive(pro.RobLand == 0);
        notHogFlags[index].SetActive(pro.RobLand == 1);
        hogFlags[index].SetActive(pro.RobLand == 2);
        gradLandlordFlags[index].SetActive(pro.RobLand == 3);
        if (pro.RobLandUser == GameModel.chairId)
        {
            btnCallLandlord.gameObject.SetActive(false);
            btnGradLandlord.gameObject.SetActive(false);
            btnNotCall.gameObject.SetActive(false);
            btnNotHog.gameObject.SetActive(false);
        }
        CloseTimer();
        int nextCallIndex = ((int)pro.NextUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        timeCount[nextCallIndex].SetActive(true);
        if (pro.NextUser != 65535)
        {
            if (!LandlordsModel.isPlayerCallLandlord)
            {
                if (pro.RobLand != 0)
                {
                    LandlordsModel.isPlayerCallLandlord = true;
                }
            }
            if (pro.NextUser == GameModel.chairId)
            {
                if (pro.RobLand == 0 && !LandlordsModel.isPlayerCallLandlord)
                {
                    btnCallLandlord.gameObject.SetActive(true);
                    btnNotCall.gameObject.SetActive(true);
                }
                else
                {
                    btnGradLandlord.gameObject.SetActive(true);
                    btnNotHog.gameObject.SetActive(true);
                }
            }
            notCallFlags[nextCallIndex].SetActive(false);
            notHogFlags[nextCallIndex].SetActive(false);
            hogFlags[nextCallIndex].SetActive(false);
            gradLandlordFlags[nextCallIndex].SetActive(false);
            StartTimeCount(LandlordsModel.callTime, nextCallIndex, null);
        }
        #endregion
    }

    //重新发牌
    void OnReSendCard(Bs.Gameddz.S_ReOutCard pro)
    {
        #region 关闭显示状态
        for (int i = 0; i < 3; i++)
        {
            notCallFlags[i].SetActive(false);
            notHogFlags[i].SetActive(false);
            timeCount[i].SetActive(false);
            userCardCount[i].SetActive(false);
        }
        handCardList.Clear();
        #endregion
        #region 关闭上局显示的牌
        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            myHandCards[i].gameObject.SetActive(false);
        }
        LandlordsModel.currentUser = (int)pro.StartUser;
        for (int i = 0; i < pro.CardData.Count; i++)
        {
            LandlordsModel.myHandCards[i] = (byte)pro.CardData[i];
        }

        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            handCardList.Add(LandlordsModel.myHandCards[i]);
        }
        StartCoroutine(PlayGameStartAnim());
        #endregion
    }

    //显示庄家信息，庄家开始出牌
    void OnBankerStartOutCard(Bs.Gameddz.S_BankerInfo pro)
    {
        #region 显示地主信息
        for (int i = 0; i < 3; i++)
        {
            hogFlags[i].SetActive(false);
            gradLandlordFlags[i].SetActive(false);
            notCallFlags[i].SetActive(false);
            notHogFlags[i].SetActive(false);
        }
        int BankerIndex = (LandlordsModel.bankerChairdId - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        //landlordFlags[BankerIndex].SetActive(true);
        //添加地主动画
        Vector3 tarPos = landlordFlags[BankerIndex].transform.localPosition;
        landlordAnim.gameObject.SetActive(true);
        landlordAnim.localPosition = new Vector3(0f, 30f, 0f);
        landlordAnim.localScale = Vector3.one * 0.8f;
        landlordAnim.DOScale(Vector3.one * 0.3f, 0.6f).SetDelay(0.3f);
        landlordAnim.DOLocalMove(tarPos, 0.6f).SetDelay(0.3f).onComplete = delegate
        {
            landlordAnim.gameObject.SetActive(false);
            landlordFlags[BankerIndex].SetActive(true);
            //地主动画完成后操作
            for (int i = 0; i < 3; i++)
            {
                bankerHoleCards[i].gameObject.SetActive(true);
                bankerHoleCards[i].spriteName = LandlordsModel.GetPokerName(LandlordsModel.bankerHoleCards[i]);
            }
            #endregion
            #region 增加地主手牌
            if (LandlordsModel.bankerChairdId == GameModel.chairId)
            {
                //增加我自己的牌的数目
                for (int i = 0; i < LandlordsModel.bankerHoleCards.Length; i++)
                {
                    handCardList.Add(LandlordsModel.bankerHoleCards[i]);
                }
                StartCoroutine(PlayBankerSendCardAnim());
            }
            else
            {
                userCardCount[BankerIndex].GetComponent<UILabel>().text = "20";
            }
            #endregion
            #region 是否有加倍
            CloseTimer();
            if (pro.HasAddTime == 0)
            {
                if (LandlordsModel.bankerChairdId == GameModel.chairId)
                {
                    btnOutCard.transform.localPosition = new Vector3(0f, -45f, 0f);
                    btnOutCard.gameObject.SetActive(true);
                }
                for (int i = 0; i < 20; i++)
                {
                    myHandCards[i].GetComponent<BoxCollider>().enabled = true;
                }
                StartTimeCount(LandlordsModel.firstOutCardTime, BankerIndex, null);
            }
            else
            {
                btnDouble.gameObject.SetActive(pro.HasAddTime == 1);
                btnNotDouble.gameObject.SetActive(pro.HasAddTime == 1);
                timeCount[0].SetActive(true);
                StartTimeCount(LandlordsModel.addTime, 0, null);
            }
            #endregion
        };


    }

    //用户加倍
    void OnUserAddTime(Bs.Gameddz.S_AddTimes pro)
    {
        //显示倍数
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i == GameModel.chairId)
                currentTotalScore.text = pro.Times[i].ToString();
        }

        #region 播放音效
        PlayerInRoom player = GameModel.GetDeskUser((int)pro.User);
        if (player != null)
        {
            if (player.cbGender == 0)
            {
                if (pro.AddTimes == 1)
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_double);
                else
                    AudioManager.Instance.PlaySound(LandlordsModel.woman_notDouble);
            }
            else
            {
                if (pro.AddTimes == 1)
                    AudioManager.Instance.PlaySound(LandlordsModel.man_double);
                else
                    AudioManager.Instance.PlaySound(LandlordsModel.man_notDouble);
            }
        }
        #endregion        
        #region 显示加倍状态
        if (pro.AddTimes != 255)
        {
            int index = ((int)pro.User - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            if (pro.User == GameModel.chairId)
            {
                btnNotDouble.gameObject.SetActive(false);
                btnDouble.gameObject.SetActive(false);
                CloseTimer();
            }
            if (pro.AddTimes == 1) //加倍
            {
                doubleFlags[index].SetActive(true);
            }
            else//不加倍
            {
                notDoubleFlags[index].SetActive(true);
            }
        }
        #endregion
        #region 显示庄家信息
        if (pro.CanOutCard == 1)//庄家可以开始出牌
        {
            for (int i = 0; i < 3; i++)
            {
                doubleFlags[i].SetActive(false);
                notDoubleFlags[i].SetActive(false);
            }
            if (LandlordsModel.bankerChairdId == GameModel.chairId)
            {
                btnOutCard.transform.localPosition = new Vector3(0f, -45f, 0f);
                btnOutCard.gameObject.SetActive(true);
            }
            for (int i = 0; i < 20; i++)
            {
                myHandCards[i].GetComponent<BoxCollider>().enabled = true;
            }
            int bankerIndex = (LandlordsModel.bankerChairdId - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            StartTimeCount(LandlordsModel.firstOutCardTime, bankerIndex, null);
        }
        #endregion
    }

    //用户出牌
    void OnUserOutCard(Bs.Gameddz.S_OutCard pro)
    {
        selectIndex = 0;    //重置提示选择下标
        //播放出牌音效
        PlayOutCardTypeAudioAndAnim((UInt16)pro.OutCardUser, (byte)pro.CardType, (byte)pro.CardData[0]);
        LandlordsModel.finallyOutCardType = (int)pro.CardType;
        //显示出牌玩家出的牌
        int outCardIndex = ((int)pro.OutCardUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;

        //Debug.Log("用户出牌,NextUserCanOutCard=" + pro.NextUserCanOutCard
        //    + ",OutCardUser=" + pro.OutCardUser
        //    + ",bankerChairdId=" + LandlordsModel.bankerChairdId
        //    + ",pro.CardData.Count=" + pro.CardData.Count
        //    + ",chairId=" + GameModel.chairId
        //    + ",outCardIndex=" + outCardIndex
        //    + ",NextUser=" + pro.NextUser
        //    );

        byte[] cbCardData = new byte[pro.CardData.Count];
        for(int i = 0; i < pro.CardData.Count; i ++)
        {
            cbCardData[i] = (byte)pro.CardData[i];
        }
        RefreshOutCard(outCardIndex, (int)pro.CardData.Count, cbCardData);
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i == GameModel.chairId)
                currentTotalScore.text = pro.Times[i].ToString();
        }
        #region 出牌是地主
        if (pro.OutCardUser == LandlordsModel.bankerChairdId)
        {
            for (int i = 0; i < pro.CardData.Count; i++)
            {
                outCards[outCardIndex, i].transform.Find("landlordsFlag").gameObject.SetActive(true);
            }
        }
        #endregion
        #region 出牌数据排序
        if (pro.CardData.Count > 1)
        {
            for (int i = 0; i < pro.CardData.Count; i++)
            {
                for (int j = i + 1; j < pro.CardData.Count; j++)
                {
                    if (LandlordsModel.CompareCard((byte)pro.CardData[i], (byte)pro.CardData[j]) < 0)
                    {
                        byte mid = (byte)pro.CardData[i];
                        pro.CardData[i] = pro.CardData[j];
                        pro.CardData[i] = mid;
                    }
                }
            }
        }
        #endregion
        #region 出牌玩家
        if (pro.OutCardUser == GameModel.chairId)
        {
            btnOutCard.gameObject.SetActive(false);
            btnCardTip.gameObject.SetActive(false);
            btnNotOutCard.gameObject.SetActive(false);
            for (int i = 0; i < pro.CardData.Count; i++)
            {
                handCardList.Remove((byte)pro.CardData[i]);
            }
            RefreshHandCard();
        }
        else
        {
            //保存最后一个玩家出牌数据
            beforeUserOutCardList.Clear();
            for (int i = 0; i < pro.CardData.Count; i++)
            {
                beforeUserOutCardList.Add((byte)pro.CardData[i]);
            }
            //刷新其他玩家的手牌数目
            byte[] cardCount = new byte[pro.RestCardCount.Count];
            for(int i = 0;i < pro.RestCardCount.Count; i ++)
            {
                cardCount[i] = (byte)pro.RestCardCount[i];
            }
            RefreshOtherHandCardNum(cardCount);
        }
        #endregion
        #region 下一个出牌的是我
        if (pro.NextUser != GameModel.INVALID_CHAIR)
        {
            CloseTimer();
            int nextOutCardIndex = ((int)pro.NextUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            //Debug.Log("用户出牌,nextOutCardIndex=" + nextOutCardIndex + ",outCardTime=" + LandlordsModel.outCardTime);
            if (pro.NextUser == GameModel.chairId)
            {
                if (pro.NextUserCanOutCard == 1)
                {
                    StartTimeCount(LandlordsModel.outCardTime, nextOutCardIndex, null);
                    btnOutCard.transform.localPosition = new Vector3(220f, -45f, 0f);
                    btnCardTip.transform.localPosition = new Vector3(0f, -45f, 0f);
                    btnNotOutCard.transform.localPosition = new Vector3(-220f, -45f, 0f);
                    btnOutCard.gameObject.SetActive(true);
                    btnCardTip.gameObject.SetActive(true);
                    btnNotOutCard.gameObject.SetActive(true);
                    //要的起 保存提示信息
                    tipCardList = new List<byte[]>();
                    for (int i = 0; i < pro.SearchCount; i++)
                    {
                        var cardInfo = pro.ResultCard[i];
                        byte[] temp = new byte[pro.SearchCardCount[i]];
                        for (int j = 0; j < pro.SearchCardCount[i]; j++)
                        {
                            temp[j] = (byte)cardInfo.Card[j];
                            //temp[j] = pro.cbResultCard[i, j];
                        }
                        tipCardList.Add(temp);
                    }
                }
                else
                {
                    StartTimeCount(LandlordsModel.canotAfford, nextOutCardIndex, null);
                    btnCannotAfford.gameObject.SetActive(true);
                }
            }
            else
            {
                StartTimeCount(LandlordsModel.outCardTime, nextOutCardIndex, null);
            }
            #region 倒计时、清除出的牌
            //清除出的牌
            for (int i = 0; i < 20; i++)
            {
                outCards[nextOutCardIndex, i].SetActive(false);
                if (notOutCardFlags[nextOutCardIndex].activeSelf)
                {
                    notOutCardFlags[nextOutCardIndex].SetActive(false);
                }
            }
            #endregion
        }
        #endregion
    }

    void OnOutCardFail(Bs.Gameddz.S_OutCardFail pro)
    {
        AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
        RefreshMyHandCardsPos();    //回复玩家的手牌位置
        Util.Instance.DoAction(GameEvent.V_OpenShortTip, pro.DescribeString);
    }

    //用户弃牌
    void OnPassCard(Bs.Gameddz.S_PassCard pro)
    {
        selectIndex = 0;    //重置提示选择下标
        #region 播放音效
        PlayerInRoom player = GameModel.GetDeskUser((int)pro.PassCardUser);
        if (player != null)
        {
            if (player.cbGender == 0)
            {
                AudioManager.Instance.PlaySound(LandlordsModel.woman_pass[UnityEngine.Random.Range(0, 4)]);
            }
            else
            {
                AudioManager.Instance.PlaySound(LandlordsModel.man_pass[UnityEngine.Random.Range(0, 4)]);
            }
        }
        #endregion
        #region  显示不出标志
        int index = ((int)pro.PassCardUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
        notOutCardFlags[index].SetActive(true);
        if (pro.PassCardUser == GameModel.chairId)
        {
            RefreshMyHandCardsPos();
            btnCannotAfford.gameObject.SetActive(false);
            btnOutCard.gameObject.SetActive(false);
            btnCardTip.gameObject.SetActive(false);
            btnNotOutCard.gameObject.SetActive(false);
        }
        #endregion
        #region 下一个操作玩家
        CloseTimer();
        int nextOutCardIndex = ((int)pro.NextUser - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;

        //Debug.Log("用户弃牌,PassCardUser=" + pro.PassCardUser
        //    + ",chairId=" + GameModel.chairId
        //    + ",NextUserCanOutCard=" + pro.NextUserCanOutCard
        //    + ",TurnOver=" + pro.TurnOver
        //    + ",SearchCount=" + pro.SearchCount
        //    + ",player.null=" + (player == null)
        //    + ",index=" + index
        //    + ",nextOutCardIndex=" + nextOutCardIndex
        //    + ",outCardTime=" + LandlordsModel.outCardTime
        //    );

        if (pro.NextUser == GameModel.chairId)
        {
            if (pro.NextUserCanOutCard == 1)//要的起
            {
                StartTimeCount(LandlordsModel.outCardTime, nextOutCardIndex, null);
                btnOutCard.gameObject.SetActive(true);
                if (pro.TurnOver != 1)
                {
                    btnNotOutCard.gameObject.SetActive(pro.TurnOver != 1);
                    btnCardTip.gameObject.SetActive(true);
                    btnOutCard.transform.localPosition = new Vector3(220f, -45f, 0f);
                    btnCardTip.transform.localPosition = new Vector3(0f, -45f, 0f);
                    btnNotOutCard.transform.localPosition = new Vector3(-220f, -45f, 0f);
                }
                else
                {
                    btnOutCard.transform.localPosition = new Vector3(0f, -45f, 0f);
                }
                //要的起 保存提示信息
                tipCardList = new List<byte[]>();
                for (int i = 0; i < pro.SearchCount; i++)
                {
                    var cardInfo = pro.ResultCard[i];
                    byte[] temp = new byte[pro.SearchCardCount[i]];
                    for (int j = 0; j < pro.SearchCardCount[i]; j++)
                    {
                        temp[j] = (byte)cardInfo.Card[j];
                        //temp[j] = pro.cbResultCard[i, j];
                    }
                    tipCardList.Add(temp);
                }
            }
            else//要不起
            {
                StartTimeCount(LandlordsModel.canotAfford, nextOutCardIndex, null);
                btnCannotAfford.gameObject.SetActive(true);
            }
        }
        else
        {
            StartTimeCount(LandlordsModel.outCardTime, nextOutCardIndex, null);
        }
        #endregion
        #region 清除下操作个玩家显示、倒计时

        //清除出的牌
        for (int i = 0; i < 20; i++)
        {
            outCards[nextOutCardIndex, i].SetActive(false);
            if (notOutCardFlags[nextOutCardIndex].activeSelf)
            {
                notOutCardFlags[nextOutCardIndex].SetActive(false);
            }
        }
        #endregion
    }

    float waitTime = 0;// 动画等待时间
    //游戏结束
    void OnGameEnd(Bs.Gameddz.S_GameConclude pro)
    {
        #region 关闭游戏UI
        CloseTimer();
        CloseGameUI();
        for (int i = 0; i < 20; i++)
        {
            myHandCards[i].GetComponent<BoxCollider>().enabled = false;
        }
        #endregion
        #region 保存数据
        Landlords3Result res = new Landlords3Result();
        res.chairId = GameModel.chairId;
        for (int i = 0; i < 3; i++)
        {
            PlayerInRoom player = GameModel.GetDeskUser(i);
            res.userPhotos[i] = GameModel.GetUserPhoto(i);
            if (player != null)
            {
                res.gameId[i] = (int)player.dwGameID;
                res.userName[i] = player.nickName;
            }
            if (player.wChairID == LandlordsModel.bankerChairdId)
            {
                res.isBankerId[i] = 1;
            }
        }
        for(int i = 0; i < pro.GameScore.Count; i ++)
        {
            res.userScore[i] = pro.GameScore[i];
            Debug.Log("游戏结束,GameScore=" + pro.GameScore[i]);
        }
        #endregion
        #region 统计时间
        waitTime = 0;
        if (LandlordsModel.finallyOutCardType == 3 || LandlordsModel.finallyOutCardType == 7 || LandlordsModel.finallyOutCardType == 10 ||
            LandlordsModel.finallyOutCardType == 11 || LandlordsModel.finallyOutCardType == 12)
        {
            switch (LandlordsModel.finallyOutCardType)
            {
                case 3:
                    waitTime += 1.2f;
                    break;
                case 7:
                    waitTime += 1.2f;
                    break;
                case 10:
                    waitTime += 2.06f;
                    break;
                case 11:
                    waitTime += 1.11f;
                    break;
                case 12:
                    waitTime += 2.2f;
                    break;
                default:
                    break;
            }
        }
        if (pro.ChunTian == 1 || pro.FanChunTian == 1)
        {
            waitTime += 1f;
        }
        waitTime += 3f;
        #endregion
        if (LandlordsEvent.V_GameOver != null)
        {
            LandlordsEvent.V_GameOver.Invoke(res, waitTime);
        }
        //刷新得分
        DoActionDelay(GameEvent.V_RefreshUserInfo, waitTime, true);
        StartCoroutine(PlayGameEndAnim(pro));
    }

    //用户托管
    void OnUserTrustee(Bs.Gameddz.S_TRUSTEE pro)
    {
        if (pro.TrusteeUser == GameModel.chairId)
        {
            btnTrustee.gameObject.SetActive(LandlordsModel.isPlayerTrustee[GameModel.chairId]);
            if (LandlordsModel.isPlayerTrustee[GameModel.chairId])
            {
                isAuto = false;
            }
            else
            {
                isAuto = true;
            }
            RefreshAutoButton();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            int index = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
            autoFlags[index].SetActive(LandlordsModel.isPlayerTrustee[i]);
        }
    }
    #endregion

    #region 计时器

    int timeIndex = -1;

    /// 开始计时
    void StartTimeCount(int time, int index, UnityAction callBack)
    {
        if (!timeCount[index].activeSelf)
        {
            timeCount[index].SetActive(true);
        }
        currentTimer = time;
        timeOutCallBack = callBack;
        timeIndex = index;

        CancelInvoke("TimeCount");
        InvokeRepeating("TimeCount", 0f, 1f);
    }

    //停止计时器
    void StopTimeOutCallBack()
    {
        timeOutCallBack = null;
    }

    //关闭计时器
    void CloseTimer()
    {
        for (int i = 0; i < 3; i++)
        {
            timeCount[i].SetActive(false);
        }
        timeOutCallBack = null;
        CancelInvoke("TimeCount");
    }

    //计时方法
    void TimeCount()
    {
        timeCount[timeIndex].transform.Find("lblTime").GetComponent<UILabel>().text = currentTimer.ToString();
        currentTimer--;
        if (currentTimer <= 5)
        {
            if (timeIndex == 0)
                AudioManager.Instance.PlaySound(LandlordsModel.audioTimer);
        }
        if (currentTimer < 0)
        {
            timeCount[timeIndex].transform.Find("lblTime").GetComponent<UILabel>().text = "0";
            CancelInvoke("TimeCount");
            DoAction(timeOutCallBack);
        }
    }

    #endregion

    #region UI操作

    //游戏开始动画
    IEnumerator PlayGameStartAnim()
    {
        //1.发牌
        for (int j = 0; j < LandlordsModel.NORMAL_HANDCARDNUM; j++)
        {
            AudioManager.Instance.PlaySound(LandlordsModel.audioSendCard);
            myHandCards[j].gameObject.SetActive(true);
            myHandCards[j].transform.localPosition += new Vector3(0f, 5f, 0f);
            myHandCards[j].transform.DOLocalMoveY(0f, 0.1f);
            myHandCards[j].GetComponent<UISprite>().spriteName = LandlordsModel.GetPokerName(handCardList[j]);
            //显示其他玩家的手牌数目
            for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
            {
                int cardNumIndex = (i - GameModel.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
                if (cardNumIndex != 0)//显示别人剩余多少张牌
                {
                    userCardCount[cardNumIndex].SetActive(true);
                    userCardCount[cardNumIndex].GetComponent<UILabel>().text = (j + 1).ToString();
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.1f);
        //2.重排手牌
        //AudioManager.Instance.PlaySound(ShiSanShuiModel8.audioSortCard);
        RefreshHandCard();
        yield return new WaitForSeconds(0.3f);
        if (LandlordsModel.currentUser == GameModel.chairId)
        {
            btnCallLandlord.gameObject.SetActive(true);
            btnNotCall.gameObject.SetActive(true);
        }
        //3.开始计时
        int StartIndex = (LandlordsModel.currentUser - GameModel.chairId + 3) % 3;
        timeCount[StartIndex].SetActive(true);
        StartTimeCount(LandlordsModel.callTime, StartIndex, null);
    }

    //地主增加牌动画
    IEnumerator PlayBankerSendCardAnim()
    {
        //1.发牌
        for (int j = 17; j < 20; j++)
        {
            AudioManager.Instance.PlaySound(LandlordsModel.audioSendCard);
            myHandCards[j].gameObject.SetActive(true);
            myHandCards[j].transform.localPosition += new Vector3(0f, 5f, 0f);
            myHandCards[j].transform.DOLocalMoveY(0f, 0.1f);
            myHandCards[j].GetComponent<UISprite>().spriteName = LandlordsModel.GetPokerName(handCardList[j]);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.1f);
        myHandCards[19].transform.Find("landlordPokerFlag").gameObject.SetActive(true);

        //2.重排手牌
        //AudioManager.Instance.PlaySound(ShiSanShuiModel8.audioSortCard);
        RefreshHandCard();
        yield return new WaitForSeconds(0.3f);
    }


    //游戏结束动画
    IEnumerator PlayGameEndAnim(Bs.Gameddz.S_GameConclude pro)
    {
        Landlords3Result res = new Landlords3Result();
        res.chairId = GameModel.chairId;
        #region 延时最后出特殊牌型动画时间、春天反春天时间
        if (LandlordsModel.finallyOutCardType == 3 || LandlordsModel.finallyOutCardType == 7 || LandlordsModel.finallyOutCardType == 10 ||
            LandlordsModel.finallyOutCardType == 11 || LandlordsModel.finallyOutCardType == 12)
        {
            switch (LandlordsModel.finallyOutCardType)
            {
                case 3:
                    yield return new WaitForSeconds(1.2f);
                    break;
                case 7:
                    yield return new WaitForSeconds(1.2f);
                    break;
                case 10:
                    yield return new WaitForSeconds(2.06f);
                    break;
                case 11:
                    yield return new WaitForSeconds(1.11f);
                    break;
                case 12:
                    yield return new WaitForSeconds(2.2f);
                    break;
                default:
                    break;
            }
        }
        if (pro.ChunTian == 1 || pro.FanChunTian == 1)
        {
            chunTianFlag.SetActive(pro.ChunTian == 1);
            fanChunTianFlag.SetActive(pro.FanChunTian == 1);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        CloseCardTypeFlag();
        #endregion        
        #region  显示所有玩家剩下的手牌
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            if (i != res.chairId)
            {
                if (pro.CardCount[i] != 255 && pro.CardCount[i] != 0)
                {
                    //显示出牌玩家出的牌
                    int outCardIndex = (i - res.chairId + LandlordsModel.GAME_NUM) % LandlordsModel.GAME_NUM;
                    byte[] cardData = new byte[pro.CardCount[i]];
                    var cardInfo = pro.CardData[i];
                    for (int j = 0; j < pro.CardCount[i]; j++)
                    {
                        byte card = (byte)cardInfo.Card[j];
                        if (card != 0)
                        {
                            cardData[j] = card;
                        }
                    }
                    RefreshOutCard(outCardIndex, (int)pro.CardCount[i], cardData);
                    if (i == LandlordsModel.bankerChairdId)   //显示地主出牌的标志
                    {
                        for (int j = 0; j < pro.CardCount[i]; j++)
                        {
                            outCards[outCardIndex, j].transform.Find("landlordsFlag").gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.2f);
        #endregion
        #region 显示输赢
        for (int i = 0; i < 3; i++)
        {
            int chairId = (res.chairId + i) % 3;
            //显示输赢分数
            if (pro.GameScore[chairId] > 0)
            {
                lblWinScore[i].gameObject.SetActive(true);
                lblWinScore[i].text = "+" + pro.GameScore[chairId].ToString();
                //lblWinScore[i].GetComponent<TweenAlpha>().PlayForward();
                lblWinScore[i].GetComponent<TweenPosition>().PlayForward();
            }
            if (pro.GameScore[chairId] < 0)
            {
                lblLoseScore[i].gameObject.SetActive(true);
                lblLoseScore[i].text = pro.GameScore[chairId].ToString();
                //lblLoseScore[i].GetComponent<TweenAlpha>().PlayForward();
                lblLoseScore[i].GetComponent<TweenPosition>().PlayForward();
            }
        }
        if (pro.GameScore[res.chairId] >= 0)
        {
            AudioManager.Instance.PlaySound(LandlordsModel.audioGameWin);
        }
        else
        {
            AudioManager.Instance.PlaySound(LandlordsModel.audioGameFail);
        }
        yield return new WaitForSeconds(0.3f);
        #endregion
        #region 显示输赢标志
        if (!isHaveRedPack)
        {
            if (pro.GameScore[res.chairId] > 0)
            {
                if (LandlordsModel.bankerChairdId == res.chairId)
                {
                    winFlag.GetComponent<UISprite>().spriteName = "landlordWinFlag";
                }
                else
                {
                    winFlag.GetComponent<UISprite>().spriteName = "husbandmanWinFlag";
                }
                winFlag.SetActive(true);
                winFlag.GetComponent<TweenPosition>().PlayForward();
                winFlag.GetComponent<TweenAlpha>().PlayForward();
                winFlag.transform.Find("Sprite").GetComponent<TweenScale>().PlayForward();
            }
            if (pro.GameScore[res.chairId] < 0)
            {
                if (LandlordsModel.bankerChairdId == res.chairId)
                {
                    lostFlag.GetComponent<UISprite>().spriteName = "landlordLostFlag";
                }
                else
                {
                    lostFlag.GetComponent<UISprite>().spriteName = "husbandmanLostFlag";
                }
                lostFlag.SetActive(true);
                lostFlag.GetComponent<TweenPosition>().PlayForward();
                lostFlag.GetComponent<TweenAlpha>().PlayForward();
                lostFlag.transform.Find("Sprite").GetComponent<TweenScale>().PlayForward();
            }
        }
        isHaveRedPack = false;
        yield return new WaitForSeconds(1.5f);
        winFlag.SetActive(false);
        lostFlag.SetActive(false);
        winFlag.GetComponent<TweenPosition>().PlayReverse();
        winFlag.GetComponent<TweenAlpha>().PlayReverse();
        lostFlag.GetComponent<TweenPosition>().PlayReverse();
        lostFlag.GetComponent<TweenAlpha>().PlayReverse();
        winFlag.transform.Find("Sprite").GetComponent<TweenScale>().PlayReverse();
        lostFlag.transform.Find("Sprite").GetComponent<TweenScale>().PlayReverse();
        #endregion        
    }

    #endregion

    #region  超时处理

    //抢地主超时
    void GardLandlordTimeOut()
    {
        CloseTimer();
        LandlordsService.Instance.C2S_CallScore(0);
    }

    //加倍超时处理
    void AddTimesTimeOut()
    {
        CloseTimer();
        LandlordsService.Instance.C2S_AddTime(0);
    }

    #endregion
}

public class Landlords3Result
{
    public int chairId;
    public int[] isBankerId = new int[3];//是否是地主

    public int[] gameId = new int[3];
    public string[] userName = new string[3];
    public Texture[] userPhotos = new Texture[3];

    public Int64[] totalScore = new Int64[3];       //玩家总分(房卡游戏)
    public int[] winCount = new int[3];             //赢次数 

    public Int64[] userScore = new Int64[3];		//用户得分
}
