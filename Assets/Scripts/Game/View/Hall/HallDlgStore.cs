using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HallDlgStore : View
{
    private Transform bg;

    private UIButton btnMonthCard;
    private UIButton btnFirstCharge;
    private UILabel lblCoinCount;
    private UILabel lblDiamondCount;
    private UILabel lblCardCount;

    private UIButton btnClose;
    private UIButton btnDiamond;
    private UIButton btnCard;
    private UIButton btnCoin;
    //pnlCard
    private GameObject pnlCard;
    private GameObject[] btnCards = new GameObject[8];
    //pnlDiamond
    private GameObject pnlDiamond;
    private GameObject[] btnDiamonds = new GameObject[8];
    //pnlCoin
    private GameObject pnlCoin;
    private GameObject[] btnCoins = new GameObject[8];


    public override void Init()
    {
        bg = transform.Find("bg");
        btnMonthCard = transform.Find("bg/btnMonthCard").GetComponent<UIButton>();
        btnFirstCharge = transform.Find("bg/btnFirstCharge").GetComponent<UIButton>();
        lblCoinCount = transform.Find("bg/lblCoinCount").GetComponent<UILabel>();
        lblDiamondCount = transform.Find("bg/lblDiamondCount").GetComponent<UILabel>();
        lblCardCount = transform.Find("bg/lblCardCount").GetComponent<UILabel>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        btnDiamond = transform.Find("bg/menu/btnDiamond").GetComponent<UIButton>();
        btnCard = transform.Find("bg/menu/btnCard").GetComponent<UIButton>();
        btnCoin = transform.Find("bg/menu/btnCoin").GetComponent<UIButton>();
        //pnlCard
        pnlCard = transform.Find("bg/pnlCard").gameObject;
        for (int i = 0; i < 8; i++)
        {
            btnCards[i] = transform.Find("bg/pnlCard/btnGood_" + i).gameObject;
            UIEventListener.Get(btnCards[i]).onClick = OnBtnCardsClick;
        }
        //pnlDiamond
        pnlDiamond = transform.Find("bg/pnlDiamond").gameObject;
        for (int i = 0; i < 8; i++)
        {
            btnDiamonds[i] = transform.Find("bg/pnlDiamond/btnGood_" + i).gameObject;
            UIEventListener.Get(btnDiamonds[i]).onClick = OnBtnDiamondsClick;
        }
        //pnlCoin
        pnlCoin = transform.Find("bg/pnlCoin").gameObject;
        for (int i = 0; i < 8; i++)
        {
            btnCoins[i] = transform.Find("bg/pnlCoin/btnGood_" + i).gameObject;
            UIEventListener.Get(btnCoins[i]).onClick = OnBtnCoinsClick;
        }

        //月卡描述信息
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type != 4) continue;
            //月卡描述信息
            btnMonthCard.transform.Find("Label").GetComponent<UILabel>().text = "¥ " + info.RechargePrice;
            btnMonthCard.transform.Find("Label0").GetComponent<UILabel>().text = "钻石*"+info.Count;
            btnMonthCard.transform.Find("Label1").GetComponent<UILabel>().text = "金币*"+info.GiveCount;
        }

        //月卡ID 620
        foreach(ExchangeInfo info in AppConfig.exchangeDic.Values)
        {
            if(info.ID != 620) continue;
            
            //月卡描述信息
            btnMonthCard.transform.Find("Label2").GetComponent<UILabel>().text = "钻石*"+info.UseResultsIngot;
            btnMonthCard.transform.Find("Label3").GetComponent<UILabel>().text = "金币*"+info.UseResultsGold;
            btnMonthCard.transform.Find("Label4").GetComponent<UILabel>().text = "礼券*"+info.UseResultsCash;
            break;
        }
        

        //首充类型 5
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type != 5) continue;
            //首充描述信息
            btnFirstCharge.transform.Find("Label").GetComponent<UILabel>().text = "¥ " + info.RechargePrice;
            btnFirstCharge.transform.Find("Label0").GetComponent<UILabel>().text = "钻石*"+info.Count;
            btnFirstCharge.transform.Find("Label1").GetComponent<UILabel>().text = "金币*"+info.GiveCount;
            break;
        }
        
        EventDelegate.Add(btnMonthCard.onClick, OnBtnMonthCardClick);
        EventDelegate.Add(btnFirstCharge.onClick, OnBtnFirstChargeClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnCard.onClick, OnBtnCardClick);
        EventDelegate.Add(btnDiamond.onClick, OnBtnDiamondClick);
        EventDelegate.Add(btnCoin.onClick, OnBtnCoinClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgStore += Open;
        GameEvent.V_RefreshUserInfo += RefreshUserInfo;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgStore -= Open;
        GameEvent.V_RefreshUserInfo -= RefreshUserInfo;
    }



    void Open(DlgStoreArg arg)
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenAlpha>().PlayForward();
        pnlCard.SetActive(arg == DlgStoreArg.CardPage);
        pnlDiamond.SetActive(arg == DlgStoreArg.DiamondPage);
        pnlCoin.SetActive(arg == DlgStoreArg.CoinPage);
        btnCard.isEnabled = !(arg == DlgStoreArg.CardPage);
        btnDiamond.isEnabled = !(arg == DlgStoreArg.DiamondPage);
        btnCoin.isEnabled = !(arg == DlgStoreArg.CoinPage);
        InitConfig();
        RefreshUserInfo(true);
    }

    void Close()
    {
        bg.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }

    void RefreshUserInfo(bool Score)
    {
        lblCardCount.text = HallModel.userCardCount.ToString("N0");
        lblCoinCount.text = HallModel.userCoinInGame.ToString("N0");
        lblDiamondCount.text = HallModel.userDiamondCount.ToString("N0");
    }

    void InitConfig()
    {
        //信息保存
        List<GoodInfo> diamondList = new List<GoodInfo>();
        List<GoodInfo> cardList = new List<GoodInfo>();
        List<GoodInfo> coinList = new List<GoodInfo>();
        if (Application.platform == RuntimePlatform.IPhonePlayer && HallModel.isVerify)
        {
            foreach (GoodInfo info in AppConfig.goodDic_ios.Values)
            {
                if (info.Type == 1)
                {
                    cardList.Add(info);
                }
                else if (info.Type == 2)
                {
                    diamondList.Add(info);                    
                }
                else if (info.Type == 3)
                {
                    coinList.Add(info);
                }
            }
        }
        else
        {
            foreach (GoodInfo info in AppConfig.goodDic_android.Values)
            {
                if (info.Type == 1)
                {
                    cardList.Add(info);
                }
                else if (info.Type == 2)
                {
                    diamondList.Add(info);
                }
                else if (info.Type == 3)
                {
                    coinList.Add(info);
                }
            }
        }
        //显示
        for (int i = 0; i < 8; i++)
        {
            if (i < diamondList.Count)
            {
                btnDiamonds[i].SetActive(true);
                btnDiamonds[i].name = "btnGood_" + diamondList[i].RechargeID;
                btnDiamonds[i].transform.Find("lblCount").GetComponent<UILabel>().text = diamondList[i].Count + "钻石";
                btnDiamonds[i].transform.Find("lblPrice").GetComponent<UILabel>().text = "¥ " + diamondList[i].RechargePrice;
                btnDiamonds[i].transform.Find("lblSendCount").gameObject.SetActive(diamondList[i].GiveCount > 0);
                btnDiamonds[i].transform.Find("lblSendRate").gameObject.SetActive(diamondList[i].GiveRate > 0);
                btnDiamonds[i].transform.Find("lblSendCount").GetComponent<UILabel>().text = "赠" + diamondList[i].GiveCount + "颗";
                btnDiamonds[i].transform.Find("lblSendRate").GetComponent<UILabel>().text = "+" + diamondList[i].GiveRate + "%";
            }
            else
            {
                btnDiamonds[i].SetActive(false);
            }
            if (i < cardList.Count)
            {
                btnCards[i].SetActive(true);
                btnCards[i].name = "btnGood_" + cardList[i].RechargeID;
                btnCards[i].transform.Find("lblCount").GetComponent<UILabel>().text = cardList[i].Count + "房卡";
                btnCards[i].transform.Find("lblPrice").GetComponent<UILabel>().text = "¥ " + cardList[i].RechargePrice;

                btnCards[i].transform.Find("lblSendCount").gameObject.SetActive(cardList[i].GiveCount > 0);
                btnCards[i].transform.Find("lblSendRate").gameObject.SetActive(cardList[i].GiveRate > 0);
                btnCards[i].transform.Find("lblSendCount").GetComponent<UILabel>().text = "赠" + cardList[i].GiveCount + "张";
                btnCards[i].transform.Find("lblSendRate").GetComponent<UILabel>().text = "+" + cardList[i].GiveRate + "%";
            }
            else
            {
                btnCards[i].SetActive(false);
            }
            if (i < coinList.Count)
            {
                btnCoins[i].SetActive(true);
                btnCoins[i].name = "btnGood_" + coinList[i].RechargeID;
                btnCoins[i].transform.Find("lblCount").GetComponent<UILabel>().text = Util.GetCoinNumStr(coinList[i].Count) + "金币";
                btnCoins[i].transform.Find("lblPrice").GetComponent<UILabel>().text = "¥ " + coinList[i].RechargePrice;

                btnCoins[i].transform.Find("lblSendCount").gameObject.SetActive(coinList[i].GiveCount > 0);
                btnCoins[i].transform.Find("lblSendRate").gameObject.SetActive(coinList[i].GiveRate > 0);
                btnCoins[i].transform.Find("lblSendCount").GetComponent<UILabel>().text = "赠" + Util.GetCoinNumStr(coinList[i].GiveCount) + "";
                btnCoins[i].transform.Find("lblSendRate").GetComponent<UILabel>().text = "+" + coinList[i].GiveRate + "%";
            }
            else
            {
                btnCoins[i].SetActive(false);
            }
        }
    }


    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    //首充
    void OnBtnFirstChargeClick()
    {
        if (HallModel.isFirstCharge)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "首充每天仅能购买一次！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        int goodId = -1;
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type == 5)
            {
                goodId = info.RechargeID;
            }
        }
        if (goodId < 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "商品信息不存在！");
            return;
        }
        DoAction(GameEvent.V_OpenDlgPayMode, goodId);
    }

    //月卡
    void OnBtnMonthCardClick()
    {
        if (HallModel.isBuyMonthCard)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "您已拥有月卡，无法再次购买！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        int goodId = -1;
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type == 4)
            {
                goodId = info.RechargeID;
            }
        }
        if (goodId < 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "商品信息不存在！");
            return;
        }
        DoAction(GameEvent.V_OpenDlgPayMode, goodId);
    }

    void OnBtnDiamondClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        btnCard.isEnabled = true;
        btnDiamond.isEnabled = false;
        btnCoin.isEnabled = true;
        pnlCard.SetActive(false);
        pnlDiamond.SetActive(true);
        pnlCoin.SetActive(false);
    }

    void OnBtnCardClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        btnCard.isEnabled = false;
        btnDiamond.isEnabled = true;
        btnCoin.isEnabled = true;
        pnlCard.SetActive(true);
        pnlDiamond.SetActive(false);
        pnlCoin.SetActive(false);
    }

    void OnBtnCoinClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        btnCard.isEnabled = true;
        btnDiamond.isEnabled = true;
        btnCoin.isEnabled = false;
        pnlCard.SetActive(false);
        pnlDiamond.SetActive(false);
        pnlCoin.SetActive(true);
    }



    void OnBtnCardsClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        int index = int.Parse(obj.name.Split('_')[1]);
        DoAction(GameEvent.V_OpenDlgPayMode, index);
    }

    void OnBtnDiamondsClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        int index = int.Parse(obj.name.Split('_')[1]);
        DoAction(GameEvent.V_OpenDlgPayMode, index);
    }

    void OnBtnCoinsClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        int index = int.Parse(obj.name.Split('_')[1]);
        DoAction(GameEvent.V_OpenDlgPayMode, index);
    }


}
