using UnityEngine;
using System.Collections;

public class HallDlgUserInfo : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnBindPhone;
    private UIButton btnBindSpreader;
    private UIButton btnUnBindPhone;
    private UIButton btnSex_0;
    private UIButton btnSex_1;
    private UIButton btnBuyCard;
    private UIButton btnBuyDiamond;
    private UIButton btnModifyNickName;
    private UIButton btnClose;

    private UITexture sptPhoto;
    private UILabel lblNickName;
    private UILabel lblGameId;
    private UILabel lblDiamondCount;
    private UILabel lblCardCount;
    private UILabel lblUserIp;
    private UILabel lblUserRate;
    private UILabel lblPhoneNum;
    private UILabel lblSpreaderInfo;


    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        btnBindPhone = transform.Find("bg/btnBindPhone").GetComponent<UIButton>();
        btnBindSpreader = transform.Find("bg/btnBindSpreader").GetComponent<UIButton>();
        btnUnBindPhone = transform.Find("bg/lblPhoneNum/btnUnBindPhone").GetComponent<UIButton>();
        btnSex_0 = transform.Find("bg/btnSex_0").GetComponent<UIButton>();
        btnSex_1 = transform.Find("bg/btnSex_1").GetComponent<UIButton>();
        btnBuyCard = transform.Find("bg/btnBuyCard").GetComponent<UIButton>();
        btnBuyDiamond = transform.Find("bg/btnBuyDiamond").GetComponent<UIButton>();
        btnModifyNickName = transform.Find("bg/btnModifyNickName").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        sptPhoto = transform.Find("bg/sptPhoto").GetComponent<UITexture>();
        lblNickName = transform.Find("bg/lblNickName").GetComponent<UILabel>();
        lblGameId = transform.Find("bg/lblGameId").GetComponent<UILabel>();
        lblDiamondCount = transform.Find("bg/lblDiamondCount").GetComponent<UILabel>();
        lblCardCount = transform.Find("bg/lblCardCount").GetComponent<UILabel>();
        lblUserIp = transform.Find("bg/lblUserIP").GetComponent<UILabel>();
        lblUserRate = transform.Find("bg/lblUserRate").GetComponent<UILabel>();
        lblPhoneNum = transform.Find("bg/lblPhoneNum").GetComponent<UILabel>();
        lblSpreaderInfo = transform.Find("bg/lblSpreaderInfo").GetComponent<UILabel>();

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnBindPhone.onClick, OnBtnBindPhoneClick);
        EventDelegate.Add(btnUnBindPhone.onClick, OnBtnUnBindPhoneClick);
        EventDelegate.Add(btnBindSpreader.onClick, OnBtnBindSpreaderClick);
        EventDelegate.Add(btnSex_0.onClick, OnBtnSexClick_0);
        EventDelegate.Add(btnSex_1.onClick, OnBtnSexClick_1);
        EventDelegate.Add(btnBuyCard.onClick, OnBtnBuyCoinClick);
        EventDelegate.Add(btnBuyDiamond.onClick, OnBtnBuyDiamondClick);
        EventDelegate.Add(btnModifyNickName.onClick ,OnBtnModifyNickNameClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();

    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgUserInfo += Open;
        GameEvent.V_RefreshUserInfo += RefreshUserInfo;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgUserInfo -= Open;
        GameEvent.V_RefreshUserInfo -= RefreshUserInfo;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        RefreshUserInfo(true);

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

    void RefreshUserInfo(bool isRefreshScore)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (HallModel.userPhotos.ContainsKey(HallModel.userId))
        {
            sptPhoto.mainTexture = HallModel.userPhotos[HallModel.userId];
        }
        else
        {
            sptPhoto.mainTexture = HallModel.defaultPhoto;
        }
        lblNickName.text = HallModel.userName;
        lblGameId.text = "ID : " + HallModel.gameId.ToString();
        lblDiamondCount.text = HallModel.userDiamondCount.ToString("N0");
        //lblCardCount.text = HallModel.userCardCount.ToString("N0");
        lblCardCount.text = HallModel.userCoinInGame.ToString("N0");
        lblUserIp.text = HallModel.userIP;
        lblUserRate.text = HallModel.userRateInfo;

        btnBindSpreader.gameObject.SetActive(HallModel.spreaderId == 0);
        lblSpreaderInfo.gameObject.SetActive(HallModel.spreaderId != 0);
        lblSpreaderInfo.text = HallModel.spreaderName + "(" + HallModel.spreaderId + ")";

        btnBindSpreader.gameObject.SetActive(false);
        //lblSpreaderInfo.gameObject.SetActive(true);
        //if (HallModel.spreaderId == 0)
        //{
        //    lblSpreaderInfo.color = Color.gray;
        //    lblSpreaderInfo.text = "无";
        //}
        //else
        //{
        //    lblSpreaderInfo.color = Color.green;
        //    lblSpreaderInfo.text = HallModel.spreaderName + "(" + HallModel.spreaderId + ")";
        //}


        if (HallModel.userPhone.Length == 11)
        {
            btnBindPhone.gameObject.SetActive(false);
            btnUnBindPhone.gameObject.SetActive(false);
            lblPhoneNum.gameObject.SetActive(true);
            string phoneNum = HallModel.userPhone.Remove(3, 4);
            phoneNum = phoneNum.Insert(3, "****");
            lblPhoneNum.text = phoneNum;
        }
        else
        {
            btnBindPhone.gameObject.SetActive(false);
            btnUnBindPhone.gameObject.SetActive(false);
            lblPhoneNum.gameObject.SetActive(false);
        }

        btnSex_0.GetComponent<UISprite>().spriteName = HallModel.userSex == 0 ? "select_open" : "select_close";
        btnSex_1.GetComponent<UISprite>().spriteName = HallModel.userSex == 1 ? "select_open" : "select_close";
        btnSex_0.normalSprite = HallModel.userSex == 0 ? "select_open" : "select_close";
        btnSex_1.normalSprite = HallModel.userSex == 1 ? "select_open" : "select_close";

    }





    void OnBtnBindPhoneClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        DoActionDelay(HallEvent.V_OpenDlgBindOp, 0.1f, DlgBindArg.BindPhonePage);
    }

    void OnBtnUnBindPhoneClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        DoActionDelay(HallEvent.V_OpenDlgBindOp, 0.1f, DlgBindArg.UnBindPhonePage);
    }

    void OnBtnBindSpreaderClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        DoActionDelay(HallEvent.V_OpenDlgBindOp, 0.1f, DlgBindArg.BindSpreader);
    }

    void OnBtnSexClick_0()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        HallModel.userSex = 0;
        btnSex_0.GetComponent<UISprite>().spriteName = HallModel.userSex == 0 ? "select_open" : "select_close";
        btnSex_1.GetComponent<UISprite>().spriteName = HallModel.userSex == 1 ? "select_open" : "select_close";
        btnSex_0.normalSprite = HallModel.userSex == 0 ? "select_open" : "select_close";
        btnSex_1.normalSprite = HallModel.userSex == 1 ? "select_open" : "select_close";
    }

    void OnBtnSexClick_1()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        HallModel.userSex = 1;
        btnSex_0.GetComponent<UISprite>().spriteName = HallModel.userSex == 0 ? "select_open" : "select_close";
        btnSex_1.GetComponent<UISprite>().spriteName = HallModel.userSex == 1 ? "select_open" : "select_close";
        btnSex_0.normalSprite = HallModel.userSex == 0 ? "select_open" : "select_close";
        btnSex_1.normalSprite = HallModel.userSex == 1 ? "select_open" : "select_close";
    }

    void OnBtnBuyCoinClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        //DoActionDelay(HallEvent.V_OpenDlgStore, 0.3f, DlgStoreArg.CardPage);
        //首充判断
        if (HallModel.isFirstCharge == false)
        {
            DoActionDelay(GameEvent.V_OpenDlgFirstCharge, 0.3f, delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.CoinPage); });

        }
        else
        {
            DoActionDelay(HallEvent.V_OpenDlgStore, 0.3f, DlgStoreArg.CoinPage);
        }
    }

    void OnBtnBuyDiamondClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        //首充判断
        if (HallModel.isFirstCharge == false)
        {
            DoActionDelay(GameEvent.V_OpenDlgFirstCharge, 0.3f, delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.DiamondPage); });

        }
        else
        {
            DoActionDelay(HallEvent.V_OpenDlgStore, 0.3f, DlgStoreArg.DiamondPage);
        }
    }

    void OnBtnModifyNickNameClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        DoActionDelay(HallEvent.V_OpenDlgModifyNickName, 0.1f);
    }

    public void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    
}
