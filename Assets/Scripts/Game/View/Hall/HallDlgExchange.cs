using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class HallDlgExchange : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnClose;
    private UIButton btnExchange;
    private UIButton btnRecord;
    private UIButton btnHelp;
    private UILabel lblRedPackCount;
    //pnlExchange
    private GameObject pnlExchange;
    private GameObject[] btnExchanges = new GameObject[8];
     //pnlShareTip
    private GameObject pnlShareTip;
    private UIButton btnShareCircle;
    private UIButton btnShareClose;
    //pnlRecord
    private GameObject pnlRecord;
    private Transform[] itemRecord = new Transform[5];
    private UILabel lblPageInfo;
    private UIButton btnPrePage;
    private UIButton btnNextPage;
    //pnlHelp
    private GameObject pnlHelp;

    private int exchangeId = 0;         //兑换商品ID
    private int currentPageIndex = 1;   //当前页
    private int totalPageIndex = 1;     //总页数

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        btnExchange = transform.Find("bg/menu/btnExchange").GetComponent<UIButton>();
        btnRecord = transform.Find("bg/menu/btnRecord").GetComponent<UIButton>();
        btnHelp = transform.Find("bg/menu/btnHelp").GetComponent<UIButton>();
        lblRedPackCount = transform.Find("bg/pnlExchange/lblRedPackCount").GetComponent<UILabel>();
        //pnlGift
        pnlExchange = transform.Find("bg/pnlExchange").gameObject;
        for (int i = 0; i < 8; i++)
        {
            btnExchanges[i] = transform.Find("bg/pnlExchange/btnGood_" + i).gameObject;
            UIEventListener.Get(btnExchanges[i]).onClick = OnBtnGiftsClick;
        }

        pnlShareTip = transform.Find("bg/pnlExchange/pnlShareTip").gameObject;
        btnShareCircle = transform.Find("bg/pnlExchange/pnlShareTip/btnShare").GetComponent<UIButton>();
        btnShareClose = transform.Find("bg/pnlExchange/pnlShareTip/btnClose").GetComponent<UIButton>();

        //pnlRecord
        pnlRecord = transform.Find("bg/pnlRecord").gameObject;
        lblPageInfo = transform.Find("bg/pnlRecord/lblPageInfo").GetComponent<UILabel>();
        btnPrePage = transform.Find("bg/pnlRecord/btnPrePage").GetComponent<UIButton>();
        btnNextPage = transform.Find("bg/pnlRecord/btnNextPage").GetComponent<UIButton>();
        for (int i = 0; i < 5; i++)
        {
            itemRecord[i] = transform.Find("bg/pnlRecord/record_" + i);
        }
        //pnlHelp
        pnlHelp = transform.Find("bg/pnlHelp").gameObject;

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnExchange.onClick, OnBtnExchangeClick);
        EventDelegate.Add(btnRecord.onClick, OnBtnRecordClick);
        EventDelegate.Add(btnHelp.onClick, OnBtnHelpClick);
        EventDelegate.Add(btnPrePage.onClick, OnBtnPrePageClick);
        EventDelegate.Add(btnNextPage.onClick, OnBtnNextPageClick);

        EventDelegate.Add(btnShareCircle.onClick, OnBtnShareCircle);
        EventDelegate.Add(btnShareClose.onClick, OnBtnShareClose);
        pnlShareTip.SetActive(false);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgExchange += Open;
        GameEvent.V_RefreshUserInfo += RefreshUserInfo;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgExchange -= Open;
        GameEvent.V_RefreshUserInfo -= RefreshUserInfo;
    }



    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
        pnlExchange.SetActive(true);
        pnlHelp.SetActive(false);
        pnlRecord.SetActive(false);
        btnExchange.isEnabled = false;
        btnRecord.isEnabled = true;
        btnHelp.isEnabled = true;
        //初始化界面
        InitConfig();
        RefreshUserInfo(true);
    }

    void Close()
    {
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }

    void RefreshUserInfo(bool isRefreshScore)
    {
        lblRedPackCount.text = HallModel.userRedPackCount.ToString();
    }

    void InitConfig()
    {
        int index = 0;
        for (int i = 0; i < 8; i++)
        {
            btnExchanges[i].SetActive(false);
        }
        foreach(ExchangeInfo info in AppConfig.exchangeDic.Values)
        {
            if (index < 8)
            {
                btnExchanges[index].SetActive(true);
                btnExchanges[index].name = "btnGood_" + info.ID;
                if (info.Kind == 18)
                {
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().spriteName = "红包标志";
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().MakePixelPerfect();
                    btnExchanges[index].transform.Find("lblCount").GetComponent<UILabel>().text = info.UseResultsCash + "元微信红包";
                    btnExchanges[index].transform.Find("lblPrice").GetComponent<UILabel>().text = "x " + info.Cash;
                    index++;
                }
                else if(info.Kind == 19)
                {
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().spriteName = "钻石02";
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().MakePixelPerfect();
                    btnExchanges[index].transform.Find("lblCount").GetComponent<UILabel>().text = info.UseResultsIngot + " 个钻石";
                    btnExchanges[index].transform.Find("lblPrice").GetComponent<UILabel>().text = "x " + info.Cash;
                    index++;
                }
                else if (info.Kind == 20)
                {
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().spriteName = "金币04";
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().height = 110;
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().width = 110;
                    btnExchanges[index].transform.Find("lblCount").GetComponent<UILabel>().text = Util.GetCoinNumStr(info.UseResultsGold) + "金币";
                    btnExchanges[index].transform.Find("lblPrice").GetComponent<UILabel>().text = "x " + info.Cash;
                    index++;
                }
                else if (info.Kind == 21)
                {
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().spriteName = "房卡标志";
                    btnExchanges[index].transform.Find("flagGood").GetComponent<UISprite>().MakePixelPerfect();
                    btnExchanges[index].transform.Find("lblCount").GetComponent<UILabel>().text = Util.GetCoinNumStr(info.UseResultsRoomCard) + "张房卡";
                    btnExchanges[index].transform.Find("lblPrice").GetComponent<UILabel>().text = "x " + info.Cash;
                    index++;
                }
            }
        }
    }


    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    void OnBtnExchangeClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        btnExchange.isEnabled = false;
        btnRecord.isEnabled = true;
        btnHelp.isEnabled = true;
        pnlExchange.SetActive(true);
        pnlHelp.SetActive(false);
        pnlRecord.SetActive(false);
    }

    void OnBtnRecordClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        btnExchange.isEnabled = true;
        btnRecord.isEnabled = false;
        btnHelp.isEnabled = true;
        pnlExchange.SetActive(false);
        pnlHelp.SetActive(false);
        pnlRecord.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            itemRecord[i].gameObject.SetActive(false);
        }
        lblPageInfo.text = currentPageIndex + "/" + totalPageIndex;
        GetExchangeRecord(currentPageIndex);
    }

    void OnBtnHelpClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        btnExchange.isEnabled = true;
        btnRecord.isEnabled = true;
        btnHelp.isEnabled = false;
        pnlExchange.SetActive(false);
        pnlHelp.SetActive(true);
        pnlRecord.SetActive(false);

        pnlHelp.GetComponent<UIScrollView>().ResetPosition();
    }

    void OnBtnGiftsClick(GameObject obj)
    {
        //测试代码
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        exchangeId = int.Parse(obj.name.Split('_')[1]);
        if (exchangeId >= 602 && exchangeId <= 604 && HallModel.isNeedShareGameWhenExchange == true)
        {
            pnlShareTip.SetActive(true);
        }
        else
        {
            Exchange();
        }
        
        return;

        // if (HallModel.userPhone.Length != 11)
        // {
        //     DoAction(HallEvent.V_OpenDlgBindOp, DlgBindArg.BindPhonePage);
        //     return;
        // }

        // AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        // exchangeId = int.Parse(obj.name.Split('_')[1]);
        // if (GameEvent.V_OpenDlgTip != null)
        // {
        //     string redpackTip = obj.transform.Find("lblPrice").GetComponent<UILabel>().text;
        //     string giftTip = obj.transform.Find("lblCount").GetComponent<UILabel>().text;
        //     GameEvent.V_OpenDlgTip.Invoke("您将使用" + AppConfig.exchangeDic[exchangeId].Cash + "礼券，兑换" + giftTip, "先把喜悦分享给好友，才能领取红包哦", ShareCircle, null);
        // }
    }

    void OnBtnPrePageClick()
    {
        if (currentPageIndex <= 1)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            currentPageIndex = 1;
            DoAction(GameEvent.V_OpenShortTip, "当前已是第一页！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        for (int i = 0; i < 5; i++)
        {
            itemRecord[i].gameObject.SetActive(false);
        }
        GetExchangeRecord(currentPageIndex - 1);
    }

    void OnBtnNextPageClick()
    {
        if (currentPageIndex >= totalPageIndex)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            currentPageIndex = totalPageIndex;
            DoAction(GameEvent.V_OpenShortTip, "当前已是最后一页！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        for (int i = 0; i < 5; i++)
        {
            itemRecord[i].gameObject.SetActive(false);
        }
        GetExchangeRecord(currentPageIndex + 1);
    }

    //分享朋友圈
    void OnBtnShareCircle()
    {
        pnlShareTip.SetActive(false);
        StartCoroutine(WeiXinShare(1));
    }

    //关闭分享
    void OnBtnShareClose()
    {
        pnlShareTip.SetActive(false);
    }

    //打开绑定手机窗口
    void OpenDlgBindPhone()
    {
        Close();
        DoActionDelay(HallEvent.V_OpenDlgBindOp, 0.3f, DlgBindArg.BindPhonePage);
    }

    //分享朋友圈
    void ShareCircle()
    {
        StartCoroutine(WeiXinShare(1));
    }

    //兑换商品
    void Exchange()
    {
        //Web_C_Exchange pro = new Web_C_Exchange();
        //pro.userId = HallModel.userId;
        //pro.goodsId = exchangeId;
        //WebService.Instance.Send<Web_S_Exchange>(AppConfig.url_Exchange, pro, OnExchangeResult);
    }

    void OnExchangeResult(Web_S_Exchange pro)
    {
        if (pro == null)
        {
            DoAction(GameEvent.V_OpenShortTip, "兑换失败！");
        }
        else if (pro.return_code != 10000)
        {
            DoAction(GameEvent.V_OpenShortTip, pro.return_message);
        }
        else
        {
            if (AppConfig.exchangeDic.ContainsKey(exchangeId) && AppConfig.exchangeDic[exchangeId].UseResultsCash > 0)
            {
                if (GameEvent.V_OpenDlgTip != null)
                {
                    GameEvent.V_OpenDlgTip.Invoke("兑换成功, 微信红包请前往微信公众号\n“兴隆娱乐中心”领取！", "", null, null);
                }
            }
            else
            {
                GameEvent.V_OpenShortTip.Invoke("兑换成功！");
            }
            HallService.Instance.QueryBankInfo();
        }
    }



    //请求兑换记录
    void GetExchangeRecord(int index)
    {
        //Web_C_ExchangRecord pro = new Web_C_ExchangRecord();
        //pro.userId = HallModel.userId;
        //pro.pageIndex = index;
        //WebService.Instance.Send<Web_S_ExchangeRecord>(AppConfig.url_Exchange, pro, OnGetExchangeRecord);
    }

    void OnGetExchangeRecord(Web_S_ExchangeRecord pro)
    {
        if (pro == null)
        {
            DoAction(GameEvent.V_OpenShortTip, "查询记录失败！");
        }
        else if (pro.return_code != 10000)
        {
            DoAction(GameEvent.V_OpenShortTip, pro.message);
        }
        else
        {
            currentPageIndex = pro.pageIndex;
            totalPageIndex = pro.count;
            lblPageInfo.text = currentPageIndex + "/" + totalPageIndex;
            for (int i = 0; i < 5; i++)
            {
                if (i < pro.return_message.Count)
                {
                    itemRecord[i].gameObject.SetActive(true);
                    itemRecord[i].Find("lblNum").GetComponent<UILabel>().text = (i + 1).ToString();
                    itemRecord[i].Find("lblName").GetComponent<UILabel>().text = pro.return_message[i].PropertyName;
                    itemRecord[i].Find("lblDate").GetComponent<UILabel>().text = pro.return_message[i].UseDate;
                    itemRecord[i].Find("lblCode").GetComponent<UILabel>().text = pro.return_message[i].RecordID.ToString();
                }
                else
                {
                    itemRecord[i].gameObject.SetActive(false);
                }
            }
        }
    }



    IEnumerator WeiXinShare(int type)
    {
        //1.获取分享图片下载地址
        WWW www01 = new WWW(AppConfig.weixinShareTextureUrl);
        yield return www01;
        if (www01.error == null)
        {
            string textureUrl = www01.text;
            yield return new WaitForEndOfFrame();
            //2.下载分享图片
            WWW www02 = new WWW(textureUrl);
            yield return www02;
            if (www02.error == null)
            {
                //3.保存分享图片到本地
                string savePath = Application.persistentDataPath + "/wxShare_" + HallModel.gameId + ".jpg";

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                yield return new WaitForEndOfFrame();
                try
                {
                    File.WriteAllBytes(savePath, www02.bytes);
                }
                catch (Exception e)
                {
                    Debug.Log("保存图片异常 ： " + e.ToString());
                }
                yield return new WaitForSeconds(0.1f);
                //4.分享到朋友圈
                PluginManager.Instance.WxShareImage(type, savePath, Exchange);
            }
            else
            {
                Debug.LogError("下载分享图片错误 ： " + www02.error);
            }
        }
        else
        {
            Debug.LogError(www01.error);
        }
    }

}
