using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class LandlordsPnlMain : View
{
    #region 变量声明

    //界面按钮
    private UIButton btnMenu, btnCloseMenu, btnDisRoom, btnReturn, btnSet, btnRule, btnChat, btnRedRule, btnRedPack,btnDigTreasure;
    private UILabel lblRedPackInfo;
    private UILabel lblDigTreasureInfo;
    private GameObject btnVoice;
    private TweenScale menu;
    private Transform[] users = new Transform[3];
    //聊天
    private Transform[] chatText = new Transform[3];
    private Transform[] chatAudio = new Transform[3];
    private Transform[] chatEmoji = new Transform[3];

    //roomInfo 
    private UILabel lblCurrentTime, lblRoomID, lblNumberOfGame;
    private UISlider sliderBattery;
    private GameObject[] signals = new GameObject[4];

    private UILabel[] lblMinute = new UILabel[3];
    private UILabel[] lblCoin = new UILabel[3];
    private UILabel[] lblRedPacket = new UILabel[3];
    private UILabel[] lblDiamond = new UILabel[3];
    

    #endregion

    public override void Init()
    {
        btnMenu = transform.Find("bg/btnMenu").GetComponent<UIButton>();
        btnCloseMenu = transform.Find("bg/btnMenu/Panel/menu/btnCloseMenu").GetComponent<UIButton>();
        btnDisRoom = transform.Find("bg/btnMenu/Panel/menu/btnDisRoom").GetComponent<UIButton>();
        btnChat = transform.Find("bg/btnChat").GetComponent<UIButton>();
        
        btnRedRule = transform.Find("bg/btnRedRule").GetComponent<UIButton>();
        btnRedPack = transform.Find("bg/btnRedPack").GetComponent<UIButton>();
        btnDigTreasure = transform.Find("bg/btnDigTreasure").GetComponent<UIButton>();
        lblRedPackInfo = transform.Find("bg/btnRedPack/Label").GetComponent<UILabel>();
        lblDigTreasureInfo = transform.Find("bg/btnDigTreasure/Label").GetComponent<UILabel>();
        btnVoice = transform.Find("bg/btnVoice").gameObject;

        menu = transform.Find("bg/btnMenu/Panel/menu").GetComponent<TweenScale>();
        btnReturn = transform.Find("bg/btnMenu/Panel/menu/btnReturn").GetComponent<UIButton>();
        btnSet = transform.Find("bg/btnMenu/Panel/menu/btnSet").GetComponent<UIButton>();
        btnRule = transform.Find("bg/btnMenu/Panel/menu/btnRule").GetComponent<UIButton>();
        for (int i = 0; i < 4; i++)
        {
            if (i < 3)
            {
                users[i] = transform.Find("bg/userInfo_" + i);
                lblMinute[i] = transform.Find("bg/userInfo_" + i + "/lblMinute").GetComponent<UILabel>();
                lblCoin[i] = transform.Find("bg/userInfo_" + i + "/lblCoin").GetComponent<UILabel>();
                lblRedPacket[i] = transform.Find("bg/userInfo_" + i + "/lblRedPacket").GetComponent<UILabel>();
                lblDiamond[i] = transform.Find("bg/userInfo_" + i + "/lblDiamond").GetComponent<UILabel>();

                chatText[i] = transform.Find("bg/pnlTip/lblChat_" + i);
                chatAudio[i] = transform.Find("bg/pnlTip/audioFlag_" + i);
                chatEmoji[i] = transform.Find("bg/pnlTip/chatEmoji_" + i);
            }            

            signals[i] = transform.Find("bg/phoneInfo/signalBg/signal_" + i).gameObject;
        }

        lblCurrentTime = transform.Find("bg/phoneInfo/phoneTime").GetComponent<UILabel>();
        lblRoomID = transform.Find("bg/phoneInfo/lblRoomId").GetComponent<UILabel>();
        lblNumberOfGame = transform.Find("bg/roomInfo/lblNumberOfGame").GetComponent<UILabel>();
        sliderBattery = transform.Find("bg/phoneInfo/Battery/sliderBattery").GetComponent<UISlider>();
                
        //lblTimer = transform.FindChild("bg/timeCount/lblTime").GetComponent<UILabel>();
        //lblTimeSign = transform.FindChild("bg/timeCount/lblTimeSign").GetComponent<UILabel>();
        //添加监听        
        EventDelegate.Add(btnMenu.onClick, OnBtnMenuClick);
        EventDelegate.Add(btnCloseMenu.onClick, OnBtnCloseMenuClick);
        EventDelegate.Add(btnDisRoom.onClick, OnBtnDisRoomClick);
        EventDelegate.Add(btnReturn.onClick, OnBtnReturnClick);
        EventDelegate.Add(btnSet.onClick, OnBtnSetClick);
        EventDelegate.Add(btnRule.onClick, OnBtnRuleClick);
        EventDelegate.Add(btnChat.onClick, OnBtnChatClick);
        
        EventDelegate.Add(btnRedRule.onClick, OnBtnRedRuleClick);
        EventDelegate.Add(btnRedPack.onClick, OnBtnRedPackClick);
        EventDelegate.Add(btnDigTreasure.onClick, OnBtnDigTreasureClick);
        UIEventListener.Get(btnVoice).onPress = OnBtnAudioPress;

        InitGameScene();
        RefreshRedPackState();
        RefreshDigTreasureState();
        InvokeRepeating("RefreshRoomInfo", 0.3f, 5);
    }

    //初始化
    void InitGameScene()
    {
        //初始化
        gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
        menu.ResetToBeginning();
        if (GameModel.serverType == GameModel.ServerKind_Private)
        {
            btnDisRoom.gameObject.SetActive(true);
            btnReturn.transform.localPosition = new Vector3(0f, -247f, 0f);
            menu.GetComponent<UISprite>().height = 285;
        }
        else
        {
            btnDisRoom.gameObject.SetActive(false);
            btnReturn.transform.localPosition = new Vector3(0f, -175f, 0f);
            menu.GetComponent<UISprite>().height = 210;
        }
        for (int i = 0; i < 3; i++)
        {
            chatText[i].gameObject.SetActive(true);
            chatEmoji[i].gameObject.SetActive(true);
            chatAudio[i].gameObject.SetActive(true);
            chatText[i].GetComponent<TweenScale>().ResetToBeginning();
            chatText[i].GetComponent<TweenAlpha>().ResetToBeginning();
            chatEmoji[i].GetComponent<TweenAlpha>().ResetToBeginning();
            chatAudio[i].GetComponent<TweenScale>().ResetToBeginning();
            chatAudio[i].GetComponent<TweenAlpha>().ResetToBeginning();

            
            lblMinute[i].gameObject.SetActive(false);
            lblCoin[i].gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_Gold);
            //lblRedPacket[i].gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_RedPack);
            lblRedPacket[i].gameObject.SetActive(false);
            //只显示自己
            if (GameModel.serverType == GameModel.ServerKind_RedPack && i == 0)
                lblRedPacket[i].gameObject.SetActive(true);
            lblDiamond[i].gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_RedPack);
        }
        lblNumberOfGame.gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_Private);
        btnRedPack.gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_RedPack);
        btnDigTreasure.gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_Gold);
        btnRedRule.gameObject.SetActive(false);

        //自适应
        chatText[0].localPosition = new Vector3(-AppConfig.screenWidth/2 + 140f, -263f, 0f);
        chatText[1].localPosition = new Vector3(AppConfig.screenWidth/2 - 160f, 92f, 0f);
        chatText[2].localPosition = new Vector3(-AppConfig.screenWidth/2 + 160f, 92f, 0f);
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenRedPackButton += OpenRedPackButton;
        GameEvent.V_RefreshRedPackState += RefreshRedPackState;
        GameEvent.V_RefreshDigTreasureState += RefreshDigTreasureState;

        GameEvent.V_RefreshRoomInfo += RefreshRoomInfo;
        GameEvent.V_RefreshUserInfo += RefreshUserInfo;
        GameEvent.S_ReceiveChatMessage += OnReceiveChatMessage;
    }
     
    public override void RemoveAction()
    {
        GameEvent.V_OpenRedPackButton -= OpenRedPackButton;
        GameEvent.V_RefreshRedPackState -= RefreshRedPackState;
        GameEvent.V_RefreshDigTreasureState -= RefreshDigTreasureState;

        GameEvent.V_RefreshRoomInfo -= RefreshRoomInfo;
        GameEvent.V_RefreshUserInfo -= RefreshUserInfo;
        GameEvent.S_ReceiveChatMessage -= OnReceiveChatMessage;

        
    }

    #region UI响应
    //打开菜单
    private void OnBtnMenuClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        if (menu.transform.localScale.y < 0.2f)
        {
            menu.PlayForward();
        }
        btnMenu.GetComponent<UISprite>().enabled = false;
    }

    //关闭菜单
    public void OnBtnCloseMenuClick()
    {
        if (menu.transform.localScale.y > 0.8f)
        {
            menu.PlayReverse();
        }
        btnMenu.GetComponent<UISprite>().enabled = true;
    }

    //返回
    public void OnBtnReturnClick()
    {
        //座位号判断 无效但有可能是等待分配状态


        //游戏状态
        if (GameModel.isInGame && (GameModel.chairId != 65535 && LandlordsModel.playerInGame[GameModel.chairId] == 1))
        {
            //游戏已经开始，不能退出
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            if(GameModel.serverType == GameModel.ServerKind_RedPack)            //红包场
            {
                string message = "离开后将由系统代出,若不及时返回将中断红包进度!";
                GameEvent.V_OpenDlgTip.Invoke(message, null, delegate { GameService.Instance.ReturnToHall(); }, null);
            }
            else if (GameModel.serverType == GameModel.ServerKind_Gold)             //金币场
            {
                string message = "离开后将由系统代出!";
                GameEvent.V_OpenDlgTip.Invoke(message, null, delegate { GameService.Instance.ReturnToHall(); }, null);
            }
            else
            {
                DoAction(GameEvent.V_OpenShortTip, "游戏已经开始，不能离开房间！");
            }
        }
        else if (GameModel.serverType == GameModel.ServerKind_Private && GameModel.currentGameCount > 0)
        {
            //房卡模式，已经开始
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "游戏已经开始，不能离开，请先解散房间！");
        }
        else
        {
            AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
            //红包场判断
            if(GameModel.serverType == GameModel.ServerKind_RedPack)
            {
                GameEvent.V_OpenLeaveGame.Invoke();
            }
            else
            {
                GameService.Instance.UserStand();
            }
        }
    }

    //解散房间
    public void OnBtnDisRoomClick()
    {
        if (GameModel.serverType != GameModel.ServerKind_Private)
        {
            OnBtnReturnClick();
        }
        else
        {
            //游戏尚未开始，解散房间
            if (!GameModel.isInGame)
            {
                if (GameModel.currentGameCount < 1)
                {
                    if (HallModel.userId == GameModel.hostUserId)
                    {
                        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
                        if (GameEvent.V_OpenDlgTip != null)
                        {
                            GameEvent.V_OpenDlgTip.Invoke("游戏尚未开始，是否立即解散？", "", GameService.Instance.DisRoomBeforeGame, null);
                        }
                        return;
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
                        DoAction(GameEvent.V_OpenShortTip, "游戏尚未开始，只有房主才能解散房间！");
                        return;
                    }
                }
                else
                {
                    AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
                    if (GameEvent.V_OpenDlgTip != null)
                    {
                        GameEvent.V_OpenDlgTip.Invoke("游戏已经开始，需要所有玩家同意才能解散，是否开始投票？", "", GameService.Instance.DisRoomInGame, null);
                    }
                    return;
                }
            }
            else
            {
                AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
                if (GameEvent.V_OpenDlgTip != null)
                {
                    GameEvent.V_OpenDlgTip.Invoke("游戏已经开始，需要所有玩家同意才能解散，是否开始投票？", "", GameService.Instance.DisRoomInGame, null);
                }
                return;
            }
        }
    }

    //设置
    public void OnBtnSetClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(GameEvent.V_OpenDlgSet);
    }

    //规则按钮
    public void OnBtnRuleClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(GameEvent.V_OpenDlgRule);
    }

    private void OnBtnChatClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(GameEvent.V_OpenDlgChat);
    }

    //红包场规则
    public void OnBtnRedRuleClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        DoAction(GameEvent.V_OpenDlgRedRule);
    }

    //音量键按下
    private float pressTimer = 0;
    public void OnBtnAudioPress(GameObject obj, bool isPress)
    {
        if (isPress)
        {
            DoAction(GameEvent.V_OpenFlagAudio);
            pressTimer = Time.time;
            Invoke("PlayMicroTip", 0.3f);
        }
        else
        {
            DoAction(GameEvent.V_CloseFlagAudio);
            float timer = Time.time - pressTimer;
            if (IsInvoking("PlayMicroTip"))
            {
                CancelInvoke("PlayMicroTip");
            }
            if (timer > 0.3f)
            {
                MicroPhoneManager.Instance.StopRecord();
                byte[] data = MicroPhoneManager.Instance.GetAudioData();
                GameService.Instance.SendAudioMessage(data);
            }
            else
            {
                DoAction(GameEvent.V_OpenShortTip, "录音时间太短");
            }
        }
    }

    void PlayMicroTip()
    {
        AudioManager.Instance.PlaySound(GameModel.audioMicroClip);
        MicroPhoneManager.Instance.StartRecord();
    }

    #endregion

    #region 红包

    void OpenRedPackButton(CMD_Game_S_GetRedPack pro)
    {
        bool isEnable = (pro.dwUserID[GameModel.chairId] == HallModel.userId);
        btnRedPack.GetComponent<TweenScale>().enabled = isEnable;

        //非0既请求，1首局赠送 ...
        if(pro.cbSendType != 0)
        {
            GameService.Instance.OpenRedPack();
        }
    }

    void RefreshRedPackState()
    {
        lblRedPackInfo.text = GameModel.currentRedPackCount + "/" + GameModel.totalRedPackCount;
    }

    void RefreshDigTreasureState()
    {
        lblDigTreasureInfo.text = HallModel.curPlayCount + "/" + HallModel.totalPlayCount;
    }

    //开红包
    void OnBtnRedPackClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (btnRedPack.GetComponent<TweenScale>().enabled)
        {
            btnRedPack.GetComponent<TweenScale>().enabled = false;
            GameService.Instance.OpenRedPack();
        }
        else
        {
            DoAction(GameEvent.V_OpenShortTip, GameModel.redPackDescribe);
        }
    }

    /// <summary>
    /// 打开挖宝
    /// </summary>
    void OnBtnDigTreasureClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        DoAction(GameEvent.V_OpenDlgDigTreasure, null);
    }

    #endregion


    #region 刷新方法
    void RefreshRoomInfo()
    {
        lblCurrentTime.text = DateTime.Now.ToString("HH:mm");
        //房卡类型
        if (GameModel.serverType == GameModel.ServerKind_Private)
        {
            lblRoomID.text = "房间ID:"+GameModel.currentRoomId.ToString();
            lblNumberOfGame.text = "局数:"+GameModel.currentGameCount + "/" + GameModel.totalGameCount;
        }
        else
        {
            //lblRoomID.text = HallModel.currentRoomName;
            lblRoomID.text = "";
        }
        sliderBattery.value = Util.GetBatteryLevel();
        for (int i = 0; i < 4; i++)
        {
            if (i <= GameModel.netSpeed)
            {
                signals[i].SetActive(true);
            }
            else
            {
                signals[i].SetActive(false);
            }
        }
    }

    //刷新玩家信息显示
    public void RefreshUserInfo(bool isRefreshScore)
    {
        if (GameModel.chairId == GameModel.INVALID_CHAIR)
        {
            if (GameModel.playerInRoom.ContainsKey((uint)HallModel.userId))
            {
                GameModel.chairId = GameModel.playerInRoom[(uint)HallModel.userId].wChairID;
                Debug.Log("刷新玩家信息显示,设置椅子,chairId=" + GameModel.chairId + ",isRefreshScore=" + isRefreshScore);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            int chairId = (GameModel.chairId + i) % 3;
            PlayerInRoom player = GameModel.GetDeskUser(chairId);

            if (player == null || GameModel.chairId == GameModel.INVALID_CHAIR)
            {
                //Debug.LogError("刷新玩家信息显示,未找到,chairId=" + GameModel.chairId + ",chairId=" + chairId + ",player.null="+ (player == null) + ",isRefreshScore=" + isRefreshScore);
                users[i].Find("userPhoto").GetComponent<UITexture>().mainTexture = HallModel.defaultPhoto;
                users[i].Find("lblUserName").GetComponent<UILabel>().text = "等待加入...";
                users[i].Find("sptIsOffline").gameObject.SetActive(false);

                lblDiamond[i].text = "0";
                lblRedPacket[i].text = "0";
                lblCoin[i].text = "0";
                lblMinute[i].gameObject.SetActive(false);
            }
            else
            {
                users[i].Find("userPhoto").GetComponent<UITexture>().mainTexture = GameModel.GetUserPhoto(chairId);
                string name = player.nickName;
                if (i != 0 && name.Length > 5)
                {
                    name = name.Remove(4);
                    name = name + "..";
                }                
                users[i].Find("lblUserName").GetComponent<UILabel>().text = name;
                users[i].Find("sptIsOffline").gameObject.SetActive(player.cbUserStatus == UserState.US_OFFLINE || LandlordsModel.isPlayerTrustee[chairId]);
                lblRedPacket[i].text = player.redPack.ToString();
                if (isRefreshScore)
                {
                    lblDiamond[i].text = player.lIngot.ToString();
                    lblCoin[i].text = player.lScore.ToString();
                    if (GameModel.serverType == GameModel.ServerKind_Private)
                    {
                        if (player != null)
                        {
                            lblMinute[i].gameObject.SetActive(true);
                            if (player != null)
                            {
                                lblMinute[i].text = player.lScore.ToString();
                            }
                        }
                    }
                }
            }
        }
    }

    //收到聊天消息
    public void OnReceiveChatMessage(ChatMessage message)
    {
        int index = (message.chairId - GameModel.chairId + 3) % 3;
        switch (message.type)
        {
            case ChatMessageType.Audio:
                chatAudio[index].GetComponent<TweenScale>().ResetToBeginning();
                chatAudio[index].GetComponent<TweenAlpha>().ResetToBeginning();
                chatAudio[index].GetComponent<TweenScale>().PlayForward();
                chatAudio[index].GetComponent<TweenAlpha>().PlayForward();
                AudioManager.Instance.PlayLanguage(message.clip);
                float audioLength = message.clip.length;
                break;
            case ChatMessageType.Emoji:
                int emojiIndex = int.Parse(message.message);
                chatEmoji[index].GetComponent<TweenAlpha>().ResetToBeginning();
                chatEmoji[index].GetComponent<TweenAlpha>().PlayForward();
                chatEmoji[index].GetComponent<UI2DSpriteAnimation>().Pause();
                chatEmoji[index].GetComponent<UI2DSpriteAnimation>().frames = GameModel.GetEmojiSprites(emojiIndex);
                chatEmoji[index].GetComponent<UI2DSprite>().sprite2D = GameModel.GetEmojiSprites(emojiIndex)[2];
                chatEmoji[index].GetComponent<UI2DSprite>().MakePixelPerfect();
                chatEmoji[index].GetComponent<UI2DSpriteAnimation>().ResetToBeginning();
                chatEmoji[index].GetComponent<UI2DSpriteAnimation>().Play();
                break;
            case ChatMessageType.Shorter:
                int shortIndex = 0;
                if (int.TryParse(message.message, out shortIndex))
                {
                    if (shortIndex < 0 || shortIndex > 11) { return; }
                    if (message.sex == 0)
                    {
                        message.clip = GameModel.chatWomen[shortIndex];
                    }
                    else if (message.sex == 1)
                    {
                        message.clip = GameModel.chatMen[shortIndex];
                    }
                    message.message = GameModel.chatMessage[shortIndex];
                }
                AudioManager.Instance.PlaySound(message.clip);
                chatText[index].GetComponent<UILabel>().text = message.message;
                chatText[index].GetComponent<TweenScale>().ResetToBeginning();
                chatText[index].GetComponent<TweenAlpha>().ResetToBeginning();
                chatText[index].GetComponent<TweenScale>().PlayForward();
                chatText[index].GetComponent<TweenAlpha>().PlayForward();
                break;
            case ChatMessageType.Text:
                chatText[index].GetComponent<UILabel>().text = message.message;
                chatText[index].GetComponent<TweenScale>().ResetToBeginning();
                chatText[index].GetComponent<TweenAlpha>().ResetToBeginning();
                chatText[index].GetComponent<TweenScale>().PlayForward();
                chatText[index].GetComponent<TweenAlpha>().PlayForward();
                break;
        }
    }

    #endregion

}
