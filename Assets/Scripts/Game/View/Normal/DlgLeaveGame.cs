using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DlgLeaveGame : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnContinue;
    private UIButton btnLeave;
    private UIButton btnStore;
    private UIButton btnMouthCard;
    private UIButton btnFirstCharge;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnContinue = transform.Find("bg/btnContinue").GetComponent<UIButton>();
        btnLeave = transform.Find("bg/btnLeave").GetComponent<UIButton>();
        btnStore = transform.Find("bg/btnStore").GetComponent<UIButton>();
        btnMouthCard = transform.Find("bg/btnMouthCard").GetComponent<UIButton>();
        btnFirstCharge = transform.Find("bg/btnFirstCharge").GetComponent<UIButton>();


        EventDelegate.Add(btnContinue.onClick, OnBtnContinueClick);
        EventDelegate.Add(btnLeave.onClick, OnBtnLeaveClick);
        EventDelegate.Add(btnStore.onClick, OnBtnStoreClick);
        EventDelegate.Add(btnMouthCard.onClick, OnBtnMouthCardClick);
        EventDelegate.Add(btnFirstCharge.onClick, OnBtnFirstChargeClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenLeaveGame += Open;
        GameEvent.V_CloseLeaveGame += Close;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenLeaveGame -= Open;
        GameEvent.V_CloseLeaveGame -= Close;
    }

    //打开界面
    void Open()
    {
        string message;
        if(GameModel.currentRedPackCount >= GameModel.totalRedPackCount)
        {
            message = "局数已满坐等红包雨,确定离开？";
        }
        else
        {
            message = "再玩" + (GameModel.totalRedPackCount - GameModel.currentRedPackCount) + "局即可喜迎红包雨,确定离开？";
        }
        bg.transform.Find("Label0").GetComponent<UILabel>().text = message;

        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
    }

    void Close()
    {
        if(gameObject.activeSelf == false) return;
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }


    //继续游戏
    void OnBtnContinueClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
    }

    //离开游戏
    void OnBtnLeaveClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        GameService.Instance.UserStand();
    }

    //商店
    void OnBtnStoreClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        GameEvent.V_OpenDlgStoreInGame.Invoke(DlgStoreArg.DiamondPage);
    }

    //月卡
    void OnBtnMouthCardClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
    }

    //首充
    void OnBtnFirstChargeClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        //首充判断
        if(HallModel.isFirstCharge == false)
        {
            //Util.Instance.DoAction(GameEvent.V_OpenDlgFirstCharge, delegate { GameEvent.V_OpenDlgStoreInGame.Invoke(DlgStoreArg.DiamondPage); });
            Util.Instance.DoAction(GameEvent.V_OpenDlgFirstCharge, null);
        }
        else
        {
            //打开商城
            GameEvent.V_OpenDlgTip.Invoke("首充今日已使用,快去商城充值吧！", "", delegate { GameEvent.V_OpenDlgStoreInGame.Invoke(DlgStoreArg.DiamondPage); }, null);
            
        }
    }


}
