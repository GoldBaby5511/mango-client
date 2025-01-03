using UnityEngine;
using System.Collections;

public class DlgPayMode : View
{
    private Transform bg;
    private Transform shade;

    private UILabel lblGoodName;
    private UILabel lblGoodPrice;

    private UIButton btnAliPay;
    private UIButton btnWxPay;
    private UIButton btnClose;

    private int goodId = 0;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        lblGoodName = transform.Find("bg/lblGoodName").GetComponent<UILabel>();
        lblGoodPrice = transform.Find("bg/lblGoodPrice").GetComponent<UILabel>();
        btnAliPay = transform.Find("bg/btnAliPay").GetComponent<UIButton>();
        btnWxPay = transform.Find("bg/btnWxPay").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnAliPay.onClick, OnBtnAliPayClick);
        EventDelegate.Add(btnWxPay.onClick, OnBtnWxPayClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgPayMode += Open;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgPayMode -= Open;
    }

    void Open(int _goodId)
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();


        goodId = _goodId;
        if (Application.platform == RuntimePlatform.IPhonePlayer && HallModel.isVerify)
        {
            lblGoodName.text = AppConfig.goodDic_ios[goodId].RechargeName;
            lblGoodPrice.text = AppConfig.goodDic_ios[goodId].RechargePrice + "元";
        }
        else
        {
            lblGoodName.text = AppConfig.goodDic_android[goodId].RechargeName;
            lblGoodPrice.text = AppConfig.goodDic_android[goodId].RechargePrice + "元";
        }
    }

    void Close()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();

        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }


    void OnBtnAliPayClick()
    {
        //AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        //PluginManager.Instance.AliPay(HallModel.userId, goodId);
        Close();
    }

    void OnBtnWxPayClick()
    {
        //AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        //PluginManager.Instance.WxPay(HallModel.userId, goodId);
        Close();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }
}
