using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class DlgChat : View
{
    private static int MaxChatItemCount = 30;           //显示最大聊天记录条数

    private Transform bg;

    private UIInput inputMessage;
    private UIButton btnClose;
    private UIButton btnSend;

    private UIButton btnEmoji;
    private UIButton btnShorter;
    private UIButton btnChat;

    private GameObject pnlShorter;
    private GameObject pnlEmoji;
    private GameObject pnlChat;

    private GameObject[] shorter = new GameObject[12];
    private GameObject[] emoji = new GameObject[9];

    private Transform otherChatItem;    //其他玩家聊天内容
    private Transform myChatItem;       //自己聊天内容

    private GameObject flagAudio;
    private UISprite sptAudio;


    private List<Transform> itemList = new List<Transform>();


    public override void Init()
    {
        bg = transform.Find("bg");

        inputMessage = transform.Find("bg/inputMessage").GetComponent<UIInput>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        btnSend = transform.Find("bg/btnSend").GetComponent<UIButton>();

        btnEmoji = transform.Find("bg/menu/btnEmoji").GetComponent<UIButton>();
        btnShorter = transform.Find("bg/menu/btnShorter").GetComponent<UIButton>();
        btnChat = transform.Find("bg/menu/btnChat").GetComponent<UIButton>();

        pnlChat = transform.Find("bg/content/pnlChat").gameObject;
        pnlEmoji = transform.Find("bg/content/pnlEmoji").gameObject;
        pnlShorter = transform.Find("bg/content/pnlShorter").gameObject;

        for (int i = 0; i < 12; i++)
        {
            shorter[i] = transform.Find("bg/content/pnlShorter/lblShorter_" + i).gameObject;
            UIEventListener.Get(shorter[i]).onClick = OnShorterClick;
        }
        for (int i = 0; i < 9; i++)
        {
            emoji[i] = transform.Find("bg/content/pnlEmoji/emoji_" + i).gameObject;
            UIEventListener.Get(emoji[i]).onClick = OnEmojiClick;
        }

        otherChatItem = transform.Find("bg/content/pnlChat/otherChatItem");
        myChatItem = transform.Find("bg/content/pnlChat/myChatItem");

        flagAudio = transform.Find("flagAudio").gameObject;
        sptAudio = transform.Find("flagAudio/sptAudio").GetComponent<UISprite>();

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnSend.onClick, OnBtnSendClick);
        EventDelegate.Add(btnEmoji.onClick, OnBtnEmojiClick);
        EventDelegate.Add(btnShorter.onClick, OnBtnShorterClick);
        EventDelegate.Add(btnChat.onClick, OnBtnChatClick);

        gameObject.SetActive(true);
        otherChatItem.gameObject.SetActive(false);
        myChatItem.gameObject.SetActive(false);
        bg.gameObject.SetActive(true);
        btnClose.isEnabled = false;
        bg.localPosition = new Vector3(AppConfig.screenWidth/2 + 340f, 0f, 0f);
        inputMessage.defaultText = "聊天内容不超过32个汉字";

        flagAudio.SetActive(false);

        itemList.Clear();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgChat += Open;
        GameEvent.S_ReceiveChatMessage += AddChatItem;
        GameEvent.V_OpenFlagAudio += OpenFlagAudio;
        GameEvent.V_CloseFlagAudio += CloseFlagAudio;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgChat -= Open;
        GameEvent.S_ReceiveChatMessage -= AddChatItem;
        GameEvent.V_OpenFlagAudio -= OpenFlagAudio;
        GameEvent.V_CloseFlagAudio -= CloseFlagAudio;
    }

    #region UI方法

    public void Open()
    {
        CloseFlagAudio();
        btnClose.isEnabled = true;
        bg.DOLocalMoveX(AppConfig.screenWidth/2 - 340f, 0.3f);
        inputMessage.value = "";

        pnlChat.SetActive(false);
        pnlEmoji.SetActive(false);
        pnlShorter.SetActive(false);

        btnChat.isEnabled = false;
        btnShorter.isEnabled = true;
        btnEmoji.isEnabled = true;


        pnlChat.GetComponent<UIScrollView>().ResetPosition();
        DoActionDelay(OpenChatRecord, 0.3f);
    }

    void OpenChatRecord()
    {
        pnlChat.SetActive(true);
    }

    public void Close()
    {
        bg.DOLocalMoveX(AppConfig.screenWidth/2 + 340f, 0.3f);
        btnClose.isEnabled = false;
    }

    //显示聊天标志
    public void OpenFlagAudio()
    {
        flagAudio.SetActive(true);
        sptAudio.fillAmount = 0;
        CancelInvoke("PlayFlagAudioAnim");
        InvokeRepeating("PlayFlagAudioAnim", 0, 0.15f);
    }

    //关闭聊天标志
    public void CloseFlagAudio()
    {
        flagAudio.SetActive(false);
        CancelInvoke("PlayFlagAudioAnim");
    }

    //聊天标志动画
    void PlayFlagAudioAnim()
    {
        if (sptAudio.fillAmount >= 1)
        {
            sptAudio.fillAmount = 0f;
        }
        else
        {
            sptAudio.fillAmount += 0.167f;
        }
    }

    

    public void AddChatItem(ChatMessage message)
    {
        if (message.type == ChatMessageType.Audio)
        {
            return;
        }

        //聊天记录最多显示30条
        if (itemList.Count >= MaxChatItemCount)
        {
            PoolManager.Instance.Unspawn(itemList[0].gameObject);
            itemList.RemoveAt(0);
        }
        //取出Item
        Transform item = null;
        if (message.userName == HallModel.userName)
        {
            item = PoolManager.Instance.Spawn(myChatItem.gameObject).transform;
            item.name = myChatItem.name;
        }
        else
        {
            item = PoolManager.Instance.Spawn(otherChatItem.gameObject).transform;
            item.name = otherChatItem.name;
        }
        item.parent = pnlChat.transform;
        //初始化显示
        item.Find("lblUserName").GetComponent<UILabel>().text = message.userName;
        item.Find("userPhoto").GetComponent<UITexture>().mainTexture = GameModel.GetUserPhoto(message.chairId);
        
        switch (message.type)
        {
            case ChatMessageType.Audio:
                item.Find("lblMessage").gameObject.SetActive(false);
                item.Find("emoji").gameObject.SetActive(false);
                item.Find("btnAudio").gameObject.SetActive(true);
                item.Find("btnAudio").GetComponent<AudioSource>().clip = message.clip;
                item.Find("btnAudio/lblTime").GetComponent<UILabel>().text = DateTime.Now.ToString("HH:mm");
                UIEventListener.Get(item.Find("btnAudio").gameObject).onClick = OnBtnAudioClick;
                break;
            case ChatMessageType.Emoji:
                item.Find("lblMessage").gameObject.SetActive(false);
                item.Find("emoji").gameObject.SetActive(true);
                item.Find("btnAudio").gameObject.SetActive(false);
                item.Find("emoji/lblTime").GetComponent<UILabel>().text = DateTime.Now.ToString("HH:mm");
                int emojiIndex = int.Parse(message.message);
                item.Find("emoji").GetComponent<UI2DSpriteAnimation>().Pause();
                item.Find("emoji").GetComponent<UI2DSpriteAnimation>().frames = GameModel.GetEmojiSprites(emojiIndex);
                item.Find("emoji").GetComponent<UI2DSpriteAnimation>().Play();
                break;
            case ChatMessageType.Shorter:
                item.Find("lblMessage").gameObject.SetActive(true);
                item.Find("emoji").gameObject.SetActive(false);
                item.Find("btnAudio").gameObject.SetActive(false);
                int index = 0;
                if (int.TryParse(message.message, out index))
                {
                    if (index >= 0 && index < 12)
                    {
                        item.Find("lblMessage").GetComponent<UILabel>().text = GameModel.chatMessage[index];
                    }
                    else
                    {
                        item.Find("lblMessage").GetComponent<UILabel>().text = "聊天数据解析错误 index out of array";
                    }
                }
                else
                {
                    item.Find("lblMessage").GetComponent<UILabel>().text = message.message;
                }
                item.Find("lblMessage/lblTime").GetComponent<UILabel>().text = DateTime.Now.ToString("HH:mm");
                break;
            case ChatMessageType.Text:
                item.Find("lblMessage").gameObject.SetActive(true);
                item.Find("emoji").gameObject.SetActive(false);
                item.Find("btnAudio").gameObject.SetActive(false);
                item.Find("lblMessage").GetComponent<UILabel>().text = message.message;
                item.Find("lblMessage/lblTime").GetComponent<UILabel>().text = DateTime.Now.ToString("HH:mm");
                break;
        }
        //调整位置
        if (item.Find("lblMessage").GetComponent<UILabel>().height < 45)
        {
            item.GetComponent<UIWidget>().height = 70;
        }
        else
        {
            item.GetComponent<UIWidget>().height = item.Find("lblMessage").GetComponent<UILabel>().height + 30;
        }
        if (itemList.Count > 0)
        {
            item.transform.localPosition = itemList[itemList.Count - 1].transform.localPosition - new Vector3(0, itemList[itemList.Count - 1].GetComponent<UIWidget>().height, 0);
        }
        else
        {
            item.transform.localPosition = new Vector3(0f, 360f, 0f);
        }
        item.transform.localScale = Vector3.one;
        itemList.Add(item);
        pnlChat.GetComponent<UIScrollView>().ResetPosition();
    }



    #endregion

    #region UI响应

    //常用语
    public void OnBtnShorterClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);

        btnEmoji.isEnabled = true;
        btnShorter.isEnabled = false;
        btnChat.isEnabled = true;
        pnlEmoji.SetActive(false);
        pnlShorter.SetActive(true);
        pnlChat.SetActive(false);

        pnlShorter.GetComponent<UIScrollView>().ResetPosition();
    }

    //聊天
    public void OnBtnChatClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);

        btnEmoji.isEnabled = true;
        btnShorter.isEnabled = true;
        btnChat.isEnabled = false;
        pnlEmoji.SetActive(false);
        pnlShorter.SetActive(false);
        pnlChat.SetActive(true);

        pnlChat.GetComponent<UIScrollView>().ResetPosition();
    }


    //表情
    public void OnBtnEmojiClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);

        btnEmoji.isEnabled = false;
        btnShorter.isEnabled = true;
        btnChat.isEnabled = true;
        pnlEmoji.SetActive(true);
        pnlShorter.SetActive(false);
        pnlChat.SetActive(false);

        pnlEmoji.GetComponent<UIScrollView>().ResetPosition();
    }

    public void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    //发送按钮响应
    public void OnBtnSendClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        string text = inputMessage.value;
        
        //输入校验
        if (text.Length == 0)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "聊天内容不能为空！");
            return;
        }
        if (text.Length > 32)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "聊天内容不能超过32个中文字！");
            return;
        }
        if (text.Contains("&"))
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "聊天内容不能包含特殊字符&");
            return;
        }
        //发送消息
        if (GameService.Instance.isConnect)
        {
            SendMessage(text);
        }
        else
        {
            DoActionDelay(SendMessage, 1f, text);
        }
        inputMessage.value = "";
    }

    void SendMessage(string text)
    {
        ChatMessage chatMessage = new ChatMessage(ChatMessageType.Text, text);
        GameService.Instance.SendChatMessage(chatMessage);
    }


    

    //点击表情
    public void OnEmojiClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        int index = int.Parse(obj.name.Split('_')[1]);
        ChatMessage chatMessage = new ChatMessage(ChatMessageType.Emoji, index.ToString());
        GameService.Instance.SendChatMessage(chatMessage);
        //切换至聊天记录页面
        btnEmoji.isEnabled = true;
        btnShorter.isEnabled = true;
        btnChat.isEnabled = false;
        pnlEmoji.SetActive(false);
        pnlShorter.SetActive(false);
        pnlChat.SetActive(true);

        pnlChat.GetComponent<UIScrollView>().ResetPosition();
    }

    //点击常用语
    public void OnShorterClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        ChatMessage chatMessage = new ChatMessage(ChatMessageType.Shorter, obj.name.Replace("lblShorter_", "").Trim());
        GameService.Instance.SendChatMessage(chatMessage);
        //切换至聊天记录页面
        btnEmoji.isEnabled = true;
        btnShorter.isEnabled = true;
        btnChat.isEnabled = false;
        pnlEmoji.SetActive(false);
        pnlShorter.SetActive(false);
        pnlChat.SetActive(true);

        pnlChat.GetComponent<UIScrollView>().ResetPosition();
    }

    //聊天窗口中，音频按钮响应
    public void OnBtnAudioClick(GameObject obj)
    {
        obj.transform.Find("Sprite_0").GetComponent<TweenAlpha>().enabled = true;
        obj.transform.Find("Sprite_1").GetComponent<TweenAlpha>().enabled = true;
        obj.GetComponent<AudioSource>().Play();
        DoActionDelay(CloseAudioButtonAnim, 5f, obj);
    }

    void CloseAudioButtonAnim(GameObject obj)
    {
        obj.transform.Find("Sprite_0").GetComponent<TweenAlpha>().enabled = false;
        obj.transform.Find("Sprite_1").GetComponent<TweenAlpha>().enabled = false;
        obj.transform.Find("Sprite_0").GetComponent<UISprite>().alpha = 1;
        obj.transform.Find("Sprite_1").GetComponent<UISprite>().alpha = 1;
    }
    #endregion
}
