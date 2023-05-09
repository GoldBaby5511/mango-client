using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using DG.Tweening;

public class DlgTip : View
{
    private GameObject dialog;
    private GameObject shade;
    private UIButton btnConfirm;
    private UIButton btnClose;
    private UILabel lblMainTip;
    private UILabel lblSubTip;

    private GameObject shortTip;
    private GameObject connectTip;


    private UnityAction confirmAction;
    private UnityAction cancelAction;

    public override void Init()
    {
        dialog = transform.Find("dialog").gameObject;
        shade = transform.Find("shade").gameObject;
        btnConfirm = transform.Find("dialog/btnConfirm").GetComponent<UIButton>();
        btnClose = transform.Find("dialog/btnClose").GetComponent<UIButton>();
        lblMainTip = transform.Find("dialog/lblMainTip").GetComponent<UILabel>();
        lblSubTip = transform.Find("dialog/lblSubTip").GetComponent<UILabel>();

        shortTip = transform.Find("shortTip").gameObject;
        connectTip = transform.Find("connectTip").gameObject;

        //添加监听
        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        //初始化
        gameObject.SetActive(true);
        dialog.SetActive(false);
        shade.SetActive(false);
        dialog.GetComponent<TweenScale>().ResetToBeginning();
        dialog.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
        shortTip.SetActive(false);
        connectTip.SetActive(false);
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgTip += OpenDlgTip;
        GameEvent.V_OpenShortTip += OpenShortTip;
        GameEvent.V_OpenConnectTip += OpenConnectTip;
        GameEvent.V_CloseConnectTip += CloseConnnectTip;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgTip -= OpenDlgTip;
        GameEvent.V_OpenShortTip -= OpenShortTip;
        GameEvent.V_OpenConnectTip -= OpenConnectTip;
        GameEvent.V_CloseConnectTip -= CloseConnnectTip;
    }

    void OpenDlgTip(string mainMsg, string subMsg, UnityAction confirmFunc, UnityAction cancelFunc = null)
    {
        dialog.SetActive(true);
        shade.SetActive(true);
        dialog.GetComponent<TweenAlpha>().PlayForward();
        dialog.GetComponent<TweenScale>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
        lblMainTip.text = mainMsg;
        lblSubTip.text = subMsg;
        confirmAction = confirmFunc;
        cancelAction = cancelFunc;
    }

    void CloseDlgTip()
    {
        dialog.GetComponent<TweenAlpha>().PlayReverse();
        dialog.GetComponent<TweenScale>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        Invoke("CloseDlgTipCor", 0.3f);
    }

    void CloseDlgTipCor()
    {
        dialog.SetActive(false);
        shade.SetActive(false);
    }

    void OpenShortTip(string msg)
    {
        shortTip.SetActive(true);
        shortTip.GetComponent<UILabel>().text = msg;
        shortTip.GetComponent<TweenAlpha>().ResetToBeginning();
        shortTip.GetComponent<TweenScale>().ResetToBeginning();
        shortTip.GetComponent<TweenScale>().PlayForward();
        shortTip.GetComponent<TweenAlpha>().PlayForward();
        if (IsInvoking("CloseShortTip"))
        {
            CancelInvoke("CloseShortTip");
        }
        Invoke("CloseShortTip", 2f);
    }

    void CloseShortTip()
    {
        shortTip.SetActive(false);
    }

   
    //连接提示
    void OpenConnectTip(string msg)
    {
        connectTip.SetActive(true);
        connectTip.GetComponent<UILabel>().text = msg;
        if (IsInvoking("CloseConnnectTip"))
        {
            CancelInvoke("CloseConnnectTip");
        }
        Invoke("CloseConnnectTip", 8f);
    }

    //关闭连接提示
    void CloseConnnectTip()
    {
        connectTip.SetActive(false);
    }


    void OnBtnConfirmClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        CloseDlgTip();
        DoActionDelay(confirmAction, 0.3f);
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        CloseDlgTip();
        DoActionDelay(cancelAction, 0.3f);
    }

}
