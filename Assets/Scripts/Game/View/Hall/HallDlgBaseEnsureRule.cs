using UnityEngine;
using System.Collections;

public class HallDlgBaseEnsureRule : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnGetAward;
    private UIButton btnExchange;
    private UIButton btnClose;

    private UILabel lblCoinTip;
    private UILabel lblIngotTip;
    private UILabel lblIngotEnsureTimes;
    private UILabel lblCoinEnsureTimes;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        btnGetAward = transform.Find("bg/btnGetAward").GetComponent<UIButton>();
        btnExchange = transform.Find("bg/btnExchange").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        lblCoinTip = transform.Find("bg/lblCoinTip").GetComponent<UILabel>();
        lblIngotTip = transform.Find("bg/lblIngotTip").GetComponent<UILabel>();
        lblIngotEnsureTimes = transform.Find("bg/lblIngotEnsureTimes").GetComponent<UILabel>();
        lblCoinEnsureTimes = transform.Find("bg/lblCoinEnsureTimes").GetComponent<UILabel>();

        EventDelegate.Add(btnGetAward.onClick, OnBtnGetAwardClick);
        EventDelegate.Add(btnExchange.onClick, OnBtnExchangeClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgBaseEnsureRule += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgBaseEnsureRule -= Open;
    }




    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        lblIngotTip.text = "钻石低于" + HallModel.ingotBaseEnsureCondition + "颗时，点击兑换按钮，系统补满" + HallModel.ingotBaseEnsureCondition + "颗钻石。每次兑换需消耗" + HallModel.ingotBaseEnsureCoinCost + "金币!";
        lblCoinTip.text = "金币低于" + HallModel.coinBaseEnsureCondition + "时，点击领取按钮，系统增加" + HallModel.coinBaseEnsureCount + "金币，祝您游戏愉快！";

        if (HallModel.totalIngotBaseEnsureTimes > 0)
        {
            lblIngotEnsureTimes.text = "每日可兑换" + HallModel.totalIngotBaseEnsureTimes + "次，今日已兑换" + HallModel.currentIngotBaseEnsureTimes + "次";
        }
        else
        {
            lblIngotEnsureTimes.text = "";
        }
        if (HallModel.totalCoinBaseEnsureTimes > 0)
        {
            lblCoinEnsureTimes.text = "每日可领取" + HallModel.totalCoinBaseEnsureTimes + "次，今日已领取" + HallModel.currentCoinBaseEnsureTimes + "次";
        }
        else
        {
            lblCoinEnsureTimes.text = "";
        }
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

    void OnBtnExchangeClick()
    {
        if (HallModel.userDiamondCount > HallModel.ingotBaseEnsureCondition)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "您的钻石超过" + HallModel.ingotBaseEnsureCondition + "颗，无法进行兑换！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        HallService.Instance.GetBaseEnsure(0);
        Close();
    }

    void OnBtnGetAwardClick()
    {
        if (HallModel.userCoinInGame > HallModel.coinBaseEnsureCondition)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "您的金币超过" + HallModel.coinBaseEnsureCondition + "，无法领取补助！");
            return; 
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        HallService.Instance.GetBaseEnsure(1);
        Close();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }
}
