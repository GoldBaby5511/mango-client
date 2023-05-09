using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DlgFirstCharge : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnBuy;
    private UIButton btnClose;

    private UnityAction cancelAction;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnBuy = transform.Find("bg/btnBuy").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        //首充类型 5
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type != 5) continue;
            //首充描述信息
            string msg = "首充超值礼!\n仅需"+ info.RechargePrice +"元,立即获得"+info.Count+"钻石 + "+info.GiveCount+"金币!";
            bg.transform.Find("Label").GetComponent<UILabel>().text = msg;
            bg.transform.Find("Label0").GetComponent<UILabel>().text = "钻石*"+info.Count;
            bg.transform.Find("Label1").GetComponent<UILabel>().text = "金币*"+info.GiveCount;
            btnBuy.transform.Find("Label").GetComponent<UILabel>().text = info.RechargePrice.ToString();
        }
        
        EventDelegate.Add(btnBuy.onClick, OnBtnBuyClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgFirstCharge += Open;
        GameEvent.V_CloseDlgFirstCharge += Close;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgFirstCharge -= Open;
        GameEvent.V_CloseDlgFirstCharge -= Close;
    }

    //打开首充界面
    void Open(UnityAction cancelFunc)
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
        cancelAction = cancelFunc;
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


    //首充
    void OnBtnBuyClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        int goodId = -1;
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type == 5)
            {
                goodId = info.RechargeID;
                break;
            }
        }
        if (goodId < 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "没有商品信息！");
            return;
        }
        DoAction(GameEvent.V_OpenDlgPayMode, goodId);
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
        DoActionDelay(cancelAction, 0.3f);
        //DoAction(HallEvent.V_OpenDlgStore, DlgStoreArg.DiamondPage);
        //DoActionDelay(HallEvent.V_OpenDlgActivity, 0.2f);
    }


}
