using UnityEngine;
using System.Collections;

public class HallDlgService : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnCopyWxAccount;
    private UIButton btnClose;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        btnCopyWxAccount = transform.Find("bg/btnCopyWxAccount").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnCopyWxAccount.onClick, OnBtnCopyWxAccountClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgService += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgService -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
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

    #region UI响应

    void OnBtnCopyWxAccountClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Util.CopyMessage(AppConfig.wxAccount);
        DoAction(GameEvent.V_OpenShortTip, "公众号已复制到剪切板！");
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }


    #endregion
}
