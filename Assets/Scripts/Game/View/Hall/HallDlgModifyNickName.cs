using UnityEngine;
using System.Collections;

public class HallDlgModifyNickName : View
{
    private Transform bg;
    private Transform shade;

    private UIInput inputNickName;

    private UIButton btnConfirm;
    private UIButton btnClose;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        inputNickName = transform.Find("bg/inputNickName").GetComponent<UIInput>();

        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();

        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgModifyNickName += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgModifyNickName -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
        inputNickName.value = "";
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

    void OnBtnConfirmClick()
    {
        string nickName = inputNickName.value;
        if (nickName.Length >= 1 && nickName.Length < 20)
        {
            AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
            HallService.Instance.ModifyUserNickName(nickName);
        }
        else
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "用户昵称必须包含1~20个字符！");
            return;
        }
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
        DoActionDelay(HallEvent.V_OpenDlgUserInfo, 0.1f);
    }

    
    #endregion
}
