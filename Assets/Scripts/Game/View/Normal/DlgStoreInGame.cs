using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class DlgStoreInGame : View
{
    private Transform bg;
    private Transform shade;


    private UIButton btnClose;
    private UIButton btnDiamond;
    //pnlDiamond
    private GameObject pnlDiamond;
    private GameObject[] btnDiamonds = new GameObject[8];
    //pnlCoin
    private GameObject pnlCoin;
    private GameObject[] btnCoins = new GameObject[8];


    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

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

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgStoreInGame += Open;
        GameEvent.V_CloseDlgStoreInGame += Close;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgStoreInGame -= Open;
        GameEvent.V_CloseDlgStoreInGame -= Close;
    }

    #region UI操作

    public void Open(DlgStoreArg arg)
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        pnlDiamond.SetActive(arg == DlgStoreArg.DiamondPage);
        pnlCoin.SetActive(arg == DlgStoreArg.CoinPage);
        InitConfig();
    }

    public void Close()
    {
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    public void CloseCor()
    {
        gameObject.SetActive(false);
    }


    #endregion

    void InitConfig()
    {
        //信息保存
        List<GoodInfo> diamondList = new List<GoodInfo>();
        List<GoodInfo> coinList = new List<GoodInfo>();
        if (Application.platform == RuntimePlatform.IPhonePlayer && HallModel.isVerify)
        {
            foreach (GoodInfo info in AppConfig.goodDic_ios.Values)
            {
                if (info.Type == 2)
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
                if (info.Type == 2)
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
